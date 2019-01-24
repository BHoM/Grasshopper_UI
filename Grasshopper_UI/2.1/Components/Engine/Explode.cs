/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
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
using GH = Grasshopper;
using Grasshopper.Kernel;
using BH.oM.Base;
using BH.UI.Grasshopper.Base;
using BH.UI.Grasshopper.Templates;
using BH.UI.Templates;
using BH.UI.Components;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BH.UI.Grasshopper.Components
{
    public class ExplodeComponent : CallerComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Caller Caller { get; } = new ExplodeCaller();


        /*******************************************/
        /**** Public Override Methods           ****/
        /*******************************************/

        public override void OnBHoMUpdates(object sender = null, object e = null)
        {
            RecordUndoEvent("OnBHoMUpdates");
            // Forces the component to update
            this.RegisterOutputParams();
            this.Params.OnParametersChanged();
            this.ExpireSolution(true);
        }

        /*******************************************/

        public override void OnGrasshopperUpdates(object sender = null, object e = null)
        {
            // Update the output params based on input data
            Params.Input[0].CollectData();
            List<object> data = Params.Input[0].VolatileData.AllData(true).Select(x => x.ScriptVariable()).ToList();
            ExplodeCaller caller = Caller as ExplodeCaller;
            caller.CollectOutputTypes(data);
        }


        /*******************************************/
        /**** Protected Override Methods        ****/
        /*******************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            this.OnGrasshopperUpdates();

            if (this.IsExpired())
            {
                Engine.Reflection.Compute.ClearCurrentEvents();
                Engine.Reflection.Compute.RecordWarning("Output paramters do not match object properties. Please right click and <Update Outputs>");
                Logging.ShowEvents(this, BH.Engine.Reflection.Query.CurrentEvents());
            }
            else
            {
                base.SolveInstance(DA);
            }
        }

        /*******************************************/

        protected override void AfterSolveInstance()
        {
            if (Caller.OutputParams.Count != 0 & Params.Output.Count == 0)
                this.OnBHoMUpdates();
            base.BeforeSolveInstance();
        }

        /*******************************************/

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            Menu_AppendItem(menu, "Update Outputs", OnBHoMUpdates);
            base.AppendAdditionalComponentMenuItems(menu);
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        private bool IsExpired()
        {
            if (Caller.OutputParams.Count == 0 | Params.Output.Count == 0)
            {
                return true;
            }

            if (Caller.OutputParams.Count != Params.Output.Count)
            {
                return true;
            }

            bool expired = false;
            for (int i = 0; i < Caller.OutputParams.Count; i++)
            {
                expired |= Caller.OutputParams[i].Name != Params.Output[i].NickName;
                if (expired)
                {
                    return true;
                }
            }
            return false;
        }

        /*******************************************/
    }
}
