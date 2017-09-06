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
    public class DataLink : GH_Component
    {
        public DataLink() : base("DataLink", "DataLink", "Define the configuration of a link between datasets.", "Alligator", "Chrome") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("D45A6846-1F01-4CDF-A3F9-CE8CF540F12D");
            }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.quinary;
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("set", "set", "datasets to link", GH_ParamAccess.item);
            pManager.AddTextParameter("dim", "dim", "Corresponding dimension to link for each dataset.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("link", "link", "link", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string set = "", dim = "";
            DA.GetData<string>(0, ref set);
            DA.GetData<string>(1, ref dim);

            string link = "{\"set\": \"" + set + "\", \"dim\": \"" + dim + "\"}";

            DA.SetData(0, link);
        }
    }
}
