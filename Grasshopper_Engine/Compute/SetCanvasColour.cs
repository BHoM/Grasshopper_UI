/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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

using gh = Grasshopper;
using Grasshopper.Kernel.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BH.Engine.Grasshopper
{
    public static partial class Compute
    {
        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        [Description("Sets the display colour of Background, Grid and other elements in the Grasshopper UI.")]
        public static void SetCanvasColour(Color? backgroundCol = null, Color? gridCol = null, Color? wireDeselected = null, bool activate = false)
        {
            Color grid_default = Color.FromArgb(30, 0, 0, 0);
            Color bg_default = Color.FromArgb(255, 212, 208, 200);
            Color wireDeselected_default = Color.FromArgb(150, 0, 0, 0);

            if (!activate)
            {
                gh.GUI.Canvas.GH_Skin.canvas_grid = grid_default;
                gh.GUI.Canvas.GH_Skin.canvas_back = bg_default;
                gh.GUI.Canvas.GH_Skin.wire_default = wireDeselected_default;
            }
            else
            {
                gh.GUI.Canvas.GH_Skin.canvas_grid = gridCol ?? grid_default;
                gh.GUI.Canvas.GH_Skin.canvas_back = backgroundCol ?? bg_default;
                gh.GUI.Canvas.GH_Skin.wire_default = wireDeselected ?? wireDeselected_default;
            }
        }

        [Description("Attempts to reset the applied GH canvas colours by reading the grasshopper_gui.xml file if it exists.")]
        public static void ResetCanvasColours(bool activate = false)
        {
            if (activate)
                gh.GUI.Canvas.GH_Skin.LoadSkin();
        }
    }
}


