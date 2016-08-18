using Etabs_Adapter.Structural.Interface;
using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtabsAlligator.Application
{
    public class EtabsApp : GH_Component
    {
        public EtabsApp() : base("Etabs Application", "EtabsApp", "Creates an Etabs Application", "Structure", "Application") { }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Filename", "F", "Etabs Filename", GH_ParamAccess.item);
            pManager.AddGenericParameter("Settings", "S", "Application Settings", GH_ParamAccess.item);
            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "A", "Etabs Application", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string filename = "";  DA.GetData<string>(0, ref filename);
            EtabsAdapter app = new EtabsAdapter(filename);
            DA.SetData(0, app);
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("97b9ffd4-41a2-4ded-b835-432332fb760a"); }
        }    
    }
}
