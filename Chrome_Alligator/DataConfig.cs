using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using CA = BH.Adapter.Chrome;

namespace BH.UI.Alligator.Chrome
{
    public class DataConfig : GH_Component
    {
        public DataConfig() : base("DataConfig", "DataConfig", "Define the configuration of data pushed to Chrome.", "Alligator", "Chrome") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("D9F813A1-CD45-43CC-B539-5378D9AC13EA");
            }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.quinary;
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("config", "config", "config", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string key = "type: data";

            DA.SetData(0, key);
        }
    }
}
