using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GHE = Grasshopper_Engine;
using BHE = BHoM.Structural.Elements;
using BHI = BHoM.Structural.Interface;
using Grasshopper.Kernel.Data;

namespace Grasshopper_Engine.Components
{   
    public abstract class ImportComponent<T> : GH_Component where T : BHoM.Base.IBase
    {
        public ComboBox m_Options;
        protected BHI.ObjectSelection m_Selection;
        public ImportComponent() { }

        public ImportComponent(string name, string nickname, string description, string category, string subcategory) : base(name, nickname, description, category, subcategory)
        {
            string[] OptionNames = Enum.GetNames(typeof(BHI.ObjectSelection));
            m_Options = new ComboBox();
            m_Options.DropDownStyle = ComboBoxStyle.DropDownList;
            m_Options.SelectedValueChanged += OptionChanged;
            m_Options.Items.AddRange(OptionNames);
            m_Options.SelectedIndex = 0;
        }

        private void OptionChanged(object sender, EventArgs e)
        {
            m_Selection = (BHI.ObjectSelection)Enum.Parse(typeof(BHI.ObjectSelection), m_Options.SelectedItem.ToString());
            this.Message = m_Options.SelectedItem.ToString();
            this.ExpireSolution(true);
            Params.Input[1].Optional = m_Selection != BHI.ObjectSelection.FromInput;
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("{3950F5C9-F1F4-4D0A-AD93-340D8FCAE9B9}");
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
            pManager.AddTextParameter("Ids", "Ids", "Application based id objects", GH_ParamAccess.list);
            pManager.AddGenericParameter("Elements", "Elements", "imported BHoM objects", GH_ParamAccess.list);
            pManager.AddGeometryParameter("Geometry", "Geometry", "geometry of BHoM objects", GH_ParamAccess.tree);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (GHE.DataUtils.Run(DA, 2))
            {
                BHI.IElementAdapter app = GHE.DataUtils.GetGenericData<BHI.IElementAdapter>(DA, 0);
                if (app != null)
                {
                    List<string> ids = null;
                    List<string> outIds = null;
                    IGH_DataTree geom = null;
                    app.Selection = m_Selection;
                    if (m_Selection == BHI.ObjectSelection.FromInput)
                        ids = GHE.DataUtils.GetDataList<string>(DA, 1);
                    List<T> objects = GetObjects(app, ids, out geom, out outIds);

                    DA.SetDataList(0, outIds);
                    DA.SetDataList(1, objects);
                    DA.SetDataTree(2, geom);
                }
            }
        }

        public abstract List<T> GetObjects(BHI.IElementAdapter app,  List<string> objectIds, out IGH_DataTree geom, out List<string> outIds);
        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            if (m_Options != null)
            {
                writer.SetString("EnumOption", m_Options.Text);
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
                // Menu
                Menu_AppendSeparator(menu);
                Menu_AppendObjectHelp(menu);
                return true;
            }
            else
            {
                return base.AppendMenuItems(menu);
            }
        }
    }
}
