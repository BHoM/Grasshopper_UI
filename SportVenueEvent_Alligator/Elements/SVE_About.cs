using System;
using BHoM.Structural;
using Grasshopper.Kernel;
using System.Collections.Generic;

namespace Alligator.SportVenueEvent.General
{
    public class About : GH_Component 
    {
        public About() : base("I", "Info", "Sport venue event components for building specific objects and methods", "SportVenueEvent", "General") { }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Info", "Info", "About SVE", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("Info", "Info", "About SVE", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("2220dc1b-87b3-491a-93fa-1495315ca5a3"); }
        }
    }
}
