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
    public class ChromeLink : GH_Component
    {
        public ChromeLink() : base("ChromeLink", "ChromeLink", "Create a link to Chrome", "Alligator", "Chrome") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("3138C272-AF6E-451A-A535-C3256B4525AB");
            }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.primary;
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("port", "port", "port used by the socket. Value between 3000 and 9000", GH_ParamAccess.item, 3000);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("link", "link", "link to Chrome", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int port = 0; // = GHE.DataUtils.GetData<int>(DA, 0);
            DA.GetData<int>(0, ref port);
            DA.SetData(0, new CA.ChromeAdapter(port));
        }
    }
}
