using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using BH.UI.Alligator.Templates;
using Grasshopper.Kernel.Special;
using System.Windows.Forms;
using BH.UI.Alligator.Base.NonComponents.Menus;
using BH.oM.DataStructure;
using System.Linq;
using BH.Engine.Reflection.Convert;
using BH.Engine.DataStructure;

namespace BH.UI.Alligator.Base
{
    public class CreateBHoMEnum : GH_ValueList
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Guid ComponentGuid { get; } = new Guid("68B29FAE-057B-417A-96BC-32224974CCBE"); 

        protected override System.Drawing.Bitmap Icon { get; } = Properties.Resources.BHoM_Enum; 

        public override GH_Exposure Exposure { get; } = GH_Exposure.primary; 


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public CreateBHoMEnum()
        {
            Name = "Create BHoM Enum";
            NickName = "BHoMEnum";
            Description = "Creates a specific type of BHoM Enum";
            Category = "Alligator";
            SubCategory = " oM";
            ListItems.Clear();

            if (m_TypeTree == null || m_TypeList == null)
            {
                IEnumerable<Type> types = Engine.Reflection.Query.BHoMEnumList();
                IEnumerable<string> paths = types.Select(x => x.ToText(true));

                List<string> ignore = new List<string> { "BH", "oM", "Engine" };
                m_TypeTree = Create.Tree(types, paths.Select(x => x.Split('.').Where(y => !ignore.Contains(y))), "select a type").ShortenBranches();
                m_TypeList = paths.Zip(types, (k, v) => new Tuple<string, Type>(k, v)).ToList();
            }
        }


        /*******************************************/
        /**** Protected Methods                 ****/
        /*******************************************/

        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            GH_DocumentObject.Menu_AppendSeparator(menu);

            if (m_Type == null)
            {
                SelectorMenu<Type> selector = new SelectorMenu<Type>(menu, Item_Click);
                selector.AppendTree(m_TypeTree);
                selector.AppendSearchBox(m_TypeList);
            } 
        }

        /*******************************************/

        protected void Item_Click(object sender, Type type)
        {
            m_Type = type;
            if (type == null)
                return;

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


        /*******************************************/
        /**** Protected Fields                  ****/
        /*******************************************/

        Type m_Type = null;
        protected static Tree<Type> m_TypeTree = null;
        protected static List<Tuple<string, Type>> m_TypeList = null;

        /*******************************************/
    }
}