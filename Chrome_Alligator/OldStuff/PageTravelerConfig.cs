using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;

using CA = BH.Adapter.Chrome;

namespace BH.UI.Alligator.Chrome
{
    public class PageTravelerConfig : GH_Component
    {
        public PageTravelerConfig() : base("PageTravellerConfig", "PageTravellerConfig", "Define the configuration of the page traveller pushed to Chrome.", "Alligator", "Chrome") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("3226C45A-2CA8-4EA8-8FB0-6711333CBC4E");
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
            string key = "type: pageTraveller";

            DA.SetData(0, key);
        }
    }
}
