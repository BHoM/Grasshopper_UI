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

using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GH = Grasshopper;
using BH.UI.Grasshopper.CustomAttributes;

namespace BH.UI.Grasshopper.Components.UI
{
    public class SampleScripts : GH_Component
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Guid ComponentGuid { get { return new Guid("3F2A8B1C-D4E5-4F67-8901-2A3B4C5D6E7F"); } }

        public override GH_Exposure Exposure { get { return GH_Exposure.primary; } }

        protected override Bitmap Internal_Icon_24x24 { get { return Properties.Resources.SampleScripts; } }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public SampleScripts()
            : base("BHoM Sample Scripts", "Samples",
                "Load sample Grasshopper scripts from shared BHoM folders.",
                "BHoM", "UI")
        {
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }

        /*******************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
        }

        /*******************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BH.Engine.Base.Compute.ClearCurrentEvents();

            if (!Directory.Exists(m_SampleFolder))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning,
                    $"Sample folder not found: {m_SampleFolder}\n" +
                    "Place .gh or .ghx files there and recompute.");
                return;
            }

            m_FolderList = new List<string>();
            m_FilesList = new List<List<string>>();

            var fs = Directory.GetFiles(m_SampleFolder, "*.gh*", SearchOption.AllDirectories).ToList();
            if (fs.Any())
            {
                m_FolderList.Add(m_SampleFolder);
                m_FilesList.Add(fs);
            }

            if (!m_FilesList.Any() || !m_FilesList.SelectMany(f => f).Any())
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Remark,
                    $"No .gh or .ghx files found in: {m_SampleFolder}");
                return;
            }

            m_TemplateMenu = BuildMenu();
        }

        /*******************************************/

        public override void CreateAttributes()
        {
            var att = new SampleScriptsAttributes(this);
            att.ButtonText = "Load a sample";
            att.MouseDownEvent += (loc) =>
                m_TemplateMenu.Show(
                    (GH.GUI.Canvas.GH_Canvas)loc,
                    ((GH.GUI.Canvas.GH_Canvas)loc).CursorControlPosition);
            this.Attributes = att;
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        private Size GetMoveVector(PointF fromLocation)
        {
            var moveX = this.Attributes.Bounds.Left - 80 - fromLocation.X;
            var moveY = this.Attributes.Bounds.Y + 180 - fromLocation.Y;
            var loc = new Point(Convert.ToInt32(moveX), Convert.ToInt32(moveY));
            return new Size(loc);
        }

        /*******************************************/

        private void LoadScriptOntoCanvas(string filePath, ref bool run)
        {
            var canvas = GH.Instances.ActiveCanvas;
            if (!run || !canvas.Focused || !File.Exists(filePath))
                return;

            var io = new GH_DocumentIO();
            var success = io.Open(filePath);

            if (!success)
            {
                MessageBox.Show($"Failed to open sample file:\n{filePath}");
                return;
            }

            var docTemp = io.Document;
            docTemp.SelectAll();
            docTemp.MutateAllIds();

            // Move loaded objects next to this component
            var box = docTemp.BoundingBox(false);
            var vec = GetMoveVector(box.Location);
            docTemp.TranslateObjects(vec, true);

            docTemp.ExpireSolution();
            var docCurrent = canvas.Document;
            docCurrent.DeselectAll();
            docCurrent.MergeDocument(docTemp);
        }

        /*******************************************/

        private ToolStripDropDownMenu BuildMenu()
        {
            var menu = new ToolStripDropDownMenu();

            foreach (var folder in m_FolderList)
            {
                var menuItem = BuildMenuFromFolder(folder);
                if (menuItem != null)
                {
                    var items = menuItem.DropDown.Items.OfType<ToolStripItem>().ToArray();
                    menu.Items.AddRange(items);
                }
            }

            return menu;
        }

        /*******************************************/

        private ToolStripMenuItem BuildMenuFromFolder(string rootFolder)
        {
            var allFiles = Directory.GetFiles(rootFolder, "*.gh*", SearchOption.AllDirectories);
            if (!allFiles.Any()) return null;

            var topDirs = Directory.GetDirectories(rootFolder);
            var topFiles = Directory.GetFiles(rootFolder, "*.gh*", SearchOption.TopDirectoryOnly);

            var folderName = new DirectoryInfo(rootFolder).Name;
            var menuItem = new ToolStripMenuItem(folderName);

            // Add subfolders recursively
            foreach (var subDir in topDirs)
            {
                var subMenuItem = BuildMenuFromFolder(subDir);
                if (subMenuItem != null)
                    menuItem.DropDownItems.Add(subMenuItem);
            }

            // Add files in this folder
            foreach (var file in topFiles)
            {
                var name = Path.GetFileNameWithoutExtension(file);
                EventHandler onClick = (sender, e) =>
                {
                    var item = sender as ToolStripDropDownItem;
                    var run = true;
                    LoadScriptOntoCanvas(item.Tag.ToString(), ref run);
                    this.ExpireSolution(true);
                };

                Menu_AppendItem(menuItem.DropDown, name, onClick, null, file);
            }

            return menuItem;
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        private List<string> m_FolderList = new List<string>();
        private List<List<string>> m_FilesList = new List<List<string>>();
        private ToolStripDropDownMenu m_TemplateMenu = new ToolStripDropDownMenu();

        private static readonly string m_SampleFolder =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "BHoM", "Resources", "GrasshopperSamples");

        /*******************************************/
    }
}
