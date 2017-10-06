using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;

using CA = BH.Adapter.Chrome;

namespace BH.UI.Alligator.Chrome
{
    public class TextFilter : GH_Component
    {
        public TextFilter() : base("TextFilter", "TextFilter", "Define the config for a text filter (filter that is activated when the text is clicked)", "Alligator", "Chrome") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("89B5A6B8-A889-49FA-84BE-FAEE04FB5D89");
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
            pManager.AddTextParameter("text", "text", "text displayed by the filter", GH_ParamAccess.item, "");
            pManager.AddTextParameter("filter", "filter", "data filter used when clicked on the text", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("config", "config", "config", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string parent = "", text = "", filter = "";

            DA.GetData<string>(0, ref parent);
            DA.GetData<string>(1, ref text);
            DA.GetData<string>(2, ref filter);

            List<string> config = new List<string>();

            config.Add("type: textFilter");
            config.Add("parent: " + parent);

            if (text.Length > 0)
                config.Add("text: " + text);

            config.Add("filter: " + filter);

            DA.SetDataList(0, config);
        }
    }
}
