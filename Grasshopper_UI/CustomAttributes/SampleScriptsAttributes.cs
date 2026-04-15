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
using System.Drawing;
using System.Windows.Forms;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;

namespace BH.UI.Grasshopper.CustomAttributes
{
    public class SampleScriptsAttributes : GH_ComponentAttributes
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public SampleScriptsAttributes(GH_Component component) : base(component)
        {
        }


        /*******************************************/
        /**** Public Fields                     ****/
        /*******************************************/

        public Action<object> MouseDownEvent;
        public string ButtonText = string.Empty;


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void Layout()
        {
            base.Layout();
            Rectangle rec0 = GH_Convert.ToRectangle(Bounds);
            rec0.Height += 22;

            Rectangle rec1 = rec0;
            rec1.Y = rec1.Bottom - 22;
            rec1.Height = 22;
            rec1.Inflate(-2, -2);

            Bounds = rec0;
            m_ButtonBounds = rec1;
        }

        /*******************************************/

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            base.Render(canvas, graphics, channel);

            if (channel == GH_CanvasChannel.Objects)
            {
                GH_Capsule button = GH_Capsule.CreateTextCapsule(
                    m_ButtonBounds, m_ButtonBounds, GH_Palette.Black, ButtonText, 2, 0);
                button.Render(graphics, Selected, false, false);
                button.Dispose();
            }
        }

        /*******************************************/

        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (e.Button == MouseButtons.Left && this.MouseDownEvent != null)
            {
                RectangleF rec = m_ButtonBounds;
                if (rec.Contains(e.CanvasLocation))
                {
                    this.MouseDownEvent(sender);
                    return GH_ObjectResponse.Handled;
                }
            }
            return base.RespondToMouseDown(sender, e);
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        private Rectangle m_ButtonBounds;

        /*******************************************/
    }
}
