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
    public class DimConfig : GH_Component
    {
        public DimConfig() : base("DimConfig", "DimConfig", "Define the config for a dimension to use in a chart", "Alligator", "Chrome") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("220A1FA4-B3B5-4E62-8FF3-2A8DD7B374A2");
            }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.tertiary;
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("property", "property", "Property name used for that axis", GH_ParamAccess.item);
            pManager.AddTextParameter("domain", "domain", "Domain covered by the axis. For numerical domain, use this format: \"[min, max] or a GH domain\".", GH_ParamAccess.item, "");
            pManager.AddTextParameter("title", "title", "Text used for the tile. If not provided, dimension will be used.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("display", "display", "Choose between 'hidden', 'visible', and 'active'. 'active' is the default state and means the selection brush is available.", GH_ParamAccess.item, "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("config", "config", "config", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string property = "", domain = "", title = "", display = "";
            DA.GetData<string>(0, ref property);
            DA.GetData<string>(1, ref domain);
            DA.GetData<string>(2, ref title);
            DA.GetData<string>(3, ref display);

            string config = "{\"property\": \"" + property + "\"";

            if (domain.Length > 0)
            {
                if (domain.StartsWith("["))
                    config += ", \"domain\": " + domain + "";
                else if (domain.IndexOf(" To ") != -1)
                {
                    string[] split = domain.Split(new string[] { " To " }, StringSplitOptions.RemoveEmptyEntries);
                    if (split.Count() >= 2)
                        config += ", \"domain\": [" + split[0] + ", " + split[1] + "]";
                }
            }
                

            if (title.Length > 0)
                config += ", \"title\": \"" + title + "\"";

            if (display.Length > 0)
                config += ", \"display\": \"" + display + "\"";

            config += "}";

            DA.SetData(0, config);
        }
    }
}
