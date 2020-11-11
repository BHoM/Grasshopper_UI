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

            // Update the component details
            UpdateComponentDetails(update.ComponentUpdate);

            // Update the inputs
            update.InputUpdates.ForEach(x => UpdateInput(x as dynamic));

            // Update the outputs
            update.OutputUpdates.ForEach(x => UpdateOutput(x as dynamic));

            // Ask component to refresh
            Params.OnParametersChanged();
            ExpireSolution(true);
        }

        /*******************************************/
        /**** Component Update Methods          ****/
        /*******************************************/

        protected virtual void UpdateComponentDetails(ComponentUpdate update)
        {
            if (update != null)
            {
                Name = update.Name;
                NickName = update.Name;
                Description = update.Description;
            }
        }


        /*******************************************/
        /**** Input Update Methods              ****/
        /*******************************************/

        protected virtual void UpdateInput(ParamAdded update)
        {
            IGH_Param newParam = update.Param.ToGH_Param();

            // If there is already a param with the same name, delete it but keep the wire connections
            // Same approach as `UpdateInput(ParamUpdated update)` but with index provided by ParamAdded
            IGH_Param match = Params.Input.Find(x => x.Name == update.Name);
            if (match != null)
            {
                MoveLinks(match, newParam);
                Params.UnregisterInputParameter(match);
            }

            Params.RegisterInputParam(newParam, update.Index);
        }

        /*******************************************/

        protected virtual void UpdateInput(ParamRemoved update)
        {
            IGH_Param match = Params.Input.Find(x => x.Name == update.Name);
            if (match != null)
                Params.UnregisterInputParameter(match);
        }

        /*******************************************/

        protected virtual void UpdateInput(ParamUpdated update)
        {
            int index = Params.Input.FindIndex(x => x.Name == update.Name);
            if (index >= 0)
            {
                IGH_Param oldParam = Params.Input[index];
                IGH_Param newParam = update.Param.ToGH_Param();

                MoveLinks(oldParam, newParam);
                newParam.DataMapping = oldParam.DataMapping;

                Params.UnregisterInputParameter(oldParam);
                Params.RegisterInputParam(newParam, index);
            }    
        }

        /*******************************************/

        protected virtual void UpdateInput(ParamMoved update)
        {
            int index = Params.Input.FindIndex(x => x.Name == update.Name);
            if (index >= 0)
            {
                IGH_Param param = Params.Input[index];
                Params.Input.RemoveAt(index);
                Params.Input.Insert(update.Index, param);
            }
        }

        /*******************************************/

        protected virtual void UpdateInput(IParamUpdate update)
        {
            // Do nothing
        }


        /*******************************************/
        /**** Output Update Methods             ****/
        /*******************************************/

        protected virtual void UpdateOutput(ParamAdded update)
        {
            IGH_Param newParam = update.Param.ToGH_Param();

            // If there is already a param with the same name, delete it but keep the wire connections
            // Same approach as `UpdateOutput(ParamUpdated update)` but with index provided by ParamAdded
            IGH_Param match = Params.Output.Find(x => x.Name == update.Name);
            if (match != null)
            {
                newParam.NewInstanceGuid(match.InstanceGuid);
                Params.UnregisterOutputParameter(match);
            }

            Params.RegisterOutputParam(newParam, update.Index);
        }

        /*******************************************/

        protected virtual void UpdateOutput(ParamRemoved update)
        {
            IGH_Param match = Params.Output.Find(x => x.Name == update.Name);
            if (match != null)
                Params.UnregisterOutputParameter(match);
        }

        /*******************************************/

        protected virtual void UpdateOutput(ParamUpdated update)
        {
            int index = Params.Output.FindIndex(x => x.Name == update.Name);
            if (index >= 0)
            {
                IGH_Param oldParam = Params.Output[index];
                IGH_Param newParam = update.Param.ToGH_Param();

                // The Recipient property is still empty at this stage so we need to make sure recipient params are still finding their source 
                newParam.NewInstanceGuid(oldParam.InstanceGuid);
                newParam.DataMapping = oldParam.DataMapping;

                Params.UnregisterOutputParameter(oldParam);
                Params.RegisterOutputParam(newParam, index);
            }
        }

        /*******************************************/

        protected virtual void UpdateOutput(ParamMoved update)
        {
            int index = Params.Output.FindIndex(x => x.Name == update.Name);
            if (index >= 0)
            {
                IGH_Param param = Params.Output[index];
                Params.Output.RemoveAt(index);
                Params.Output.Insert(update.Index, param);
            }
        }

        /*******************************************/

        protected virtual void UpdateOutput(IParamUpdate update)
        {
            // Do nothing
        }


        /*******************************************/
        /**** Helper Methods                    ****/
        /*******************************************/

        protected virtual void MoveLinks(IGH_Param oldParam, IGH_Param newParam)
        {
            foreach(IGH_Param source in oldParam.Sources)
                            newParam.AddSource(source);
            foreach (IGH_Param target in oldParam.Recipients)
                target.AddSource(newParam);

            oldParam.IsolateObject();
        }

        /*******************************************/
    }
}

