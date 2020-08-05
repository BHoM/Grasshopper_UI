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
    public abstract partial class CallerComponent : GH_Component, IGH_VariableParameterComponent, IGH_InitCodeAware
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public abstract Caller Caller { get; }

        public DataAccessor_GH Accessor = null;

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return Caller.Icon_24x24; } }

        public override Guid ComponentGuid { get { return Caller.Id; } }

        public override GH_Exposure Exposure { get { return (GH_Exposure)Math.Pow(2, Caller.GroupIndex); } }

        public override bool Obsolete
        {
            get
            {
                return (Caller?.SelectedItem == null) ? false : Caller.SelectedItem.IIsDeprecated();
            }
        }


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
    }
}

