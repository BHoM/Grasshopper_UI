using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using CA = BH.Adapter.Chrome;

namespace BH.UI.Alligator.Chrome
{
    public class DataKey : GH_Component
    {
        public DataKey() : base("DataKey", "DataKey", "Create a key defining where data can be found.", "Alligator", "Chrome") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("EA02DAF5-EEC8-4C6D-9D86-612001595FAB");
            }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.tertiary;
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("name", "name", "Name of the dataset where data can be found.", GH_ParamAccess.item, "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("key", "key", "key", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string name = "";
            DA.GetData<string>(0, ref name);

            string key = "data: " + name;

            DA.SetData(0, key);
        }
    }
}
