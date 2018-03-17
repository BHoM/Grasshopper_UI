using BH.oM.DataStructure;
using Grasshopper.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BH.UI.Alligator.Base.NonComponents.Menus
{
    public class SelectorMenu<T>
    {
        /*************************************/
        /**** Public Events               ****/
        /*************************************/

        public event EventHandler<T> ItemSelected;


        /*************************************/
        /**** Constructors                ****/
        /*************************************/

        public SelectorMenu(ToolStripDropDown menu, EventHandler<T> callback)
        {
            ItemSelected += callback;
            m_Menu = menu;
        }


        /*************************************/
        /**** Public Methods              ****/
        /*************************************/

        public void AppendTree(Tree<T> tree)
        {
            AppendMenuTree(tree, m_Menu);
        }

        /*************************************/

        public void AppendSearchBox(List<Tuple<string, T>> itemList)
        {
            m_ItemList = itemList;

            AppendMenuSeparator(m_Menu);
            ToolStripMenuItem label = AppendMenuItem(m_Menu, "Search");
            label.Font = new System.Drawing.Font(label.Font, System.Drawing.FontStyle.Bold);
            m_SearchBox = AppendMenuTextItem(m_Menu, "", Search_TextChanged);
        }


        /*************************************/
        /**** Protected Methods           ****/
        /*************************************/

        protected void AppendMenuTree(Tree<T> tree, ToolStripDropDown menu)
        {
            if (tree.Children.Count > 0)
            {
                ToolStripMenuItem treeMenu = AppendMenuItem(menu, tree.Name);
                foreach (Tree<T> childTree in tree.Children.Values.OrderBy(x => x.Name))
                    AppendMenuTree(childTree, treeMenu.DropDown);
            }
            else
            {
                T method = tree.Value;
                ToolStripMenuItem methodItem = AppendMenuItem(menu, tree.Name, Item_Click);
                m_ItemLinks[methodItem] = tree.Value;
            }
        }

        /*************************************/

        protected ToolStripMenuItem AppendMenuItem(ToolStrip menu, string text, EventHandler click = null, bool enabled = true, bool @checked = false)
        {
            ToolStripMenuItem item;
            if (click == null)
                item = new ToolStripMenuItem(text);
            else
                item = new ToolStripMenuItem(text, null, click);

            item.Enabled = enabled;
            item.Checked = @checked;
            menu.Items.Add(item);
            return item;
        }

        /*************************************/

        protected ToolStripTextBox AppendMenuTextItem(ToolStripDropDown menu, string text, GH_MenuTextBox.TextChangedEventHandler textchanged = null, GH_MenuTextBox.KeyDownEventHandler keydown = null, bool enabled = true, int width = -1, bool lockOnFocus = false)
        {
            GH_MenuTextBox item = new GH_MenuTextBox(menu, text, lockOnFocus);
            item.Width = width;
            item.TextBoxItem.Enabled = enabled;
            if (keydown != null)
                item.KeyDown += keydown;
            
            if (textchanged != null)
                item.TextChanged += textchanged;
            
            return item.TextBoxItem;
        }

        /*************************************/

        protected ToolStripSeparator AppendMenuSeparator(ToolStrip menu)
        {
            if (menu.Items.Count == 0)
                return null;
            
            ToolStripItem lastItem = menu.Items[menu.Items.Count - 1];
            if (lastItem is ToolStripSeparator)
                return null;
            
            ToolStripSeparator separator = new ToolStripSeparator();
            menu.Items.Add(separator);
            return separator;
        }

        /*************************************/

        protected void Item_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            if (!m_ItemLinks.ContainsKey(item))
                return;

            if (ItemSelected != null)
                ItemSelected(this, m_ItemLinks[item]);
        }

        /*************************************/

        protected void Search_TextChanged(GH_MenuTextBox sender, string text)
        {
            // Clear the old items
            foreach (ToolStripItem item in m_SearchResultItems)
                item.Dispose();
            m_SearchResultItems.Clear();

            // Add the new ones
            text = text.ToLower();
            string[] parts = text.Split(' ');
            m_SearchResultItems.Add(AppendMenuSeparator(m_Menu));
            foreach (Tuple<string, T> tree in m_ItemList.Where(x => parts.All(y => x.Item1.ToLower().Contains(y))).Take(12).OrderBy(x => x.Item1))
            {
                ToolStripMenuItem methodItem = AppendMenuItem(m_Menu, tree.Item1, Item_Click);
                m_SearchResultItems.Add(methodItem);
                m_ItemLinks[methodItem] = tree.Item2;
            }
        }


        /*************************************/
        /**** Protected Fields            ****/
        /*************************************/

        protected ToolStripDropDown m_Menu;
        protected ToolStripTextBox m_SearchBox;
        protected List<Tuple<string, T>> m_ItemList = new List<Tuple<string, T>>();
        protected Dictionary<ToolStripMenuItem, T> m_ItemLinks = new Dictionary<ToolStripMenuItem, T>();
        protected List<ToolStripItem> m_SearchResultItems = new List<ToolStripItem>();


        /*************************************/
    }
}
