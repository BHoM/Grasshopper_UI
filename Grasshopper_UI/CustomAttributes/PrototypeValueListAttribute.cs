/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2026, the respective contributors. All rights reserved.
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
using Grasshopper.Kernel.Special;
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
    public class PrototypeValueListAttribute : GH_ValueListAttributes
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public bool Visible { get; set; } = false;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public PrototypeValueListAttribute(GH_ValueList owner) : base(owner) { }

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
            if (channel != GH_CanvasChannel.Objects)
                return;

            if(Visible)
                this.RenderPrototypeLabel(graphics, m_LabelBounds, false, false, true);

            string message = (this.Owner as CallerValueList)?.Message;

            if (!string.IsNullOrWhiteSpace(message))
            {
                RenderMessage(graphics, message);
            }
        }

        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        private void RenderMessage(Graphics graphics, string message)
        {
            Color fill, text, edge;

            this.MessageLabelColours(out fill, out text, out edge);

            int zoomFadeMedium = GH_Canvas.ZoomFadeMedium;

            if (zoomFadeMedium > 5)
            {
                Rectangle box = new Rectangle((int)Bounds.X, (int)Bounds.Y, (int)Bounds.Width, (int)Bounds.Height);
                box.Inflate(-3, 0);
                box.Y = box.Bottom;
                Font font = GH_FontServer.Standard;

                bool flag = false;
                Size size = GH_FontServer.MeasureString(message, font);
                size.Width += 8;
                if (size.Width > box.Width)
                {
                    double num = (double)box.Width / (double)size.Width;
                    font = GH_FontServer.NewFont(font, Convert.ToSingle((double)font.SizeInPoints * num));
                    size = GH_FontServer.MeasureString(message, font);
                    flag = true;
                }
                box.Height = Math.Max(size.Height, 6);
                GraphicsPath graphicsPath = new GraphicsPath();
                graphicsPath.AddArc(box.Left - 3, box.Top, 6, 6, 270f, 90f);
                graphicsPath.AddArc(box.Left + 3, box.Bottom - 6, 6, 6, 180f, -90f);
                graphicsPath.AddArc(box.Right - 9, box.Bottom - 6, 6, 6, 90f, -90f);
                graphicsPath.AddArc(box.Right - 3, box.Top, 6, 6, 180f, 90f);
                graphicsPath.CloseAllFigures();
                SolidBrush solidBrush = new SolidBrush(Color.FromArgb(zoomFadeMedium, fill));
                Pen pen = new Pen(Color.FromArgb(zoomFadeMedium, edge))
                {
                    LineJoin = LineJoin.Bevel
                };
                graphics.FillPath(solidBrush, graphicsPath);
                graphics.DrawPath(pen, graphicsPath);
                pen.Dispose();
                solidBrush.Dispose();
                graphicsPath.Dispose();
                if (graphics.Transform.Elements[0].Equals(1f))
                {
                    graphics.TextRenderingHint = GH_TextRenderingConstants.GH_CrispText;
                }
                else
                {
                    graphics.TextRenderingHint = GH_TextRenderingConstants.GH_SmoothText;
                }
                SolidBrush solidBrush2 = new SolidBrush(Color.FromArgb(zoomFadeMedium, text));
                graphics.DrawString(message, font, solidBrush2, box, GH_TextRenderingConstants.CenterCenter);
                solidBrush2.Dispose();
                if (flag)
                {
                    font.Dispose();
                }
            }
        }

        /*******************************************/

        private void MessageLabelColours(out Color fill, out Color text, out Color edge)
        {
            try
            {
                // Define the colour of the render to match the component's borders
                GH_Palette palette = GH_CapsuleRenderEngine.GetImpliedPalette(this.Owner);

                if (palette == GH_Palette.Normal && !this.Owner.IsPreviewCapable)
                {
                    palette = GH_Palette.Hidden;
                }
                GH_PaletteStyle style = GH_CapsuleRenderEngine.GetImpliedStyle(palette, this.Selected, this.Owner.Locked, this.Owner.Hidden);
                edge = style.Edge;
                fill = style.Edge;

                if (!Visible)
                {
                    //If message is warning message, set fill to GH wraning colour.
                    //This is only done if the Prototype label is not already on the component, as extra warning colour is then not needed.
                    CallerValueList owner = (this.Owner as CallerValueList);
                    if (owner != null && owner.IsWarningMessage && !Visible)
                    {
                        if (this.Selected)
                            fill = Color.FromArgb(210, 230, 25);
                        else
                            fill = Color.FromArgb(241, 175, 29);
                    }
                }

                //Set text colour to contrast background colour
                text = GH_GraphicsUtil.ForegroundColour(fill, 200);
            }
            catch
            {
                fill = Color.Black;
                text = Color.White;
                edge = Color.Black;
            }
        }

        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        RectangleF m_LabelBounds;
        int m_LabelHeight = 17;

        /*******************************************/
    }
}





