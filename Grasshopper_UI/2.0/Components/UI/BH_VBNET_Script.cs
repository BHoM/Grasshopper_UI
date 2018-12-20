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

//using System;
//using System.Collections.Generic;
//using ScriptComponents;
//using Grasshopper.Kernel;
//using Grasshopper.Kernel.Parameters;
//using Grasshopper.Kernel.Parameters.Hints;
//using BH.UI.Grasshopper.GeometryHints;

//namespace BH.UI.Grasshopper.Base
//{
//    public class BH_VBNET_Script : Component_VBNET_Script
//    {
//        /*******************************************/
//        /**** Properties                        ****/
//        /*******************************************/

//        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Properties.Resources.VBH_Script; 

//        protected override System.Drawing.Bitmap Icon { get; } = Properties.Resources.VBH_Script; 

//        public override Guid ComponentGuid { get; } = new Guid("7fe983b6-5121-4c29-8157-6203923fbafb"); 

//        public override GH_Exposure Exposure { get; } = GH_Exposure.primary; 


//        /*******************************************/
//        /**** Constructors                      ****/
//        /*******************************************/

//        public BH_VBNET_Script() : base()
//        {
//            Name = "VBH Script"; NickName = "VBH"; Description = "A VB.Net scriptable component with BHoM custom features";
//            Category = "Grasshopper"; SubCategory = "Scripting";
//        }


//        /*******************************************/
//        /**** Override Methods                  ****/
//        /*******************************************/

//        protected override string CreateSourceForEdit(ScriptSource code)
//        {
//            return base.CreateSourceForEdit(code);
//        }

//        /*******************************************/

//        protected override string CreateSourceForCompile(ScriptSource script)
//        {
//            script.References.Clear();
//            string username = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1];
//            script.References.Add("C:\\Users\\" + username + "\\AppData\\Roaming\\Grasshopper\\Libraries\\Grasshopper\\BHoM.dll");
//            return base.CreateSourceForCompile(script);
//        }

//        /*******************************************/

//        protected override List<IGH_TypeHint> AvailableTypeHints
//        {
//            get
//            {
//                List<IGH_TypeHint> hints = base.AvailableTypeHints;
//                hints.Insert(11, new BH_PointHint());
//                hints.Insert(12, new BH_VectorHint());
//                hints.Insert(13, new BH_LineHint());
//                hints.Insert(14, new BH_PolylineHint());
//                hints.Insert(15, new BH_NurbCurveHint());
//                hints.Insert(16, new BH_MeshHint());
//                hints.Insert(17, new GH_HintSeparator());
//                return hints;
//            }
//        }

//        /*******************************************/
//    }
//}
