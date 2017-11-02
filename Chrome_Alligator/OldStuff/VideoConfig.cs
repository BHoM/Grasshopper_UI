using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;

using CA = BH.Adapter.Chrome;

namespace BH.UI.Alligator.Chrome
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
            pManager.AddBooleanParameter("controls", "controls", "Show video controls", GH_ParamAccess.item, true);
            pManager.AddBooleanParameter("autoplay", "autoplay", "Play the video automatically.", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("config", "config", "config", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string parent = "", dim = "";
            bool controls = true, autoplay = false;
            DA.GetData<string>(0, ref parent);
            DA.GetData<string>(1, ref dim);
            DA.GetData<bool>(2, ref controls);
            DA.GetData<bool>(3, ref autoplay);

            List<string> config = new List<string>();

            config.Add("type: video");
            config.Add("parent: " + parent);
            config.Add("controls: " + controls.ToString().ToLower());
            config.Add("autoplay: " + autoplay.ToString().ToLower());

            if (dim.Length > 0)
                config.Add("dim: " + dim);

            DA.SetDataList(0, config);
        }
    }
}
