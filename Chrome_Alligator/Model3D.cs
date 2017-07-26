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
    public class Model3D : GH_Component
    {
        public Model3D() : base("Model3D", "Model3D", "Define the configuration of the 3d models pushed to Chrome.", "Alligator", "Chrome") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("ACBF7646-24C4-4ECE-9245-6D898976F68B");
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
            pManager.AddTextParameter("camPos", "camPos", "Define the position of the camera. Format: [x,y,z]", GH_ParamAccess.item, "");
            pManager.AddTextParameter("camTarget", "camTarget", "Define the target of the camera. Format: [x,y,z]", GH_ParamAccess.item, "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("config", "config", "config", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string parent = "", dim = "", pos = "", target = "";
            bool controls = true, autoplay = false;
            DA.GetData<string>(0, ref parent);
            DA.GetData<string>(1, ref dim);
            DA.GetData<string>(2, ref pos);
            DA.GetData<string>(3, ref target);

            List<string> config = new List<string>();

            config.Add("type: model3d");
            config.Add("parent: " + parent);

            if (dim.Length > 0)
                config.Add("dim: " + dim);

            if (pos.Length > 0)
                config.Add("camPos: " + pos);

            if (target.Length > 0)
                config.Add("camTarget: " + target);

            DA.SetDataList(0, config);
        }
    }
}
