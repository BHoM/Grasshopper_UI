using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using CA = BH.Adapter.Chrome;

namespace BH.UI.Alligator.Chrome
{
    public class DataLinkConfig : GH_Component
    {
        public DataLinkConfig() : base("DLConfig", "DLConfig", "Define the configuration of data link pushed to Chrome.", "Alligator", "Chrome") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("2B208285-3352-4C4B-84CE-46ECF444C9DE");
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
            string key = "type: datalink";

            DA.SetData(0, key);
        }
    }
}
