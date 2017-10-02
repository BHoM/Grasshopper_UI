using System;
using Grasshopper.Kernel;
using GHE = BH.Engine.Grasshopper;
using BH.Adapter;
using System.Windows.Forms;
using BHI = BH.oM.Structural.Interface;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using BH.Engine.Reflection;

namespace BH.UI.Alligator.Structural.Elements
{
    public class ImportStructuralElements : GH_Component 
    {
        private ComboBox m_Options;
        private ComboBox m_Types;
        protected BHI.ObjectSelection m_Selection;
        protected Type m_Type = null;
        public ImportStructuralElements() : base ("Import BHoM Structural Object", "GetObject", "Get a BHoM Object from an external application", "Structural", "Elements")
        {
            string[] OptionNames = Enum.GetNames(typeof(BHI.ObjectSelection));
            m_Options = new ComboBox();
            m_Options.DropDownStyle = ComboBoxStyle.DropDownList;
            m_Options.SelectedValueChanged += OptionChanged;
            m_Options.Items.AddRange(OptionNames);
            m_Options.SelectedIndex = 0;
            
            List <string> ObjectTypes = new List<string>();
            Dictionary<string, Type> typeDict = BH.Engine.Reflection.Create.CreateTypeDictionary();
            foreach(Type type in typeDict.Values)
            {
                if(type.Namespace == "BH.oM.Structural.Elements")
                {
                    ObjectTypes.Add(type.Name);
                }
            }
                        
            m_Types = new ComboBox();
            m_Types.DropDownStyle = ComboBoxStyle.DropDownList;
            m_Types.SelectedValueChanged += TypeChanged;
            m_Types.Items.AddRange(ObjectTypes.ToArray());
            m_Types.SelectedIndex = 0;
        }

        private void OptionChanged(object sender, EventArgs e)
        {
            m_Selection = (BHI.ObjectSelection)Enum.Parse(typeof(BHI.ObjectSelection), m_Options.SelectedItem.ToString());
            this.Message = m_Options.SelectedItem.ToString();
            this.ExpireSolution(true);
            Params.Input[1].Optional = m_Selection != BHI.ObjectSelection.FromInput;
        }

        private void TypeChanged(object sender, EventArgs e)
        {
            this.m_Type = BH.Engine.Reflection.Create.CreateType(m_Types.SelectedItem.ToString());
            this.Message = m_Types.SelectedItem.ToString();
            this.ExpireSolution(true);
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.tertiary;
            }
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (GHE.DataUtils.Run(DA, 2))
            {
                BHoMAdapter app = GHE.DataUtils.GetGenericData<BHoMAdapter>(DA, 0);
                if (app != null)
                {
                    if (this.m_Type == null)
                        this.m_Type = BH.Engine.Reflection.Create.CreateType("Node");
                    Adapter.Queries.FilterQuery filter = new Adapter.Queries.FilterQuery(this.m_Type);
                    DA.SetDataList(0, app.Pull(filter));
                }
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "Application", "Application to get elements from", GH_ParamAccess.item);
            pManager.AddTextParameter("Selection", "Ids", "Ids of elements to get", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Activate", "Activate", "Activate", GH_ParamAccess.item);

            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Elements", "Elements", "imported BHoM objects", GH_ParamAccess.list);
        }


        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            if (m_Options != null)
            {
                writer.SetString("EnumOption", m_Options.Text);
            }
            if (m_Types != null)
            {
                writer.SetString("TypeOption", m_Types.Text);
            }
            return base.Write(writer);
        }

        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            if (m_Options != null)
            {
                string selection = "";
                reader.TryGetString("EnumOption", ref selection);
                m_Options.SelectedItem = selection;
            }
            if (m_Types != null)
            {
                string type = "";
                reader.TryGetString("TypeOption", ref type);
                m_Types.SelectedItem = type;
            }
            return base.Read(reader);
        }

        public override bool AppendMenuItems(ToolStripDropDown menu)
        {
            if (m_Options != null)
            {
                Menu_AppendObjectName(menu);// (menu, "Section");
                Menu_AppendEnableItem(menu);
                Menu_AppendBakeItem(menu);
                Menu_AppendSeparator(menu);
                Menu_AppendCustomItem(menu, m_Options);
                Menu_AppendSeparator(menu);
                Menu_AppendCustomItem(menu, m_Types);
                Menu_AppendSeparator(menu);
                Menu_AppendObjectHelp(menu);
                return true;
            }
            else
            {
                return base.AppendMenuItems(menu);
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("1320dc1b-87b3-491a-93fa-1495315aa5a2"); }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Structural.Properties.Resources.BHoM_Bar_Import; }
        }
    }

}
