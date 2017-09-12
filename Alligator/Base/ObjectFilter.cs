using Grasshopper.Kernel;
using Grasshopper_Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GH_IO.Serialization;
using BH.oM.Base;

namespace Alligator.Base
{
    public class ObjectFilter : GH_Component
    {
        public ComboBox _FilterType;
        public string _SelectedOption;
        public ObjectFilter() : base("ObjectFilter", "ObjectFilter", "Filter a list of BH.oM objects", "Alligator", "Base")
        {
            _FilterType = new ComboBox();
            _FilterType.DropDownStyle = ComboBoxStyle.DropDownList;
            _FilterType.Items.AddRange(Enum.GetNames(typeof(BH.oM.Base.FilterOption)));

            _FilterType.SelectedIndex = 0;
            _FilterType.SelectedValueChanged += _FilterType_SelectedValueChanged;
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BH.oM_Filter; }
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        private void _FilterType_SelectedValueChanged(object sender, EventArgs e)
        {
            _SelectedOption = _FilterType.Text;
            this.ExpireSolution(true);
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("45912883-EE6F-49F4-BEBA-55155EC2370C");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("BHoMObjects", "Objects", "List of BH.oM to apply filter to", GH_ParamAccess.list);
            pManager.AddTextParameter("Match Parameter", "Key", "Name of property or key to match", GH_ParamAccess.item);
            pManager.AddGenericParameter("Match Value", "Values", "Values to match", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("BH.oM object", "object", "Resulting BH.oM object", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Dictionary<string, string> addedTypes = new Dictionary<string, string>();
            List<BH.oM.Base.BHoMObject> objects = DataUtils.GetGenericDataList<BH.oM.Base.BHoMObject>(DA, 0);
            string key = DataUtils.GetData<string>(DA, 1);
            List<string> values = DataUtils.GetGenericDataList<string>(DA, 2);
            FilterOption option = (FilterOption)Enum.Parse(typeof(FilterOption), _SelectedOption);

            Dictionary<string, BHoMObject> filter = new BH.oM.Base.ObjectFilter(objects).ToDictionary<string>(key, option);

            List<BH.oM.Base.BHoMObject> result = new List<BHoMObject>();

            for (int i = 0; i < values.Count; i++)
            {
                BHoMObject obj = null;
                filter.TryGetValue(values[i], out obj);
                result.Add(obj);
            }

            DA.SetDataList(0, result);
        }

        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            writer.SetString("Filter Type", _SelectedOption);
       
            return base.Write(writer);
        }

        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            string selection = "";
            reader.TryGetString("Filter Type", ref selection);
            _SelectedOption = selection;
                         
            return base.Read(reader);
        }
        public override bool AppendMenuItems(ToolStripDropDown menu)
        {

            Menu_AppendObjectName(menu);// (menu, "Section");
            Menu_AppendEnableItem(menu);
            Menu_AppendBakeItem(menu);
            Menu_AppendSeparator(menu);

            Menu_AppendCustomItem(menu, _FilterType);
                
            Menu_AppendSeparator(menu);
            // Menu
            Menu_AppendSeparator(menu);
            Menu_AppendObjectHelp(menu);
            return true;
        }
    }
}
