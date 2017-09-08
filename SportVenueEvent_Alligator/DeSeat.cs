using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using RHG = Rhino.Geometry;

using BHG = BHoM.Geometry;
using SportVenueEvent.oM;

namespace SportVenueEvent_Alligator
{
    public class DeSeat : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DeSeat class.
        /// </summary>
        public DeSeat()
          : base("Deconstruct Seat", "DeSeat",
              "",
              "SportVenueEvent", "Stadium")
        {
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Seat", "Seat", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Position", "Position", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("Focus", "Focus", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Seat seat = new Seat();

            DA.GetData(0, ref seat);
            DA.SetData(0, seat.Geometry);
            DA.SetData(1, seat.FocusPoint);

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
            get { return new Guid("4d704d8f-32fa-401a-a44f-e2a83def3d31"); }
        }
    }
}