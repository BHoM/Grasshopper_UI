using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using BH.UI.Alligator.Templates;
using BH.oM.DataStructure;
using System.Linq;
using System.Windows.Forms;
using Grasshopper.GUI;
using BH.Engine.DataStructure;
using BH.Engine.Reflection.Convert;

namespace BH.UI.Alligator.Base
{
    public class CreateBHoMType : GH_Component
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Guid ComponentGuid { get; } = new Guid("FC00CD7C-AAC6-43FC-A6B7-BBE35BF0E4FD");

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Properties.Resources.Type;

        public override GH_Exposure Exposure { get; } = GH_Exposure.primary; 


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public CreateBHoMType() : base("Create BHoM Type", "BHoMType", "Creates a specific type definition", "Alligator", " oM")
        {
            if (m_TypeTree == null)
            {
                List<string> ignore = new List<string> { "BH", "oM", "Engine" };
                m_TypeTree = new Tree<Type> { Name = "select a type" };
                foreach (Type type in BH.Engine.Reflection.Query.BHoMTypeList())
                    AddTypeToTree(m_TypeTree, type.ToText(true).Split('.').Where(y => !ignore.Contains(y)), type);
                m_TypeTree.ShortenBranches();
            }
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
            pManager.AddGenericParameter("Type", "Type", "Type definition", GH_ParamAccess.item);
        }

        /*******************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DA.SetData(0, m_Type);
        }

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

            if (typeString.Length > 0)
                m_Type = Type.GetType(typeString);

            if (m_Type != null)
                Message = m_Type.ToText();

            return base.Read(reader);
        }


        /*******************************************/
        /**** Protected Methods                 ****/
        /*******************************************/

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);

            AppendTreeToMenu(m_TypeTree, menu);
            AppendSearchMenu(m_TypeTree, menu);
        }

        /*******************************************/

        protected virtual void AddTypeToTree(Tree<Type> tree, IEnumerable<string> path, Type type)
        {
            Dictionary<string, Tree<Type>> children = tree.Children;

            if (path.Count() == 0)
            {
                tree.Value = type;
            }
            else
            {
                string name = path.First();
                if (!children.ContainsKey(name))
                    children.Add(name, new Tree<Type> { Name = name });
                AddTypeToTree(children[name], path.Skip(1), type);
            }
        }

        /*************************************/

        protected void AppendTreeToMenu(Tree<Type> tree, ToolStripDropDown menu)
        {
            if (tree.Children.Count > 0)
            {
                ToolStripMenuItem treeMenu = Menu_AppendItem(menu, tree.Name);
                foreach (Tree<Type> childTree in tree.Children.Values.OrderBy(x => x.Name))
                    AppendTreeToMenu(childTree, treeMenu.DropDown);
            }
            else
            {
                ToolStripMenuItem typeItem = Menu_AppendItem(menu, tree.Name, Item_Click);
                m_TypeLinks[typeItem] = tree.Value;
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
            string[] parts = text.Split(' ');
            m_SearchResultItems.Add(Menu_AppendSeparator(m_Menu));
            foreach (Tree<Type> tree in m_TypeList.Where(x => parts.All(y => x.Name.ToLower().Contains(y))).Take(12).OrderBy(x => x.Name))
            {
                ToolStripMenuItem typeItem = Menu_AppendItem(m_Menu, tree.Name, Item_Click);
                m_SearchResultItems.Add(typeItem);
                m_TypeLinks[typeItem] = tree.Value;
            }
        }

        /*************************************/

        protected IEnumerable<Tree<Type>> GetTypeList(Tree<Type> tree)
        {
            return tree.Children.Values.SelectMany(x => GetTypeList(x, ""));
        }

        /*************************************/

        protected IEnumerable<Tree<Type>> GetTypeList(Tree<Type> tree, string path)
        {
            if (path.Length > 0 && !tree.Name.StartsWith("("))
                path = path + '.';

            if (tree.Children.Count == 0)
                return new Tree<Type>[] { new Tree<Type> { Value = tree.Value, Name = path + tree.Name } };
            else
                return tree.Children.Values.SelectMany(x => GetTypeList(x, path + tree.Name));
        }

        /*************************************/

        protected void Item_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            if (!m_TypeLinks.ContainsKey(item))
                return;

            m_Type = m_TypeLinks[item];
            if (m_Type == null)
                return;

            Message = m_Type.ToText();
            ExpireSolution(true);
        }


        /*******************************************/
        /**** Protected Fields                  ****/
        /*******************************************/

        Type m_Type = null;
        protected Dictionary<ToolStripMenuItem, Type> m_TypeLinks = new Dictionary<ToolStripMenuItem, Type>();
        protected List<ToolStripItem> m_SearchResultItems = new List<ToolStripItem>();
        ToolStripTextBox m_SearchBox;
        ToolStripDropDown m_Menu;
        IEnumerable<Tree<Type>> m_TypeList = new List<Tree<Type>>();

        static Tree<Type> m_TypeTree = null;

        /*******************************************/
    }
}