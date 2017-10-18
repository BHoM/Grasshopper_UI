using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using BH.oM.SportVenueEvent;
using BH.Engine.SportVenueEvent;
using BH.UI.Alligator.Base;
using BH.UI.Alligator;

namespace BH.UI.Grasshopper.SportVenueEvent
{
    public class CreateTier : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateTier class.
        /// </summary>
        public CreateTier()
          : base("Create Tier", "Tier",
              "",
              "SportVenueEvent", "Create")
        {
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.primary;
            }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "Rows", "Rows", "", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "Tier", "Tier", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Row> rows = new List<Row>();
            rows = DA.BH_GetDataList(0, rows);
            DA.BH_SetData(0, new Tier(rows));
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
            get { return new Guid("d32777de-3e0d-49db-a8fe-3c3e18006b8a"); }
        }
    }
}