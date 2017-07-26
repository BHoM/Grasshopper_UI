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
    public class StatLineChart : GH_Component
    {
        public StatLineChart() : base("StatLineChart", "StatLineChart", "Define the config for a statistical line chart", "Alligator", "Chrome") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("845B1D8F-F863-4D0C-8FC4-34078B773F2B");
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
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("config", "config", "config", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string parent = "", xDim = "", yDim = "";
            DA.GetData<string>(0, ref parent);
            DA.GetData<string>(1, ref xDim);
            DA.GetData<string>(2, ref yDim);

            List<string> config = new List<string>();

            config.Add("type: statLineChart");
            config.Add("parent: " + parent);
            config.Add("xDim: " + xDim);
            config.Add("yDim: " + yDim);

            DA.SetDataList(0, config);
        }
    }
}
