using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;

using CA = BH.Adapter.Chrome;

namespace BH.UI.Alligator.Chrome
{
    public class PageStop : GH_Component
    {
        public PageStop() : base("PageStop", "PageStop", "Define a page stop for the page traveller.", "Alligator", "Chrome") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("6D58F697-69F2-4ABB-B8F2-58C9FFA1F2A2");
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
            pManager.AddTextParameter("element", "element", "elemenet to stop to", GH_ParamAccess.item);
            pManager.AddTextParameter("topMargin", "topMargin", "Margin between the element and the top of the page.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("stop", "stop", "stop", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string element = "", margin = "";
            DA.GetData<string>(0, ref element);
            DA.GetData<string>(1, ref margin);

            string link = "{\"element\": \"" + element + "\", \"topMargin\": \"" + margin + "\"}";

            DA.SetData(0, link);
        }
    }
}
