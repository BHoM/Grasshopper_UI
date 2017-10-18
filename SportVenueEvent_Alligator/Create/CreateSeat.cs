using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using RHG = Rhino.Geometry;

using BH.oM.Base;
using BHG = BH.oM.Geometry;
using BH.oM.SportVenueEvent;
using BH.UI.Alligator.Base;
using BH.UI.Alligator;

namespace BH.UI.Grasshopper.SportVenueEvent
{
    public class CreateSeat : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the SeatMaker class.
        /// </summary>
        public CreateSeat()
          : base("CreateSeat", "Seat",
              "Create a BHoM Stadium Seat",
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
            pManager.AddParameter(new BHoMGeometryParameter(), "Position", "Position", "Seat location", GH_ParamAccess.list);
            pManager.AddParameter(new BHoMGeometryParameter(), "Focues", "Focus", "Seat focus", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "Seats", "Seats", "List of BHoM stadium seats", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Seat> seats = new List<Seat>();
            List<BHG.Point> positions = new List<BHG.Point>();
            List<BHG.Point> focuses = new List<BHG.Point>();

            positions = DA.BH_GetDataList(0, positions);
            focuses = DA.BH_GetDataList(1, focuses);
            for (int i = 0; i < positions.Count; i++)
            {
                seats.Add(new Seat(positions[i], focuses[i]));
            }
            DA.BH_SetDataList(0, seats);
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
            get { return new Guid("{8e1edd37-c593-4758-86d6-22df94d3f52e}"); }
        }
    }
}