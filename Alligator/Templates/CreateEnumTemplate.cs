using System;
using Grasshopper.Kernel;
using BH.UI.Alligator.Base;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel.Parameters;
using System.Collections;
using System.Windows.Forms;
using BH.oM.Base;
using System.Reflection;
using BH.oM.Geometry;
using BH.oM.DataStructure;
using Grasshopper.Kernel.Special;
using Grasshopper.GUI;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

// Instructions to implement this template
// ***************************************
//
//  1. Override the GetRelevantEnums() method that provide the list of enum types this component can create
//  2. Change the value of m_MenuMaxDepth to modify the maximum depth of the menu (do that in the constructor of your new class)


namespace BH.UI.Alligator.Templates
{
    public abstract class CreateEnumTemplate : GH_ValueList
    {
        /*************************************/
        /**** 1 . Parts to Implement      ****/
        /*************************************/

        protected abstract IEnumerable<Type> GetRelevantEnums();    // 1. Define the list of relevant enum types that can be created


        /*************************************/

        protected int m_MenuMaxDepth = 10;  // 2. Change this value to modify the maximum depth of the menu


        /*************************************/
        /**** Parameters Handling         ****/
        /*************************************/

        protected CreateEnumTemplate(string name, string nickname, string description, string category, string subCategory) 
        {
            Name = name;
            NickName = nickname;
            Description = description;
            Category = category;
            SubCategory = subCategory;
            ListItems.Clear();
        }

        protected override System.Drawing.Bitmap Icon { get { return null; } }


        /*************************************/
        /**** Solve Instance              ****/
        /*************************************/

        protected override void CollectVolatileData_Custom()
        {
            this.m_data.Clear();
            try
            {
                List<GH_ValueListItem>.Enumerator enumerator = this.SelectedItems.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    GH_ValueListItem item = enumerator.Current;
                    int index = 0;
                    item.Value.CastTo<int>(out index);
                    this.m_data.Append(new GH_Enum(Enum.GetValues(m_Type).GetValue(index) as Enum), new GH_Path(0));
                }
            }
            finally
            {
                //((System.IDisposable)enumerator).Dispose();
            }
        }


        /*************************************/
        /**** Saving Component            ****/
        /*************************************/

        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            if (m_Type != null)
            {
                writer.SetString("TypeName", m_Type.AssemblyQualifiedName);
            }
            return base.Write(writer);
        }

        /*************************************/

        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            string typeString = ""; reader.TryGetString("TypeName", ref typeString);
            m_Type = Type.GetType(typeString);
            return base.Read(reader);
        }


        /*************************************/
        /**** Creating Menu               ****/
        /*************************************/

        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            GH_DocumentObject.Menu_AppendSeparator(menu);

            if (m_Type == null)
            {
                Tree<Type> types = GetEnumTree();
                AppendMethodTreeToMenu(types, menu);
                AppendSearchMenu(types, menu);
            }
        }

        /*************************************/

        protected void AppendSearchMenu(Tree<Type> types, ToolStripDropDown menu)
        {
            m_Menu = menu;
            m_TypeList = GetTypeList(types);

            Menu_AppendSeparator(menu);
            ToolStripMenuItem label = Menu_AppendItem(menu, "Search");
            label.Font = new System.Drawing.Font(label.Font, System.Drawing.FontStyle.Bold);
            m_SearchBox = Menu_AppendTextItem(menu, "", null, Search_TextChanged, false);
        }

        /*************************************/

        private void Search_TextChanged(GH_MenuTextBox sender, string text)
        {
            // Clear the old items
            foreach (ToolStripItem item in m_SearchResultItems)
                item.Dispose();
            m_SearchResultItems.Clear();

            // Add the new ones
            text = text.ToLower();
            m_SearchResultItems.Add(Menu_AppendSeparator(m_Menu));
            foreach (Tree<Type> tree in m_TypeList.Where(x => x.Name.ToLower().Contains(text)).Take(12).OrderBy(x => x.Name))
            {
                ToolStripMenuItem methodItem = Menu_AppendItem(m_Menu, tree.Name, Item_Click);
                m_SearchResultItems.Add(methodItem);
                m_TypeLinks[methodItem] = tree.Value;
            }
        }

        /*************************************/

        protected void AppendMethodTreeToMenu(Tree<Type> tree, ToolStripDropDown menu)
        {
            if (tree.Children.Count > 0)
            {
                ToolStripMenuItem treeMenu = Menu_AppendItem(menu, tree.Name);
                foreach (Tree<Type> childTree in tree.Children.Values.OrderBy(x => x.Name))
                    AppendMethodTreeToMenu(childTree, treeMenu.DropDown);
            }
            else
            {
                ToolStripMenuItem methodItem = Menu_AppendItem(menu, tree.Name, Item_Click);
                m_TypeLinks[methodItem] = tree.Value;
            }
        }

        /*************************************/

        protected void Item_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            if (!m_TypeLinks.ContainsKey(item))
                return;

            m_Type = m_TypeLinks[item];
            this.NickName = m_Type.Name;
            this.Name = m_Type.Name;

            ListItems.Clear();
            string[] names = Enum.GetNames(m_Type);
            Array values = Enum.GetValues(m_Type);
            for (int i = 0; i < names.Length; i++)
            {
                ListItems.Add(new GH_ValueListItem(names[i], i.ToString()));
            }
            this.ExpireSolution(true);
        }


        /*************************************/
        /**** Protected Methods           ****/
        /*************************************/

        protected virtual Tree<Type> GetEnumTree()
        {
            Tree<Type> root = new Tree<Type> { Name = "Select enum" };
            List<Type> createObjectMethods = BH.Engine.Reflection.Query.GetBHoMEnumList();

            foreach (Type type in GetRelevantEnums().OrderBy(x => x.Name))
            {
                // Make sure the part of the tree corresponding to the namespace exists
                Tree<Type> tree = root;
                foreach (string part in type.Namespace.Split('.').Skip(2).Take(m_MenuMaxDepth))
                {
                    if (!tree.Children.ContainsKey(part))
                        tree.Children.Add(part, new Tree<Type> { Name = part });
                    tree = tree.Children[part];
                }

                tree.Children.Add(type.Name, new Tree<Type>(type, type.Name));
            }

            return root;
        }

        /*************************************/

        protected IEnumerable<Tree<Type>> GetTypeList(Tree<Type> tree)
        {
            return tree.Children.Values.SelectMany(x => GetTypeList(x, ""));
        }

        /*************************************/

        protected IEnumerable<Tree<Type>> GetTypeList(Tree<Type> tree, string path)
        {
            if (path.Length > 0)
                path = path + '.';

            if (tree.Children.Count == 0)
                return new Tree<Type>[] { new Tree<Type>(tree.Value, path + tree.Name) };
            else
                return tree.Children.Values.SelectMany(x => GetTypeList(x, path + tree.Name));
        }


        /*************************************/
        /**** Protected Fields            ****/
        /*************************************/

        Type m_Type = null;

        protected Dictionary<ToolStripMenuItem, Type> m_TypeLinks = new Dictionary<ToolStripMenuItem, Type>();
        protected List<ToolStripItem> m_SearchResultItems = new List<ToolStripItem>();
        ToolStripTextBox m_SearchBox;
        ToolStripDropDown m_Menu;
        IEnumerable<Tree<Type>> m_TypeList = new List<Tree<Type>>();

    }
}