using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BHoM.Structural.Elements;
using BHoM.Structural.ModelTracker;
using GHE = Grasshopper_Engine;

namespace Alligator.Structural.ModelTracker
{
    public class GenerateBarTrackers : GH_Component
    {
        public GenerateBarTrackers() : base("BarTracker", "BarTracker", "Generate tracker objects from bars", "Structure", "ModelTracking")
        { }
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("840A5FB7-DF2C-49FA-9480-56D60C0D1F81");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Bar", "Bar", "Bar to generate tracker from", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Bar tracker", "Bar tracker", "The constructed bar tracker object", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Bar bar = GHE.DataUtils.GetGenericData<Bar>(DA, 0);
            if (bar != null)
                DA.SetData(0, BarTracker.TrackerFromBar(bar));
        }
    }
}
