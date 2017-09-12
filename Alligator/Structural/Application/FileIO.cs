using BH.oM.Structural;
using Grasshopper.Kernel;
using Grasshopper_Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alligator.Structural.Application
{
    public class FileApp : GH_Component
    {
        public FileApp() : base("File Input Output", "FileIO", "Imports or Exports BH.oM files to text", "Structure", "Application") { }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("In Directory", "In", "Directory to read information from", GH_ParamAccess.item);
            pManager.AddTextParameter("Out Directory", "Out", "Directory to write information to", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "A", "File Application", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string directoryIn = DataUtils.GetData<string>(DA, 0);
            string directoryOut = DataUtils.GetData<string>(DA, 1);

            FileIO app = new FileIO(directoryIn, directoryOut);
            app.Identifier = BH.oM.Base.FilterOption.Guid;
            DA.SetData(0, app);
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("97b9d234-41a2-4ded-b835-432332fb760a"); }
        }

    }
}
