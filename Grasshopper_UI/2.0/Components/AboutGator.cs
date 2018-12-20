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
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace BH.UI.Grasshopper.Base
{
    public class AboutGator : GH_Component
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Properties.Resources.BHoM_Grasshopper_Icon; 

        public override Guid ComponentGuid { get; } = new Guid("{2f4c7d17-d7ce-48d3-a098-19300720eccb}");

        public override bool Obsolete { get; } = true;

        public override GH_Exposure Exposure { get; } = GH_Exposure.hidden;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public AboutGator() : base("Grasshopper", "Gator", "About Grasshopper", "Grasshopper", " About")
        {
            //Menus.SearchPopup.Activate();
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Execute", "E", "Execute?", GH_ParamAccess.item);
        }

        /*******************************************/

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_StringParam("String", "S", "String output", GH_ParamAccess.list);
        }

        /*******************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool Execute = true;

            if (!DA.GetData(0, ref Execute)) return;
            if (!Execute) return;

            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            String versionStamp = ConvertVersionToDateBasedVersionStamp(version);

            List<String> stringOut = new List<string>();

            stringOut.Add("Snap!");
            stringOut.Add("Grasshopper - " + versionStamp);

            DA.SetDataList(0, stringOut);
  
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        private static string ConvertVersionToDateBasedVersionStamp(Version version)
        {

            DateTime date = new DateTime(2000, 01, 01).AddDays(version.Build).AddSeconds(version.Revision * 2);

            String month = AddPreceedingZeroToUnitNumber(date.Month);
            String day = AddPreceedingZeroToUnitNumber(date.Day);
            String hour = AddPreceedingZeroToUnitNumber(date.Hour);
            String minute = AddPreceedingZeroToUnitNumber(date.Minute);

            String dateStamp = date.Year.ToString() + month + day + "." + hour + ":" + minute;

            return version.Major.ToString() + "." + version.Minor.ToString() + "." + dateStamp;

        }

        /*******************************************/

        private static string AddPreceedingZeroToUnitNumber(int num)
        {
            if (num < 10) return "0" + num.ToString();

            return num.ToString();
        }

        /*******************************************/
    }
}
