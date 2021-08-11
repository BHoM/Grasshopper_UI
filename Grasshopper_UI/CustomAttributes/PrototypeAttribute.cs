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
                m_LabelBounds = new RectangleF(Bounds.X, Bounds.Bottom - 1, Bounds.Width, m_LabelHeight);
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

            Color colour;
            try
            {
                // Define the colour of the render to match teh component's borders
                GH_Palette palette = GH_CapsuleRenderEngine.GetImpliedPalette(this.Owner);
                if (palette == GH_Palette.Normal && !this.Owner.IsPreviewCapable)
                {
                    palette = GH_Palette.Hidden;
                }
                GH_PaletteStyle style = GH_CapsuleRenderEngine.GetImpliedStyle(palette, this.Selected, this.Owner.Locked, this.Owner.Hidden);

                colour = style.Edge;
            }
            catch (Exception)
            {
                //Fallback to using black if the above crashes for any reason
                colour = Color.Black;
            }
  
            // Define render parameters
            Font font = GH_FontServer.Small;
            Pen linePen = new Pen(colour);
            linePen.Width = m_LabelHeight / 3;
            string labelText = "Prototype";
            SizeF stringSize = GH_FontServer.MeasureString(labelText, font);
            stringSize.Height -= 2;

            // Draw the label
            graphics.DrawString("Prototype", font, new SolidBrush(colour), m_LabelBounds, GH_TextRenderingConstants.CenterCenter);

            // Decide if we render strips or a simple line around the label
            if (m_DrawStrips)
            {
                // Define region to draw into
                Region clip = new Region(new RectangleF(m_LabelBounds.Location, new SizeF(m_LabelBounds.Width, m_LabelBounds.Height + 1)));
                clip.Exclude(new RectangleF(
                    new PointF(m_LabelBounds.X + (m_LabelBounds.Width - stringSize.Width) / 2, m_LabelBounds.Y + (m_LabelBounds.Height - stringSize.Height) / 2),
                    stringSize));
                graphics.SetClip(clip, CombineMode.Replace);

                // Draw the strips
                Pen stripPen = new Pen(colour, m_LabelHeight / 3);
                for (int dx = -m_LabelHeight / 2; dx < m_LabelBounds.Width; dx += m_LabelHeight)
                    graphics.DrawLine(stripPen,
                        new Point((int)m_LabelBounds.X + dx, (int)m_LabelBounds.Y + (int)m_LabelBounds.Height + 2),
                        new Point((int)m_LabelBounds.X + dx + m_LabelHeight + 4, (int)m_LabelBounds.Y - 2));

                // Clear the region filter
                graphics.ResetClip();
            }
            else
            {
                // Draw the lines aroudn the label
                float y = m_LabelBounds.Y + m_LabelBounds.Height / 2;
                graphics.DrawLine(linePen, m_LabelBounds.X + 1, y, m_LabelBounds.X + (m_LabelBounds.Width - stringSize.Width) / 2 - 4, y);
                graphics.DrawLine(linePen, m_LabelBounds.X + (m_LabelBounds.Width - stringSize.Width) / 2 + stringSize.Width + 4, y, m_LabelBounds.X + m_LabelBounds.Width - 1, y);
            }
        }

        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        RectangleF m_LabelBounds;
        int m_LabelHeight = 10;
        bool m_DrawStrips = true;

        /*******************************************/
    }
}


