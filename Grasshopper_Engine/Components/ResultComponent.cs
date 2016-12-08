using BHoM.Base.Results;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GHE = Grasshopper_Engine;

namespace Grasshopper_Engine.Components
{
    public enum EnvelopeOption
    {
        None,
        Max,
        Min,
        Abs
    }

    public abstract class ResultBaseComponent<T> : GH_Component where T : IResult, new()
    {
        protected ComboBox m_Options;
        protected ComboBox m_EnvelopeOption;
        protected ResultOrder m_ResultOrder;
        protected EnvelopeOption m_EnvelopeType;
        public ResultBaseComponent() { }

        protected ResultBaseComponent(string name, string nickname, string description, string category, string subCat) : base(name, nickname, description, category, subCat)
        {
            string[] OptionNames = Enum.GetNames(typeof(ResultOrder));
            m_Options = new ComboBox();
            m_Options.DropDownStyle = ComboBoxStyle.DropDownList;
            m_Options.SelectedValueChanged += OptionChanged;
            m_Options.Items.AddRange(OptionNames);
            m_Options.SelectedIndex = 0;

            string[] EnvelopeTpes = Enum.GetNames(typeof(EnvelopeOption));
            m_EnvelopeOption = new ComboBox();
            m_EnvelopeOption.DropDownStyle = ComboBoxStyle.DropDownList;
            m_EnvelopeOption.SelectedValueChanged += EnvelopeOptionChanged;
            m_EnvelopeOption.Items.AddRange(EnvelopeTpes);
            m_EnvelopeOption.SelectedIndex = 0;
        }

        private void EnvelopeOptionChanged(object sender, EventArgs e)
        {
            m_EnvelopeType = (EnvelopeOption)Enum.Parse(typeof(EnvelopeOption), m_EnvelopeOption.SelectedItem.ToString());
            SetMessage();
            this.ExpireSolution(true);
        }

        private void OptionChanged(object sender, EventArgs e)
        {
            m_ResultOrder = (ResultOrder)Enum.Parse(typeof(ResultOrder), m_Options.SelectedItem.ToString());
            SetMessage();
            this.ExpireSolution(true);
        }

        private void SetMessage()
        {
            if (m_EnvelopeType != EnvelopeOption.None)
            {
                this.Message = m_EnvelopeType.ToString() + " Envelope By " + m_Options.SelectedItem.ToString();
            }
            else
            {
                this.Message = m_Options.SelectedItem.ToString();
            }
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid();
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Result Server", "ResultServer", "Application or Result server to extract results from", GH_ParamAccess.item);
            pManager.AddTextParameter("Ids", "Id", "List of object ids to get results for", GH_ParamAccess.list);
            pManager.AddTextParameter("Loadcase", "Loadcase", "List of Loadcases to get results for", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Activate", "Activate", "Run the component", GH_ParamAccess.item);
            Params.Input[1].Optional = true;
            Params.Input[2].Optional = true;
          //  Params.Input[1].
          //  Params.Input[2].AddVolatileData(new GH_Path(0), 0, null);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            PropertyInfo value = null;
            Type pType = null;
            string[] columnHeaders = new T().ColumnHeaders;
            for (int i = 0; i < columnHeaders.Length; i++)
            {
                value = typeof(T).GetProperty(columnHeaders[i]);
                pType = value.PropertyType;
                string description = GHE.DataUtils.GetDescription(value);
                if (pType == typeof(string))
                {
                    pManager.AddTextParameter(columnHeaders[i], columnHeaders[i], description, GH_ParamAccess.tree);
                }
                else if (GHE.DataUtils.IsNumeric(pType))
                {
                    if (GHE.DataUtils.IsInteger(pType)) pManager.AddTextParameter(columnHeaders[i], columnHeaders[i], description, GH_ParamAccess.tree);
                    else pManager.AddNumberParameter(columnHeaders[i], columnHeaders[i], description, GH_ParamAccess.tree);
                }
            }
        }

        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            if (m_Options != null)
            {
                writer.SetString("EnumOption", m_Options.Text);
                writer.SetString("EnvelopeOption", m_EnvelopeOption.Text);
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
                reader.TryGetString("EnvelopeOption", ref selection);
                m_EnvelopeOption.SelectedItem = selection;
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
                Menu_AppendItem(menu, "Order:");
                Menu_AppendCustomItem(menu, m_Options);
                Menu_AppendItem(menu, "Envelope:");
                Menu_AppendCustomItem(menu, m_EnvelopeOption);
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

        protected override void SolveInstance(IGH_DataAccess DA) { }

        public void SetResults<S>(IGH_DataAccess DA, Dictionary<string, IResultSet> data) where S : IResult, new()
        {
            string[] columnHeaders = new S().ColumnHeaders;
            List<DataTree<object>> output = new List<DataTree<object>>();

            for (int i = 0; i < columnHeaders.Length; i++)
            {
                output.Add(new DataTree<object>());
            }

            int branch = 0;
            
            foreach (IResultSet set in data.Values)
            {
                GH_Path path = new GH_Path(branch++);
                Envelope envelope = null;

                switch (m_EnvelopeType)
                {
                    case EnvelopeOption.None:
                        foreach (object[] result in set.ToListData())
                        {
                            for (int i = 0; i < result.Length; i++)
                            {
                                output[i].Add(result[i], path);
                            }
                        }
                        break;
                    case EnvelopeOption.Max:
                        envelope = set.MaxEnvelope();
                        break;
                    case EnvelopeOption.Min:
                        envelope = set.MinEnvelope();
                        break;
                    case EnvelopeOption.Abs:
                        envelope = set.AbsoluteEnvelope();
                        break;
                }

                if (envelope != null)
                {
                    object[] results = new object[columnHeaders.Length];
                    int startIndex = columnHeaders.ToList().IndexOf(envelope.Names[0]);
                    output[0].AddRange(envelope.Keys, path);
                    output[1].AddRange(envelope.Names, path);
                    output[2].AddRange(envelope.Cases, path);
                    for (int i = 0; i < envelope.Names.Length; i++)
                    {
                        output[startIndex + i].Add(envelope.Values[i], path);
                    }
                }
            }

            for (int i = 0; i < output.Count; i++)
            {
                DA.SetDataTree(i, output[i]);
            }
        }
    }
}
