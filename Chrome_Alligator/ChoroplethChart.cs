using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
//using GHE = Grasshopper_Engine;
using CA = Chrome_Adapter;

namespace Alligator.Mongo
{
    public class ChoroplethChart : GH_Component
    {
        public ChoroplethChart() : base("ChoroplethChart", "Choropleth", "Define the config for a choropleth chart", "Alligator", "Chrome") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("ABAA8416-F6D5-4DC5-8D78-E6B7507414C2");
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
            pManager.AddTextParameter("rDim", "rDim", "Dimension definition for the regions.", GH_ParamAccess.item);
            pManager.AddTextParameter("cDim", "cDim", "Dimension definition for the regions' colour. You can also plug a GH colour or leave it empty for the default colour.", GH_ParamAccess.item);
            pManager.AddTextParameter("nDim", "nDim", "Dimension definition for the regions' name. If a single number is provided, all bubbles will have that radius. If nothing is provided, the regions will have no name.", GH_ParamAccess.item, "");
            pManager.AddNumberParameter("opacity", "opacity", "Define the opacity of the regions.", GH_ParamAccess.item, 1.0);
            pManager.AddTextParameter("bounds", "bounds", "Define the bounds of the choroplet. If nothing is provided, the bounds will be calculated automatically", GH_ParamAccess.item, "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("config", "config", "config", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double opacity = -1;
            string parent = "", rDim = "", cDim = "", nDim = "", bounds = "";
            DA.GetData<string>(0, ref parent);
            DA.GetData<string>(1, ref rDim);
            DA.GetData<string>(2, ref cDim);
            DA.GetData<string>(3, ref nDim);
            DA.GetData<double>(4, ref opacity);
            DA.GetData<string>(5, ref bounds);

            List<string> config = new List<string>();

            config.Add("type: choropletChart");
            config.Add("parent: " + parent);
            config.Add("xDim: " + rDim);
            config.Add("yDim: " + cDim);

            if (nDim.Length > 0)
                config.Add("nDim: " + nDim);

            if (bounds.Length > 0)
                config.Add("bounds: " + bounds);

            if (opacity >= 0)
                config.Add("opacity: " + opacity);

            DA.SetDataList(0, config);
        }
    }
}
