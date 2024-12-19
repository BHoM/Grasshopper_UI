/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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

using BH.Engine.Grasshopper;
using BH.Engine.Programming;
using BH.oM.Programming;
using BH.UI.Grasshopper.Templates;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using BH.UI.Base.Windows;
using GH = Grasshopper;

namespace BH.UI.Grasshopper.Components.UI
{
    public class Settings : GH_Component, ISettingsWindow
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return Properties.Resources.UISettings; } }

        public override Guid ComponentGuid { get { return new Guid("d5f5846c-1a83-426e-8d0d-3479cb8e3ed1"); } }

        public override GH_Exposure Exposure { get { return GH_Exposure.primary; } }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public Settings() : base("BHoM UI Settings", "BHoM UI Settings", "Modify BHoM interactions with Grasshopper with these UI Settings", "BHoM", "UI")
        {
        }

        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("openSettings", "openSettings", "Set to 'true' to open the BHoM UI Settings window.", GH_ParamAccess.item, false);
        }

        /*******************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            
        }

        /*******************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BH.Engine.Base.Compute.ClearCurrentEvents();
            
            bool input = false;
            DA.GetData(0, ref input);

            if (input && !m_IsOpen)
            {
                try
                {
                    Thread t = new Thread(() => ShowProjectCaptureWindow(this));
                    t.SetApartmentState(ApartmentState.STA);
                    t.Start();
                    m_IsOpen = true;

                    Helpers.ShowEvents(this, BH.Engine.Base.Query.CurrentEvents());
                }
                catch (Exception e)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, e.Message);
                }
            }
        }

        /*******************************************/

        private static void ShowProjectCaptureWindow(ISettingsWindow uiParent)
        {
            BH.UI.Base.Windows.Settings.SearchSettingsWindow window = new BH.UI.Base.Windows.Settings.SearchSettingsWindow(uiParent);
        }

        /*******************************************/

        public void OnPopUpClose()
        {
            m_IsOpen = false;
        }

        /*******************************************/

        private bool m_IsOpen = false;
    }
}
