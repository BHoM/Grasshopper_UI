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
    public class MarginConfig : GH_Component
    {
        public MarginConfig() : base("MarginConfig", "MarginConfig", "Define the margins for a chrome chart", "Alligator", "Chrome") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("CF573889-B361-435F-9244-3E244B98D6A0");
            }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.tertiary;
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("top", "top", "top margin", GH_ParamAccess.item, 30);
            pManager.AddIntegerParameter("bottom", "bottom", "bottom margin", GH_ParamAccess.item, 30);
            pManager.AddIntegerParameter("left", "left", "left margin", GH_ParamAccess.item, 30);
            pManager.AddIntegerParameter("right", "right", "right margin", GH_ParamAccess.item, 30);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("config", "config", "config", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int top = 0, bottom = 0, left = 0, right = 0;
            DA.GetData<int>(0, ref top);
            DA.GetData<int>(1, ref bottom);
            DA.GetData<int>(2, ref left);
            DA.GetData<int>(3, ref right);

            string config = "margin: {\"top\": " + top + ", \"right\": " + right + ", \"bottom\": " + bottom + ", \"left\": " + left + "}";

            DA.SetData(0, config);
        }
    }
}
