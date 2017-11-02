using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;

using CA = BH.Adapter.Chrome;

namespace BH.UI.Alligator.Chrome
{
    public class TextConfig : GH_Component
    {
        public TextConfig() : base("Text", "Text", "Define the configuration of the text pushed to Chrome.", "Alligator", "Chrome") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("0EAB593E-CF6A-4242-AD0B-D4C38933B07D");
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
            pManager.AddTextParameter("dim", "dim", "Dimension where the text is stored. Leave blank if the text is the data itself.", GH_ParamAccess.item, "");
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

            config.Add("type: text");
            config.Add("parent: " + parent);

            if (dim.Length > 0)
                config.Add("dim: " + dim);

            DA.SetDataList(0, config);
        }
    }
}
