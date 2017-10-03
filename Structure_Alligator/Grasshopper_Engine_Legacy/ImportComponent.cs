//using Grasshopper.Kernel;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using GHE = BH.Engine.Grasshopper;
//using BHE = BH.oM.Structural.Elements;
//using BHI = BH.oM.Structural.Interface;
//using Grasshopper.Kernel.Data;
//using BH.Engine.Reflection;


//namespace BH.Engine.Grasshopper.Components
//{
//    public abstract class ImportComponent<T> : GH_Component where T : BH.oM.Base.BHoMObject
//    {
//        private ComboBox m_Options;
//        private ComboBox m_Types;
//        protected BHI.ObjectSelection m_Selection;
//        protected Type m_Type;
//        public ImportComponent() { }

//        public ImportComponent("Import BHoM Object", "GetObject", "Get a BHoM Object from an external application", "Alligator", "Elements") 
//        {
//            string[] OptionNames = Enum.GetNames(typeof(BHI.ObjectSelection));
//            m_Options = new ComboBox();
//            m_Options.DropDownStyle = ComboBoxStyle.DropDownList;
//            m_Options.SelectedValueChanged += OptionChanged;
//            m_Options.Items.AddRange(OptionNames);
//            m_Options.SelectedIndex = 0;

//            List<string> ObjectTypes = BH.Engine.Reflection.Create.CreateTypeDictionary().Keys.ToList();
//            //List<string> ObjectTypes = new List<string> { "Bar", "Node", "Panel" };
//            m_Types = new ComboBox();
//            m_Types.DropDownStyle = ComboBoxStyle.DropDownList;
//            m_Types.SelectedValueChanged += TypeChanged;
//            m_Types.Items.AddRange(ObjectTypes.ToArray());
//            m_Types.SelectedIndex = 0;
//        }

//        private void OptionChanged(object sender, EventArgs e)
//        {
//            m_Selection = (BHI.ObjectSelection)Enum.Parse(typeof(BHI.ObjectSelection), m_Options.SelectedItem.ToString());
//            this.Message = m_Options.SelectedItem.ToString();
//            this.ExpireSolution(true);
//            Params.Input[1].Optional = m_Selection != BHI.ObjectSelection.FromInput;
//        }

//        private void TypeChanged(object sender, EventArgs e)
//        {
//            m_Type = BH.Engine.Reflection.Create.CreateType(m_Types.SelectedItem.ToString());
//            this.Message = m_Types.SelectedItem.ToString();
//            this.ExpireSolution(true);
//        }

//        public override Guid ComponentGuid
//        {
//            get
//            {
//                return new Guid("{3950F5C9-F1F4-4D0A-AD93-340D8FCAE9B9}");
//            }
//        }

//        protected override void RegisterInputParams(GH_InputParamManager pManager)
//        {
//            pManager.AddGenericParameter("Application", "Application", "Application to get elements from", GH_ParamAccess.item);
//            pManager.AddTextParameter("Selection", "Ids", "Ids of elements to get", GH_ParamAccess.list);
//            pManager.AddBooleanParameter("Activate", "Activate", "Activate", GH_ParamAccess.item);

//            pManager[1].Optional = true;
//        }

//        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
//        {
//            pManager.AddGenericParameter("Elements", "Elements", "imported BHoM objects", GH_ParamAccess.list);
//        }


//        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
//        {
//            if (m_Options != null)
//            {
//                writer.SetString("EnumOption", m_Options.Text);
//            }
//            if (m_Types != null)
//            {
//                writer.SetString("TypeOption", m_Types.Text);
//            }
//            return base.Write(writer);
//        }

//        public override bool Read(GH_IO.Serialization.GH_IReader reader)
//        {
//            if (m_Options != null)
//            {
//                string selection = "";
//                reader.TryGetString("EnumOption", ref selection);
//                m_Options.SelectedItem = selection;
//            }
//            if (m_Types != null)
//            {
//                string type = "";
//                reader.TryGetString("Typeption", ref type);
//                m_Types.SelectedItem = type;
//            }
//            return base.Read(reader);
//        }

//        public override bool AppendMenuItems(ToolStripDropDown menu)
//        {
//            if (m_Options != null)
//            {
//                Menu_AppendObjectName(menu);// (menu, "Section");
//                Menu_AppendEnableItem(menu);
//                Menu_AppendBakeItem(menu);
//                Menu_AppendSeparator(menu);
//                Menu_AppendCustomItem(menu, m_Options);
//                Menu_AppendSeparator(menu);
//                Menu_AppendCustomItem(menu, m_Types);
//                Menu_AppendSeparator(menu);
//                Menu_AppendObjectHelp(menu);
//                return true;
//            }
//            else
//            {
//                return base.AppendMenuItems(menu);
//            }
//        }


//    }
//}
