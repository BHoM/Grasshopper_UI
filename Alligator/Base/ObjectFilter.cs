using Grasshopper.Kernel;
using Grasshopper_Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GH_IO.Serialization;

namespace Alligator.Base
{
    public class ObjectFilter : GH_Component
    {
        public ComboBox _FilterType;
        public Type _SelectedType;
        public ObjectFilter() : base("ObjectFilter", "ObjectFilter", "Filter a list of BHoM objects", "Alligator", "Base")
        {
            _FilterType = new ComboBox();
            _FilterType.DropDownStyle = ComboBoxStyle.DropDownList;
            _FilterType.SelectedValueChanged += _FilterType_SelectedValueChanged;
        }

        private void _FilterType_SelectedValueChanged(object sender, EventArgs e)
        {
            _SelectedType = Type.GetType(_FilterType.Text);
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
            pManager.AddGenericParameter("BHoMObjects", "Objects", "List of BHoM to apply filter to", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("BHoM object", "object", "Resulting BHoM object", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string json = ""; // Utils.GetGenericData<string>(DA, 0);
            Dictionary<string, string> addedTypes = new Dictionary<string, string>();
            List<BHoM.Base.BHoMObject> objects = DataUtils.GetGenericDataList<BHoM.Base.BHoMObject>(DA, 0);
            for (int i = 0; i < objects.Count;i++)
            {
                string name = "";
                if (!addedTypes.TryGetValue(objects.GetType().FullName, out name))
                {
                    addedTypes.Add(objects.GetType().FullName, objects.GetType().FullName);
                }
            }
            _FilterType.DataSource = addedTypes.Keys.ToList();
            if (_FilterType.SelectedItem.ToString() != _SelectedType.FullName)
            {
                _FilterType.SelectedItem = _SelectedType.ToString();

                DA.SetDataList(0, new BHoM.Base.ObjectFilter(objects).OfClass(_SelectedType));
            }
        }

        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            writer.SetString("Type Filter", _SelectedType.FullName);
       
            return base.Write(writer);
        }

        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            string selection = "";
            reader.TryGetString("Type Filter", ref selection);
            _SelectedType = Type.GetType(selection);
                         
            return base.Read(reader);
        }
    }
}
