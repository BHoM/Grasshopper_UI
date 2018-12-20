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

//using Grasshopper.GUI.Canvas;
//using System;
//using System.IO;
//using System.Reflection;

//namespace BH.UI.Grasshopper.Menus
//{
//    public static class SearchPopup
//    {
//        /*******************************************/
//        /**** Public Methods                    ****/
//        /*******************************************/

//        public static void Activate()
//        {
//            Grasshopper.Instances.CanvasCreated += Instances_CanvasCreated;
//        }


//        /*******************************************/
//        /**** Event Methods                     ****/
//        /*******************************************/

//        public static void Instances_CanvasCreated(GH_Canvas canvas)
//        {
//            string folder = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Grasshopper\Libraries\Grasshopper\";
//            BH.Engine.Reflection.Compute.LoadAllAssemblies(folder);

//            canvas.KeyDown += ActiveCanvas_KeyDown;
//        }

//        /*******************************************/

//        public static void ActiveCanvas_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
//        {
//            bool modCtrl = (System.Windows.Forms.Control.ModifierKeys & System.Windows.Forms.Keys.Control) == System.Windows.Forms.Keys.Control;
//            bool modShift = (System.Windows.Forms.Control.ModifierKeys & System.Windows.Forms.Keys.Shift) == System.Windows.Forms.Keys.Shift;

//            if (e.KeyCode == System.Windows.Forms.Keys.B && modCtrl && !modShift)
//            {
//                GH_Canvas canvas = Grasshopper.Instances.ActiveCanvas;
//                System.Drawing.Point position = System.Windows.Forms.Cursor.Position;

//                new BH_PopupSearchDialog { BasePoint = position, Canvas = canvas }.Show(canvas.FindForm());
//            }
//        }

//        /*******************************************/
//    }
//}
