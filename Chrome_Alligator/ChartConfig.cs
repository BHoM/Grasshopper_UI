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
    public class ChartConfig : GH_Component
    {
        public ChartConfig() : base("ChartConfig", "ChartConfig", "Define the config for a chrome chart", "Alligator", "Chrome") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("C456EF6C-6091-4841-9E6B-E91D476D839A");
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
            pManager.AddIntegerParameter("width", "width", "Chart width. If 0, ignore that config.", GH_ParamAccess.item, 0);
            pManager.AddIntegerParameter("height", "height", "Chart height. If 0, ignore that config.", GH_ParamAccess.item, 0);
            pManager.AddTextParameter("margin", "margin", "Chart margin. If '', ignore that config.", GH_ParamAccess.item, "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("config", "config", "config", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int width = 0, height = 0;
            string margin = "";
            DA.GetData<int>(0, ref width);
            DA.GetData<int>(1, ref height);
            DA.GetData<string>(2, ref margin);


            List<string> config = new List<string>();

            if (width > 0)
                config.Add("width: " + width);
            if (height > 0)
                config.Add("height: " + height);
            if (margin.StartsWith("margin:"))
                config.Add(margin);

            DA.SetDataList(0, config);
        }
    }
}
