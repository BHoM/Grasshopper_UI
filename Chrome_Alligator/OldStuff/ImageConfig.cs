using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;

using CA = BH.Adapter.Chrome;

namespace BH.UI.Alligator.Chrome
{
    public class ImageConfig : GH_Component
    {
        public ImageConfig() : base("Image", "Image", "Define the configuration of the images pushed to Chrome.", "Alligator", "Chrome") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("DAD5740A-C04D-4E59-B3F7-4DE9093AA840");
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
            pManager.AddTextParameter("dim", "dim", "Dimension where the image is stored. Leave blank if the image is the data itself.", GH_ParamAccess.item, "");
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

            config.Add("type: image");
            config.Add("parent: " + parent);

            if (dim.Length > 0)
                config.Add("dim: " + dim);

            DA.SetDataList(0, config);
        }
    }
}
