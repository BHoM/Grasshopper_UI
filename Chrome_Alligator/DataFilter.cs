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
    public class DataFilter : GH_Component
    {
        public DataFilter() : base("DataFilter", "DataFilter", "Create a data filter.", "Alligator", "Chrome") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("401D7E42-DBF1-4D0F-8399-0C07A2C7A3C5");
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
            pManager.AddTextParameter("dims", "dims", "Dimensions the filter will be applied to", GH_ParamAccess.list);
            pManager.AddTextParameter("domains", "domains", "Domains corresponding to the data that will be kept by the filter for each property. For numerical domain, use this format: \"[min, max] or a GH domain\".", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("filter", "filter", "filter", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<string> properties = new List<string>();
            List<string> domains = new List<string>();
            DA.GetDataList<string>(0, properties);
            DA.GetDataList<string>(1, domains);

            string filter = "{";

            int nb = Math.Min(properties.Count, domains.Count);
            for (int i = 0; i < nb; i++)
            {
                string range = "[-Infinity, Infinity]";
                string domain = domains[i];

                if (domain.StartsWith("["))
                    range =  domain;
                else if (domain.IndexOf(" To ") != -1)
                {
                    string[] split = domain.Split(new string[] { " To " }, StringSplitOptions.RemoveEmptyEntries);
                    if (split.Count() >= 2)
                        range = "[" + split[0] + ", " + split[1] + "]";
                }
                filter += "\"" + properties[i] + "\": " + range;
            }

            filter = filter.TrimEnd(new char[] { ',', ' ' }) + "}";

            DA.SetData(0, filter);
        }
    }
}
