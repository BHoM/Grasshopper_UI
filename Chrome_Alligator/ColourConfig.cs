using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
//using GHE = Grasshopper_Engine;
using CA = Chrome_Adapter;

namespace Alligator.Mongo
{
    public class ColourConfig : GH_Component
    {
        public ColourConfig() : base("ColourConfig", "ColourConfig", "Define the config for a set of colours to use in a chart", "Alligator", "Chrome") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("22DD336B-3856-434E-9454-08E57E1A98EC");
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
            pManager.AddColourParameter("colours", "colours", "list of colours.", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("config", "config", "config", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Color> colours = new List<Color>();
            DA.GetDataList<Color>(0, colours);

            string config = "colours: [";

            foreach (Color color in colours)
            {
                config += "\"rgba(" + color.R + "," + color.G + "," + color.B + "," + color.A + ")\",";
            }

            config = config.Trim(',') + "]";

            DA.SetData(0, config);
        }
    }
}
