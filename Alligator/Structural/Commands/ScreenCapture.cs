using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grasshopper.Kernel;
using GHE = Grasshopper_Engine;
using BHI = BHoM.Structural.Interface;

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
            bool success = false;

            if (GHE.DataUtils.GetData<bool>(DA, 4))
            {
                BHI.ICommandAdapter adapter = GHE.DataUtils.GetData<BHI.ICommandAdapter>(DA, 0);
                string filename = GHE.DataUtils.GetData<string>(DA, 1);
                List<string> cases = GHE.DataUtils.GetDataList<string>(DA, 2);
                
                List<string> viewnames = GHE.DataUtils.GetDataList<string>(DA, 3);
                string test = adapter.ToString();

                success = adapter.ScreenCapture(filename, cases, viewnames);

                DA.SetData(0, success);
            }
            
        }
    }
}
