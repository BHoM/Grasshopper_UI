using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using BHG = BH.oM.Geometry;
using BH.oM.SportVenueEvent;
using BH.Engine.SportVenueEvent;
using BH.UI.Alligator.Base;
using BH.UI.Alligator;

namespace BH.UI.Grasshopper.SportVenueEvent
{
    public class ExitPath : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ExitPath class.
        /// </summary>
        public ExitPath()
          : base("EgressPath", "EgressPath",
              "",
              "SportVenueEvent", "Measure")
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.tertiary;
            }
        }


        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "Tier", "Tier", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Path", "Path", "", GH_ParamAccess.list);
            //pManager.AddParameter(new BHoMGeometryParameter(), "Path", "Path", "", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Tier tier = new Tier();
            List<BHG.Polyline> polylines = new List<BHG.Polyline>();
            tier = DA.BH_GetData(0, tier);
            DA.SetDataList(0, tier.EgressPath());
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("f85e8f4b-a500-4d70-a929-59f4cfe00b09"); }
        }
    }
}