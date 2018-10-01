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
using Grasshopper.Kernel.Data;

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

        public override bool Obsolete { get { return m_IsDeprecated; } }


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
                IEnumerable<Type> types = BH.Engine.Reflection.Query.BHoMEnumList();
                IEnumerable<string> paths = types.Select(x => x.ToText(true));

                List<string> ignore = new List<string> { "BH", "oM", "Engine" };
                m_TypeTree = Engine.DataStructure.Create.Tree(types, paths.Select(x => x.Split('.').Where(y => !ignore.Contains(y))), "select a type").ShortenBranches();
                m_TypeList = paths.Zip(types, (k, v) => new Tuple<string, Type>(k, v)).ToList();
            }
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void CollectVolatileData_Custom()
        {
            this.m_data.Clear();
            List<GH_ValueListItem>.Enumerator enumerator = this.SelectedItems.GetEnumerator();
            while (enumerator.MoveNext())
            {
                GH_ValueListItem item = enumerator.Current;
                int index = 0;
                item.Value.CastTo<int>(out index);
                this.m_data.Append(new GH_Enum(Enum.GetValues(m_Type).GetValue(index) as Enum), new GH_Path(0));
            }
        }

        /*******************************************/

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

            //Fix for namespace change in structure
            if (typeString.Contains("oM.Structural"))
            {
                typeString = typeString.Replace("oM.Structural", "oM.Structure");
                m_IsDeprecated = true;
            }

            if (typeString.Length > 0)
                m_Type = Type.GetType(typeString);

            return base.Read(reader);
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
        private bool m_IsDeprecated = false;
        protected static Tree<Type> m_TypeTree = null;
        protected static List<Tuple<string, Type>> m_TypeList = null;

        /*******************************************/
    }
}