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

namespace BH.UI.Grasshopper.Components
{
    public class PrototypeAttribute : GH_ComponentAttributes​
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public bool Visible { get; set; } = false;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public PrototypeAttribute(GH_Component owner) : base(owner) { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void Layout()
        {
            base.Layout();

            if (Visible)
            {
                m_LabelBounds = new RectangleF(Bounds.X, Bounds.Bottom, Bounds.Width, m_LabelHeight);
                Bounds = new RectangleF(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height + m_LabelHeight);
            }
        }

        /*******************************************/

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            base.Render(canvas, graphics, channel);

            // Only draw if visible and on hte correct channel
            if (!Visible || channel != GH_CanvasChannel.Objects)
                return;

            this.RenderPrototypeLabel(graphics, m_LabelBounds, this.Owner.Params.Input.Count == 0, this.Owner.Params.Output.Count == 0, m_DrawStrips);
        }

        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        RectangleF m_LabelBounds;
        int m_LabelHeight = 17;
        bool m_DrawStrips = true;

        /*******************************************/
    }
}




