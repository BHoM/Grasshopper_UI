using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHoM.Structural;
using System.IO;
using System.Drawing;

namespace Revit_Alligator
{
    public class RevitApp : GH_Component
    {
        public RevitApp() : base("Revit Application", "RevitApp", "Creates a Revit Application", "Structure", "Application") { }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Settings", "S", "Application Settings", GH_ParamAccess.item);
            pManager[0].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "A", "Revit Application", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string directory = Path.Combine(Path.GetTempPath(), "RevitExchange");
            FileIO app = new FileIO(Path.Combine(directory,"Out"), Path.Combine(directory, "In"));
            app.Identifier = BHoM.Base.FilterOption.UserData;
            app.Key = "Revit Id";
            DA.SetData(0, app);
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("97b9d554-41a2-4ded-b835-432332fb760a"); }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override Bitmap Internal_Icon_24x24
        {
            get { return Revit_Alligator.Properties.Resources.BHoM_Revit_App; }
        }

    }
}
