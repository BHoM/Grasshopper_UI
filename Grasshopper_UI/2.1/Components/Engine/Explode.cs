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

        public bool AutoUpdateOutputs { get; set; } = true;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public ExplodeComponent() : base()
        {
            Params.ParameterChanged += (sender, e) => UpdateOutputs(false);
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();
            UpdateOutputs(false);
        }

        /*******************************************/

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            if (Params.Input.Count == 0)
                base.RegisterInputParams(pManager);
        }

        /*******************************************/

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            Menu_AppendItem(menu, "Update Outputs", RefreshLabel_Click);
            base.AppendAdditionalComponentMenuItems(menu);
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        private void RefreshLabel_Click(object sender, EventArgs e)
        {
            ExpireSolution(false);
            UpdateOutputs(true);
        }

        /*******************************************/

        private void UpdateOutputs(bool ignoreAutoUpdate)
        {
            bool doJob = ignoreAutoUpdate | AutoUpdateOutputs;
            if (!doJob)
            {
                Engine.Reflection.Compute.ClearCurrentEvents();
                Engine.Reflection.Compute.RecordWarning("Source component expired, right click and <Update Outputs> to update");
                Logging.ShowEvents(this, BH.Engine.Reflection.Query.CurrentEvents());
                return;
            }
            // Update the output params based on input data
            Params.Input[0].CollectData();
            List<object> data = Params.Input[0].VolatileData.AllData(true).Select(x => x.ScriptVariable()).ToList();
            ExplodeCaller caller = Caller as ExplodeCaller;
            caller.CollectOutputTypes(data);

            // Forces the component to update
            this.RegisterOutputParams(null);
            this.OnAttributesChanged();
        }

        /*******************************************/
    }
}
