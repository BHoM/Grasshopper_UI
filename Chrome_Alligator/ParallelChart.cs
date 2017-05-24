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
    public class ParallelChart : GH_Component
    {
        public ParallelChart() : base("ParallelChart", "ParallelChart", "Define the config for a parallel coordinates chart", "Alligator", "Chrome") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("B52696FC-ADD4-4291-A302-00A804579635");
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
            pManager.AddTextParameter("dims", "dims", "Dimensions used by the graph. If not provided, all dimensions available in the data will be used.", GH_ParamAccess.list);

            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("config", "config", "config", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string parent = "";
            List<string> dims = new List<string>();

            DA.GetData<string>(0, ref parent);
            bool userDims = DA.GetDataList<string>(1, dims);

            List<string> config = new List<string>();

            config.Add("type: parallelChart");
            config.Add("parent: " + parent);
            
            if (userDims)
            {
                var json = "[";
                foreach (string dim in dims)
                {
                    if (dim.StartsWith("{"))
                        json += dim + ", ";
                    else
                        json += "\"" + dim + "\", ";
                }
                    
                json = json.TrimEnd(new char[] { ',', ' ' }) + "]";
                config.Add("dims: " + json);
            }

            DA.SetDataList(0, config);
        }
    }
}
