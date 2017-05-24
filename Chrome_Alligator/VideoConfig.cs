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
    public class VideoConfig : GH_Component
    {
        public VideoConfig() : base("Video", "Video", "Define the configuration of the videos pushed to Chrome.", "Alligator", "Chrome") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("823ED29C-ADBB-4CCC-AF31-1ED71BE39F8E");
            }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.quarternary;
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("parent", "parent", "Define the parent to this component", GH_ParamAccess.item, "body");
            pManager.AddTextParameter("dim", "dim", "Dimension where the video is stored. Leave blank if the video is the data itself.", GH_ParamAccess.item, "");
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

            config.Add("type: video");
            config.Add("parent: " + parent);

            if (dim.Length > 0)
                config.Add("dim: " + dim);

            DA.SetDataList(0, config);
        }
    }
}
