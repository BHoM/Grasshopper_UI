/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System;
using System.Linq;
using Grasshopper.Kernel;
using BH.oM.Base;
using BH.oM.UI;
using System.Collections.Generic;
using System.Windows.Forms;
using BH.UI.Grasshopper.Global;
using BH.oM.Reflection;
using Grasshopper.Kernel.Parameters;
using BH.UI.Grasshopper.Parameters;
using BH.Engine.Reflection;
using BH.oM.Geometry;
using BH.Engine.Grasshopper;
using BH.UI.Grasshopper.Components;
using System.Collections;
using BH.Adapter;
using BH.oM.Reflection.Debugging;
using BH.UI.Base;

namespace BH.UI.Grasshopper.Templates
{
    public abstract class CallerComponent : GH_Component, IGH_VariableParameterComponent, IGH_InitCodeAware
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public abstract Caller Caller { get; }

        public DataAccessor_GH Accessor = null;

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return Caller.Icon_24x24; } }

        public override Guid ComponentGuid { get { return Caller.Id; } }

        public override GH_Exposure Exposure { get { return (GH_Exposure)Math.Pow(2, Caller.GroupIndex); } }

        public override bool Obsolete => this.IsObsolete();


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public CallerComponent() : base()
        {
            NewInstanceGuid(System.Guid.Empty);
            PostConstructor();  // Explicitly calling PostConstructor() since it is not called in the parameterless constructor overload base()
            NewInstanceGuid();

            Name = Caller.Name;
            NickName = Caller.Name;
            Description = Caller.Description;
            Category = "BHoM";
            SubCategory = Caller.Category;

            Accessor = new DataAccessor_GH(Params.Input, IsValidPrincipalParameterIndex ? PrincipalParameterIndex : -1);
            Caller.SetDataAccessor(Accessor);

            Caller.Modified += OnCallerModified;
            Caller.SolutionExpired += (sender, e) => ExpireSolution(true);
        }

        /*******************************************/

        static CallerComponent()
        {
            GlobalSearchMenu.Activate();
        }


        /*******************************************/
        /**** GH_Component Override Methods     ****/
        /*******************************************/

        protected override void RegisterInputParams(GH_InputParamManager pManager = null)
        {
            if (Caller == null)
                return;

            List<ParamInfo> inputs = Caller.InputParams;

            if (Caller.WasUpgraded)
            {
                Type fragmentType = typeof(ParamOldIndexFragment);
                List<IGH_Param> oldParams = Params.Input.ToList();
                Params.Input.Clear();
                for (int i = 0; i < inputs.Count; i++)
                {
                    ParamOldIndexFragment fragment = null;
                    if (inputs[i].Fragments.Contains(fragmentType))
                        fragment = inputs[i].Fragments[fragmentType] as ParamOldIndexFragment;

                    if (fragment == null || fragment.OldIndex < 0)
                        Params.RegisterInputParam(inputs[i].ToGH_Param());
                    else
                        Params.RegisterInputParam(oldParams[fragment.OldIndex]);
                }
            }
            else
            {
                int nbNew = inputs.Count;
                int nbOld = Params.Input.Count;

                for (int i = 0; i < Math.Min(nbNew, nbOld); i++)
                {
                    IGH_Param newParam = inputs[i].ToGH_Param();
                    if (newParam.GetType() != Params.Input[i].GetType())
                        Params.Input[i] = newParam;
                }

                for (int i = nbOld - 1; i >= nbNew; i--)
                    Params.UnregisterInputParameter(Params.Input[i]);

                for (int i = nbOld; i < nbNew; i++)
                    Params.RegisterInputParam(inputs[i].ToGH_Param());
            }

        }

        /*******************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager = null)
        {
            if (Caller == null)
                return;

            List<ParamInfo> outputs = Caller.OutputParams.Where(x => x.IsSelected).ToList();

            if (Caller.WasUpgraded)
            {
                Type fragmentType = typeof(ParamOldIndexFragment);
                List<IGH_Param> oldParams = Params.Output.ToList();
                Params.Output.Clear();
                for (int i = 0; i < outputs.Count; i++)
                {
                    ParamOldIndexFragment fragment = null;
                    if (outputs[i].Fragments.Contains(fragmentType))
                        fragment = outputs[i].Fragments[fragmentType] as ParamOldIndexFragment;

                    if (fragment == null || fragment.OldIndex < 0)
                        Params.RegisterOutputParam(outputs[i].ToGH_Param());
                    else
                        Params.RegisterOutputParam(oldParams[fragment.OldIndex]);
                }
            }
            else
            {
                int nbNew = outputs.Count;
                int nbOld = Params.Output.Count;

                for (int i = 0; i < Math.Min(nbNew, nbOld); i++)
                {
                    IGH_Param oldParam = Params.Output[i];
                    IGH_Param newParam = outputs[i].ToGH_Param();
                    if (newParam.GetType() != oldParam.GetType() || newParam.NickName != oldParam.NickName)
                    {
                        foreach (IGH_Param source in oldParam.Sources)
                            newParam.AddSource(source);
                        foreach (IGH_Param target in oldParam.Recipients)
                            target.AddSource(newParam);

                        oldParam.IsolateObject();
                        Params.Output[i] = newParam;
                    }
                    else if (newParam.Description != oldParam.Description)
                        oldParam.Description = newParam.Description;
                }

                for (int i = nbOld - 1; i >= nbNew; i--)
                    Params.UnregisterOutputParameter(Params.Output[i]);

                for (int i = nbOld; i < nbNew; i++)
                    Params.RegisterOutputParam(outputs[i].ToGH_Param());
            }
        }

        /*******************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Accessor.GH_Accessor = DA;
            Caller.Run();

            List<Event> events = Engine.Reflection.Query.CurrentEvents();
            Helpers.ShowEvents(this, events);

            if (DA.Iteration == 0)
                Engine.UI.Compute.LogUsage("Grasshopper", InstanceGuid, Caller.SelectedItem, events);
        }

        /*******************************************/

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            Menu_AppendItem(menu, "Source code", OnSourceCodeClick, Properties.Resources.BHoM_Logo);
            Caller.AddToMenu(menu);
        }

        /*******************************************/

        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            writer.SetString("Component", Caller.Write());
            return base.Write(writer);
        }

        /*************************************/

        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            if (!base.Read(reader) || !Params.Read(reader))
                return false;

            string callerString = ""; reader.TryGetString("Component", ref callerString);
            Caller.Read(callerString);

            return true;
        }

        /*******************************************/

        public override IList<string> RuntimeMessages(GH_RuntimeMessageLevel level)
        {
            //Make sure to extract both 'Remark' as well as 'Blank' messages when remarks are extracted.
            //This is because we are adding 'Blank' messages instead of 'Remarks' when there are any warnings/errors present
            //to make sure the component colouring is correct.
            //See more comments in the "Logging.cs" file.
            if (level == GH_RuntimeMessageLevel.Remark)
            {
                List<string> remarks = base.RuntimeMessages(level).ToList();
                remarks.AddRange(base.RuntimeMessages(GH_RuntimeMessageLevel.Blank));
                return remarks;
            }
            return base.RuntimeMessages(level);
        }


        /*******************************************/
        /**** IGH_InitCodeAware Methods         ****/
        /*******************************************/

        public void SetInitCode(string code)
        {
            object item = BH.Engine.Serialiser.Convert.FromJson(code);
            if (item != null)
                Caller.SetItem(item);
            this.OnItemSelected();
        }

        /*******************************************/
        /**** IGH_VariableParameterComponent    ****/
        /*******************************************/

        public virtual bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            return false;
        }

        /*************************************/

        public virtual bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            return false;
        }

        /*************************************/

        public virtual IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            return new Param_GenericObject();
        }

        /*************************************/

        public virtual bool DestroyParameter(GH_ParameterSide side, int index)
        {
            return true;
        }

        /*************************************/

        public virtual void VariableParameterMaintenance() { }


        /*******************************************/
        /**** Others                            ****/
        /*******************************************/

        public virtual void OnCallerModified(object sender, CallerUpdate update)
        {
            if (update == null)
                return;

            switch (update.Cause)
            {
                case CallerUpdateCause.ItemSelected:
                    OnItemSelected(sender, update);
                    return;
                case CallerUpdateCause.InputSelection:
                    OnInputSelection(update.InputUpdates);
                    return;
                case CallerUpdateCause.OutputSelection:
                    OnOutputSelection(update.OutputUpdates);
                    return;
                case CallerUpdateCause.ReadFromSave:
                    OnInputSelection(update.InputUpdates);
                    OnOutputSelection(update.OutputUpdates);
                    break;
                default:
                    return;
            }

        }

        /*******************************************/

        public virtual void OnItemSelected(object sender = null, CallerUpdate update = null)
        {
            Name = Caller.Name;
            NickName = Caller.Name;
            Description = Caller.Description;

            this.RegisterInputParams(null); // Cannot use PostConstructor() here since it calls CreateAttributes() without attributes, resetting the stored ones
            this.RegisterOutputParams(null); // We call its bits individually: input, output 
            this.Params.OnParametersChanged(); // and ask to update the layout with OnParametersChanged()

            this.ExpireSolution(true);
        }

        /*******************************************/

        public void OnInputSelection(List<IParamUpdate> updates)
        {
            List<ParamInfo> selection = Caller.InputParams;

            int index = 0;
            for (int i = 0; i < selection.Count; i++)
            {
                if (selection[i].IsSelected)
                {
                    if (index >= Params.Input.Count || selection[i].Name != Params.Input[index].Name)
                        Params.RegisterInputParam(selection[i].ToGH_Param(), index);
                    index++;
                }
                else
                {
                    if (index < Params.Input.Count && selection[i].Name == Params.Input[index].Name)
                        Params.UnregisterInputParameter(Params.Input[index]);
                }
            }

            Params.OnParametersChanged();
            ExpireSolution(true);
        }

        /*******************************************/

        public void OnOutputSelection(List<IParamUpdate> updates)
        {
            List<ParamInfo> selection = Caller.OutputParams;

            int index = 0;
            for (int i = 0; i < selection.Count; i++)
            {
                if (selection[i].IsSelected)
                {
                    if (index >= Params.Output.Count || selection[i].Name != Params.Output[index].Name)
                        Params.RegisterOutputParam(selection[i].ToGH_Param(), index);
                    index++;
                }
                else
                {
                    if (index < Params.Output.Count && selection[i].Name == Params.Output[index].Name)
                        Params.UnregisterOutputParameter(Params.Output[index]);
                }
            }

            Params.OnParametersChanged();
            ExpireSolution(true);
        }

        /*******************************************/

        public virtual void OnSourceCodeClick(object sender = null, object e = null)
        {
            if (Caller != null)
            {
                BH.Engine.Reflection.Compute.IOpenHelpPage(Caller.SelectedItem);
            }
        }

        /*******************************************/

        public virtual void OnGrasshopperUpdates(object sender, GH_ParamServerEventArgs e)
        {
            if (Caller == null)
                return;

            if (e?.Parameter == null || e?.ParameterIndex == -1 || Caller?.InputParams.Count - 1 < e.ParameterIndex)
                return;

            // We recompute only if there is no other scheduled solution running or the update does not come from an explode, which will cause a crash
            // we also avoid recomputing if we just reconnected the same wire
            bool recompute = this.Phase == GH_SolutionPhase.Computed
                             && !e.Parameter.Sources.Any(p => p.Attributes.GetTopLevel.DocObject is ExplodeComponent)
                             && e.Parameter.NickName != Caller.InputParams[e.ParameterIndex].Name;

            // Updating Caller.InputParams based on the new Grasshopper parameter just received
            // We update the InputParams with the new type or name
            Caller.UpdateInput(e.ParameterIndex, e.Parameter.NickName, e.Parameter.Type(Caller));

            // and expire because of the changes made
            ExpireSolution(recompute);
            return;
        }

        /*******************************************/

        public virtual bool IsObsolete()
        {
            if (this.Caller?.SelectedItem == null)
                return false;

            return this.Caller.SelectedItem.IIsDeprecated();
        }
        

        

        
        /*******************************************/
    }
}

