/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using BH.oM.Base;
using BH.UI.Grasshopper.Templates;
using BH.UI.Base;
using BH.UI.Base.Components;
using Grasshopper.GUI.Canvas;
using System.Drawing;
using Grasshopper.GUI;
using System.Drawing.Drawing2D;
using BH.UI.Grasshopper.Components;

namespace BH.UI.Grasshopper.CustomAttributes
{
    public class FalseStartToggleAttributes : GH_ComponentAttributes
    {
        public FalseStartToggleAttributes(IGH_Component component) : base(component)
        {
        }

        /*******************************************/

        public override GH_ObjectResponse RespondToMouseDoubleClick(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            ((FalseStartToggleComponent)Owner).UpdateComponent();

            return GH_ObjectResponse.Handled;
        }

        /*******************************************/

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            if (channel == GH_CanvasChannel.Objects)
            {
                var value = (bool)((FalseStartToggleComponent)Owner).Caller.SelectedItem;

                //Cache the existing style.
                GH_PaletteStyle dstyle = GH_Skin.palette_hidden_standard;
                var hiddenSelected = GH_Skin.palette_hidden_selected;

                //Swap out palette for boolean toggle components based on the current output.
                /*if (value)
                {
                    GH_Skin.palette_hidden_standard = new GH_PaletteStyle(Color.ForestGreen);
                    GH_Skin.palette_hidden_selected = new GH_PaletteStyle(Color.ForestGreen);
                }
                else
                {
                    GH_Skin.palette_hidden_standard = new GH_PaletteStyle(Color.Firebrick);
                    GH_Skin.palette_hidden_selected = new GH_PaletteStyle(Color.Firebrick);
                }*/

                base.Render(canvas, graphics, channel);

                // Put the original style back.
                GH_Skin.palette_hidden_standard = dstyle;
                GH_Skin.palette_hidden_selected = hiddenSelected;
            }
            else
                base.Render(canvas, graphics, channel);
        }
    }
}
