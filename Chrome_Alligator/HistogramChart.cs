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
    public class HistogramChart : GH_Component
    {
        public HistogramChart() : base("HistogramChart", "HistogramChart", "Define the config for an histogram chart", "Alligator", "Chrome") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("EE74AF73-D0FC-4966-84B5-525428B05F91");
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
            pManager.AddTextParameter("dim", "dim", "Dimension used by the histogram", GH_ParamAccess.item);
            pManager.AddNumberParameter("step", "step", "Step used to group the data (i.e. size of the buckets). Only relevant for numerical data.", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("gap", "gap", "Gap between the bars (expressed in pixels).", GH_ParamAccess.item, -1);
            pManager.AddNumberParameter("barWidth", "barWidth", "Forced bar width (in pixels). Gap is ignored if this value is provided", GH_ParamAccess.item, 0);

        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("config", "config", "config", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string parent = "", dim = "";
            double step = 0, gap = 0, barWidth = 0;
            DA.GetData<string>(0, ref parent);
            DA.GetData<string>(1, ref dim);
            DA.GetData<double>(2, ref step);
            DA.GetData<double>(3, ref gap);
            DA.GetData<double>(4, ref barWidth);

            List<string> config = new List<string>();

            config.Add("type: histogramChart");
            config.Add("parent: " + parent);
            config.Add("dim: " + dim);

            if (step > 0)
                config.Add("step: " + step);

            if (gap > -1)
                config.Add("gap: " + gap);

            if (barWidth > 0)
                config.Add("barWidth: " + barWidth);

            DA.SetDataList(0, config);
        }
    }
}
