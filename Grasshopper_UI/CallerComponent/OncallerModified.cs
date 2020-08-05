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
        /**** Event Methods                     ****/
        /*******************************************/

        protected virtual void OnCallerModified(object sender, CallerUpdate update)
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
        /**** Helper Methods                    ****/
        /*******************************************/

        protected virtual void OnItemSelected(object sender = null, CallerUpdate update = null)
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

        protected void OnInputSelection(List<IParamUpdate> updates)
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

        protected void OnOutputSelection(List<IParamUpdate> updates)
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
    }
}

