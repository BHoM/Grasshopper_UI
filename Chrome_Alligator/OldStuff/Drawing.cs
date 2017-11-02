using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;

using CA = BH.Adapter.Chrome;

namespace BH.UI.Alligator.Chrome
{
    public class Drawing : GH_Component
    {
        public Drawing() : base("Drawing", "Drawing", "Define the config for an svg drawing", "Alligator", "Chrome") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("1E56E2E3-5E22-4EC1-8E69-394F0119341E");
            }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("parent", "parent", "Define the parent to this component", GH_ParamAccess.item, "body");
            pManager.AddTextParameter("bounds", "bounds", "Define the bounds of the choroplet. If nothing is provided, the bounds will be calculated automatically", GH_ParamAccess.item, "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("config", "config", "config", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string parent = "", bounds = "";
            DA.GetData<string>(0, ref parent);
            DA.GetData<string>(1, ref bounds);

            List<string> config = new List<string>();

            config.Add("type: drawing");
            config.Add("parent: " + parent);

            if (bounds.Length > 0)
                config.Add("bounds: " + bounds);

            DA.SetDataList(0, config);
        }
    }
}
