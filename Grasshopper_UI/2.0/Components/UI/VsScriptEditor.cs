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

//using System.Drawing;
//using System.IO;
//using System.Collections.Generic;
//using System.Windows.Forms;
//using Grasshopper.Kernel;
//using System;
//using ScriptComponents;
//using Grasshopper.GUI.Canvas;

//namespace BH.UI.Grasshopper.Base
//{
//    public class VsScriptEditor : GH_Component
//    {
//        /*******************************************/
//        /**** Properties                        ****/
//        /*******************************************/

//        public override GH_Exposure Exposure { get; } = GH_Exposure.primary; 

//        public override Guid ComponentGuid { get; } = new Guid("{5aa64ae8-f5f0-462f-bdea-073bcad31964}"); 

//        protected override Bitmap Internal_Icon_24x24 { get; } = Properties.Resources.VS_Script; 


//        /*******************************************/
//        /**** Constructors                      ****/
//        /*******************************************/

//        public VsScriptEditor() : base("VS Editor", "VsEditor", "Uses a text file as the code of a scripting component.", "Grasshopper", "Scripting") { }


//        /*******************************************/
//        /**** Override Methods                  ****/
//        /*******************************************/

//        public override void CreateAttributes()
//        {
//            base.CreateAttributes();
//            base.m_attributes = new Attributes_Custom(this);
//        }

//        /*******************************************/

//        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
//        {
//            pManager.AddTextParameter("File", "F", "Path to the code file. Use the File Path parameter with the Syncronize option enabled.", GH_ParamAccess.item);
//        }

//        /*******************************************/

//        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager) { }


//        /*******************************************/

//        protected override void SolveInstance(IGH_DataAccess DA)
//        {
//            string file = string.Empty;
//            DA.GetData(0, ref file);

//            Rectangle bounds = Rectangle.Ceiling(this.Attributes.Bounds);
//            System.Drawing.Point point = new System.Drawing.Point(bounds.X + bounds.Width / 2, bounds.Y - 40);

//            Component_AbstractScript scriptComponent = Grasshopper.Instances.ActiveCanvas.Document.FindComponent(point) as Component_AbstractScript;
//            Type scriptLanguage = Grasshopper.Instances.ActiveCanvas.Document.FindComponent(point).GetType();
//            List<string> splitLines = new List<string>();
//            if (typeof(Component_CSNET_Script).IsAssignableFrom(scriptLanguage))
//            {
//                scriptComponent = (Component_CSNET_Script)Grasshopper.Instances.ActiveCanvas.Document.FindComponent(point);
//                scriptComponent.SourceCodeChanged(new Grasshopper.GUI.Script.GH_ScriptEditor(Grasshopper.GUI.Script.GH_ScriptLanguage.CS));
//                ToolStripDropDown menu = new ToolStripDropDown();
//                EventHandler doClick = null;
//                menu.Items.Add("Give it to me", null, doClick);
//                foreach (var param in scriptComponent.Params)
//                {

//                    param.AppendMenuItems(menu);
//                }
//                splitLines.Add("// <Custom code>");
//                splitLines.Add("// </Custom code>");
//                splitLines.Add("// <Custom additional code>");
//                splitLines.Add("// </Custom additional code>");
//            }
//            else
//            {
//                scriptComponent = (Component_VBNET_Script)Grasshopper.Instances.ActiveCanvas.Document.FindComponent(point);
//                scriptComponent.SourceCodeChanged(new Grasshopper.GUI.Script.GH_ScriptEditor(Grasshopper.GUI.Script.GH_ScriptLanguage.CS));

//                splitLines.Add("' <Custom code>");
//                splitLines.Add("' </Custom code>");
//                splitLines.Add("' <Custom additional code>");
//                splitLines.Add("' </Custom additional code>");
//            }

//            if (scriptComponent == null)
//            {
//                this.Message = string.Empty;
//                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, string.Format("No scripting component attached."));
//            }
//            else
//                this.Message = scriptComponent.Name;
//            try
//            {
//                string code;
//                using (StreamReader sr = new StreamReader(file))
//                    code = sr.ReadToEnd();

//                if (scriptComponent != null)
//                {
//                    string[] codes = code.Split(splitLines.ToArray(), StringSplitOptions.None);
//                    scriptComponent.ScriptSource.ScriptCode = codes[1];
//                    scriptComponent.ScriptSource.AdditionalCode = codes[3];
//                    scriptComponent.ExpireSolution(true);
//                }
//            }
//            catch (Exception e)
//            {
//                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, e.Message);
//            }
//        }

//        /*******************************************/
//    }


//    //////////////////////////////////////////////////////////////////////////////////////////////////////////

//    public class Attributes_Custom : Grasshopper.Kernel.Attributes.GH_ComponentAttributes
//    {
//        /*******************************************/
//        /**** Constructors                      ****/
//        /*******************************************/

//        public Attributes_Custom(GH_Component owner) : base(owner) { }


//        /*******************************************/
//        /**** Override Methods                  ****/
//        /*******************************************/

//        protected override void Layout()
//        {
//            base.Layout();
//        }

//        /*******************************************/

//        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
//        {
//            base.Render(canvas, graphics, channel);

//            if (channel == GH_CanvasChannel.Objects)
//            {
//                Rectangle rectangle = new Rectangle();
//                int diameter = 30;

//                rectangle.Y = (int)Bounds.Y - 40 - diameter / 2;
//                rectangle.X = (int)Bounds.X + (int)Bounds.Width / 2 - diameter / 2;
//                rectangle.Width = diameter;
//                rectangle.Height = diameter;

//                Pen pen = new Pen(Color.Black, 1);
//                pen.DashPattern = new float[] { 1.0f, 3.0f };
//                graphics.DrawArc(pen, rectangle, 0, 360);

//                var font = new Font(FontFamily.GenericSansSerif, 4.0f, FontStyle.Regular);
//                StringFormat format = new StringFormat();
//                format.LineAlignment = StringAlignment.Center;
//                format.Alignment = StringAlignment.Center;
//                graphics.DrawString("place\r\ncomponent\r\nhere", font, Brushes.Black, rectangle, format);
//            }
//        }

//        /*******************************************/
//    }
//}
