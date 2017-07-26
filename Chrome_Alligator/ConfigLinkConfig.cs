using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
//using GHE = Grasshopper_Engine;
using CA = BH.Adapter.Chrome;

namespace Alligator.Mongo
{
    public class ConfigLinkConfig : GH_Component
    {
        public ConfigLinkConfig() : base("CLConfig", "CLConfig", "Define the configuration of config link pushed to Chrome.", "Alligator", "Chrome") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("A88A9D61-1C05-48E2-8362-1FE8B9207EDA");
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
            string key = "type: configlink";

            DA.SetData(0, key);
        }
    }
}
