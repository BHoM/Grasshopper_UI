﻿/*
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

            Color colour;
            GH_Palette palette;
            GH_PaletteStyle style;
            try
            {
                // Define the colour of the render to match teh component's borders
                palette = GH_CapsuleRenderEngine.GetImpliedPalette(this.Owner);
                if (palette == GH_Palette.Normal && !this.Owner.IsPreviewCapable)
                {
                    palette = GH_Palette.Hidden;
                }
                style = GH_CapsuleRenderEngine.GetImpliedStyle(palette, this.Selected, this.Owner.Locked, this.Owner.Hidden);

                colour = style.Edge;
            }
            catch (Exception)
            {
                //Fallback to using black if the above crashes for any reason
                colour = Color.Black;
                palette = GH_Palette.Normal;
                style = new GH_PaletteStyle(colour, colour);
            }
  
            // Define render parameters
            Font font = GH_FontServer.StandardBold;
            Pen linePen = new Pen(colour);
            linePen.Width = m_LabelHeight / 2;
            string labelText = "Prototype";
            SizeF stringSize = GH_FontServer.MeasureString(labelText, font);
            stringSize.Height -= 4;
            Color yellowBackground = Color.FromArgb(255, 209, 2);


            // Decide if we render strips or a simple line around the label
            if (m_DrawStrips)
            {
                //Base capsule
                GH_Capsule capsule = GH_Capsule.CreateCapsule(this.Bounds, palette);
                bool jaggedLeft = this.Owner.Params.Input.Count == 0;
                bool jaggedRight = this.Owner.Params.Output.Count == 0;
                capsule.SetJaggedEdges(jaggedLeft, jaggedRight);

                float extraWidthBotRect = 10;

                // Define region to draw into
                Region clip = new Region(capsule.OutlineShape); //Outline of the base capsule
                RectangleF botRectangle = new RectangleF(m_LabelBounds.Location.X - extraWidthBotRect / 2, m_LabelBounds.Y, m_LabelBounds.Width + extraWidthBotRect, m_LabelBounds.Height);
                clip.Intersect(botRectangle);   //Intersect base capsule region with bottom rectangle
                graphics.SetClip(clip, CombineMode.Replace);

                //Draw the yellow background
                graphics.FillRectangle(new SolidBrush(yellowBackground), botRectangle);

                //Define the box around the text to cull
                float textHeight = stringSize.Height;
                float textWidth = stringSize.Width;
                stringSize.Width = Math.Max(textWidth, textHeight * 6.1f);  //To exactly hit the corners of the box, for 45degree angle, box should have aspectratio 1:6
                stringSize.Height = Math.Max(textHeight, textWidth / 6.1f); //To give a slight breathingspace, a aspectratio of 1:6.1 is set here
                RectangleF textBox = new RectangleF(
                    new PointF(m_LabelBounds.X + (m_LabelBounds.Width - stringSize.Width) / 2, m_LabelBounds.Y + (m_LabelBounds.Height - stringSize.Height
                    ) / 2),
                    stringSize);
                clip.Exclude(textBox);
                graphics.SetClip(clip, CombineMode.Replace);

                //Set up parameters for stripes
                float width = textBox.Width / 6;
                float sqrt2 = (float)Math.Sqrt(2);
                Pen stripPen = new Pen(colour, width / sqrt2);  //45degree stripes
                int additionalSpace = (int)linePen.Width;
                float textBoxCentre = textBox.X + textBox.Width / 2;
                float y1 = m_LabelBounds.Y + m_LabelBounds.Height + additionalSpace;
                float y2 = m_LabelBounds.Y - additionalSpace;
                float deltaY = y1 - y2;
                //Draw stripes to the right
                for (float dx = -deltaY / 2; dx < botRectangle.Width; dx += width * 2)
                {
                    graphics.DrawLine(stripPen, textBoxCentre + dx, y1, textBoxCentre + dx + deltaY, y2);
                }

                //Draw stripes to the left
                for (float dx = -deltaY / 2 - width * 2; dx > -botRectangle.Width; dx -= width * 2)
                {
                    graphics.DrawLine(stripPen, textBoxCentre + dx, y1, textBoxCentre + dx + deltaY, y2);
                }

                // Clear the region filter
                graphics.ResetClip();

                RectangleF textRenderBounds = m_LabelBounds;
                textRenderBounds.Y -= 1;
                // Draw the label
                graphics.DrawString("Prototype", font, new SolidBrush(colour), textRenderBounds, GH_TextRenderingConstants.CenterCenter);

                float zoom = graphics.Transform.Elements[0];
                capsule.RenderEngine.RenderOutlines(graphics, zoom, style);
            }
            else
            {
                // Draw the lines aroudn the label
                float y = m_LabelBounds.Y + m_LabelBounds.Height / 2;
                graphics.DrawLine(linePen, m_LabelBounds.X + 1, y, m_LabelBounds.X + (m_LabelBounds.Width - stringSize.Width) / 2 - 4, y);
                graphics.DrawLine(linePen, m_LabelBounds.X + (m_LabelBounds.Width - stringSize.Width) / 2 + stringSize.Width + 4, y, m_LabelBounds.X + m_LabelBounds.Width - 1, y);

                // Draw the label
                graphics.DrawString("Prototype", font, new SolidBrush(colour), m_LabelBounds, GH_TextRenderingConstants.CenterCenter);
            }


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


