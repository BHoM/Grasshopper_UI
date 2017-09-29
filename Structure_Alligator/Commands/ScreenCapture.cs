using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BH.oM.Base;
using BH.oM.Structural.Interface;

namespace Alligator.Structural.Commands
{
    public class ScreenCapture : GH_Component
    {
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("0E276EE5-C656-484A-AA45-AFB8DA590509");
            }
        }

        public ScreenCapture() : base("ScreenCapture", "SCRNCapt", "Capture views from app and save as PNGs in a folder", "Structure", "Commands")
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "Application", "Application for screencapture", GH_ParamAccess.item);
            pManager.AddTextParameter("AppPath", "FilePath", "The full filepath of the application", GH_ParamAccess.item);
            pManager.AddTextParameter("Loadcases", "Cases", "The loadcases (Combinations) for the screencapture", GH_ParamAccess.list);
            pManager.AddTextParameter("ViewNames", "ViewNames", "The names of the views to be captured", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Activate", "Go", "Activate", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("success", "success", "The operation was hopefully successfull", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool success = false, activate = false;
            ICommandAdapter adapter = default(ICommandAdapter);
            string filename = "";
            List<string> cases = new List<string>();
            List<string> viewnames = new List<string>();

            if (DA.GetData(4, ref activate))
            {
                DA.GetData(0, ref adapter);
                filename = DA.BH_GetData(1, filename);
                cases = DA.BH_GetDataList(DA, 2);
                viewnames =DA.BH_GetDataList(3, viewnames);
                string test = adapter.ToString();
                success = adapter.ScreenCapture(filename, cases, viewnames);
            }
            DA.SetData(0, success);
        }
    }
}
