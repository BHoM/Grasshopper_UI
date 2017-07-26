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
    public class SliderFilter : GH_Component
    {
        public SliderFilter() : base("SliderFilter", "SliderFilter", "Define the config for a slider filter", "Alligator", "Chrome") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("D02B7315-11C8-45EA-ABE8-56A473D4BF25");
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
            pManager.AddTextParameter("dim", "dim", "dimension filtered by the slider", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("config", "config", "config", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string parent = "", dim = "";

            DA.GetData<string>(0, ref parent);
            DA.GetData<string>(1, ref dim);

            List<string> config = new List<string>();

            config.Add("type: sliderFilter");
            config.Add("parent: " + parent);
            config.Add("dim: " + dim);

            DA.SetDataList(0, config);
        }
    }
}
