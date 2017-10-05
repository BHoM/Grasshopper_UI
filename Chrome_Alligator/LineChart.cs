using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;

using CA = BH.Adapter.Chrome;

namespace BH.UI.Alligator.Chrome
{
    public class LineChart : GH_Component
    {
        public LineChart() : base("LineChart", "LineChart", "Define the config for a line chart", "Alligator", "Chrome") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("065B9673-CE40-45A8-8B96-D86D072B4AAE");
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
            pManager.AddTextParameter("xDim", "xDim", "Dimension definition for the x axis.", GH_ParamAccess.item);
            pManager.AddTextParameter("yDim", "yDim", "Dimension definition for the y axis.", GH_ParamAccess.item);
            pManager.AddTextParameter("cDim", "cDim", "Dimension definition for bubble colour. You can also plug a GH colour or leave it empty for the default color.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("gDim", "gDim", "Dimension used to group data into lines. If not provided, a single line connecting all the data will be drawn", GH_ParamAccess.item, "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("config", "config", "config", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string parent = "", xDim = "", yDim = "", gDim = "", cDim = "";
            DA.GetData<string>(0, ref parent);
            DA.GetData<string>(1, ref xDim);
            DA.GetData<string>(2, ref yDim);
            DA.GetData<string>(3, ref cDim);
            DA.GetData<string>(4, ref gDim);

            List<string> config = new List<string>();

            config.Add("type: lineChart");
            config.Add("parent: " + parent);
            config.Add("xDim: " + xDim);
            config.Add("yDim: " + yDim);

            if (gDim.Length > 0)
                config.Add("gDim: " + gDim);

            if (cDim.Length > 0)
            {
                if (cDim.StartsWith("{") || cDim.IndexOf(',') == -1)
                    config.Add("cDim: " + cDim);
                else
                    config.Add("cDim: rgb(" + cDim + ")");
            }

            DA.SetDataList(0, config);
        }
    }
}
