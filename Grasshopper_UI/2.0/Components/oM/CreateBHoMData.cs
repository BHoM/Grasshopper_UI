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

using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using Grasshopper.Kernel.Special;
using BH.oM.DataStructure;
using BH.UI.Grasshopper.Base.NonComponents.Menus;
using BH.Engine.DataStructure;
using System.Linq;
using System.Windows.Forms;
using BH.Engine.Reflection;
using BH.oM.Base;

namespace BH.UI.Grasshopper.Base
{
    public class CreateBHoMData : GH_ValueList
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Guid ComponentGuid { get; } = new Guid("947798C3-BC2B-466B-B450-571DF8EEA66C");

        protected override System.Drawing.Bitmap Icon { get; } = Properties.Resources.BHoM_Data;

        public override GH_Exposure Exposure { get; } = GH_Exposure.hidden;

        public override bool Obsolete { get; } = true;

        public object SelectedCategory { get { return m_FileName; } }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public CreateBHoMData()
        {
            Name = "Create BHoM Reference Data";
            NickName = "BHoMData";
            Description = "Creates a BhoM object from the reference datasets";
            Category = "Grasshopper";
            SubCategory = " oM";
            ListItems.Clear();

            if (m_FileTree == null || m_FileList == null)
            {
                List<string> names = BH.Engine.Library.Query.LibraryNames();

                m_FileTree = Engine.DataStructure.Create.Tree(names, names.Select(x => x.Split('\\').ToList()).ToList(), "select a dataset").ShortenBranches();
                m_FileList = names.Select(x => new Tuple<string, string>(x, x)).ToList();
            }
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            if (m_FileName != null)
            {
                writer.SetString("FileName", m_FileName);
            }
            return base.Write(writer);
        }

        /*************************************/

        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            bool ok = base.Read(reader);
            int index = ListItems.IndexOf(SelectedItems[0]);

            string fileName = "";  reader.TryGetString("FileName", ref fileName);
            if (fileName.Length > 0)
                File_Selected(null, fileName);

            if (index < ListItems.Count)
                SelectItem(index);

            return ok;
        }


        /*******************************************/
        /**** Protected Methods                 ****/
        /*******************************************/

        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            GH_DocumentObject.Menu_AppendSeparator(menu);

            if (m_FileName == null)
            {
                SelectorMenu<string> selector = new SelectorMenu<string>(menu, File_Selected);
                selector.AppendTree(m_FileTree);
                selector.AppendSearchBox(m_FileList);
            }
            else
            {
                SelectorMenu<int> selector = new SelectorMenu<int>(menu, Item_Selected);
                selector.AppendSearchBox(m_ItemList);
            }
        }

        /*******************************************/

        protected void File_Selected(object sender, string fileName)
        {
            m_FileName = fileName;
            if (fileName == null)
                return;

            this.NickName = fileName.Split('\\').Last();
            this.Name = this.NickName;

            List<IBHoMObject> objects = BH.Engine.Library.Query.Library(fileName);
            m_ItemList = objects.Select((x, i) => new Tuple<string, int>(x.Name, i)).ToList();

            ListItems.Clear();
            var prop = typeof(GH_ValueListItem).GetField("m_value", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            foreach (IBHoMObject obj in objects)
            {
                GH_ValueListItem item = new GH_ValueListItem(obj.Name, "JustDoIt");
                prop.SetValue(item, new GH_BHoMObject(obj));

                ListItems.Add(item);
            }
            this.ExpireSolution(true);
        }

        /*******************************************/

        protected void Item_Selected(object sender, int index)
        {
            this.SelectItem(index);
            this.ExpireSolution(true);
        }


        /*******************************************/
        /**** Protected Fields                  ****/
        /*******************************************/

        string m_FileName = null;
        protected static Tree<string> m_FileTree = null;
        protected static List<Tuple<string, string>> m_FileList = null;
        protected static List<Tuple<string, int>> m_ItemList = null;

        /*******************************************/
    }
}