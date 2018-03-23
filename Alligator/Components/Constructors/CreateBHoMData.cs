using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using Grasshopper.Kernel.Special;
using BH.oM.DataStructure;
using BH.UI.Alligator.Base.NonComponents.Menus;
using BH.Engine.DataStructure;
using System.Linq;
using System.Windows.Forms;
using BH.Engine.Reflection.Convert;
using BH.oM.Base;

namespace BH.UI.Alligator.Base
{
    public class CreateBHoMData : GH_ValueList
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Guid ComponentGuid { get; } = new Guid("947798C3-BC2B-466B-B450-571DF8EEA66C");

        protected override System.Drawing.Bitmap Icon { get; } = Properties.Resources.BHoM_Data;

        public override GH_Exposure Exposure { get; } = GH_Exposure.primary; 


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public CreateBHoMData()
        {
            Name = "Create BHoM Reference Data";
            NickName = "BHoMData";
            Description = "Creates a BhoM object from the reference datasets";
            Category = "Alligator";
            SubCategory = " oM";
            ListItems.Clear();

            if (m_FileTree == null || m_FileList == null)
            {
                IEnumerable<string> names = Engine.Library.Query.LibraryNames();

                m_FileTree = Create.Tree(names, names.Select(x => x.Split('\\')), "select a dataset").ShortenBranches();
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

            string fileName = "";  reader.TryGetString("FileName", ref fileName);
            if (fileName.Length > 0)
                Item_Click(null, fileName);

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
                SelectorMenu<string> selector = new SelectorMenu<string>(menu, Item_Click);
                selector.AppendTree(m_FileTree);
                selector.AppendSearchBox(m_FileList);
            }
        }

        /*******************************************/

        protected void Item_Click(object sender, string fileName)
        {
            m_FileName = fileName;
            if (fileName == null)
                return;

            this.NickName = fileName.Split('\\').Last();
            this.Name = this.NickName;

            List<IBHoMObject> objects = Engine.Library.Query.Library(fileName);

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
        /**** Protected Fields                  ****/
        /*******************************************/

        string m_FileName = null;
        protected static Tree<string> m_FileTree = null;
        protected static List<Tuple<string, string>> m_FileList = null;

        /*******************************************/
    }
}