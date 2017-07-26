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
    public class BubbleChart : GH_Component
    {
        public BubbleChart() : base("BubbleChart", "BubbleChart", "Define the config for a bubble chart", "Alligator", "Chrome") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("F9A25FC7-87E7-44A6-9FEB-1FF90603794B");
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
            pManager.AddTextParameter("rDim", "rDim", "Dimension definition for the bubble radius. If a single number is provided, all bubbles will have that radius. If nothing is provided, the default size will be used.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("cDim", "cDim", "Dimension definition for bubble colour. You can also plug a GH colour or leave it empty for the default color.", GH_ParamAccess.item, "");
            
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("config", "config", "config", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string parent = "", xDim = "", yDim = "", rDim = "", cDim = "";
            DA.GetData<string>(0, ref parent);
            DA.GetData<string>(1, ref xDim);
            DA.GetData<string>(2, ref yDim);
            DA.GetData<string>(3, ref rDim);
            DA.GetData<string>(4, ref cDim);

            List<string> config = new List<string>();

            config.Add("type: bubbleChart");
            config.Add("parent: " + parent);
            config.Add("xDim: " + xDim);
            config.Add("yDim: " + yDim);

            if (rDim.Length > 0)
                config.Add("rDim: " + rDim);

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
