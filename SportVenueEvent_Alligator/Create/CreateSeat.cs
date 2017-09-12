using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using RHG = Rhino.Geometry;

using BHG = BH.oM.Geometry;
using BH.oM.SportVenueEvent;

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
            /*
            pManager.AddGenericParameter("Position", "Position", "Seat location", GH_ParamAccess.list);
            pManager.AddGenericParameter("Direction", "Direction", "Seat direction", GH_ParamAccess.list);
            pManager.AddTextParameter("Name", "ID", "Seat identifier", GH_ParamAccess.list);
            pManager.AddNumberParameter("C-Value", "C-Value", "C-value parameter", GH_ParamAccess.list);
            pManager.AddNumberParameter("A-Value", "A-Value", "A-Value parameter", GH_ParamAccess.list);
            pManager.AddNumberParameter("S-Value", "S-Value", "S-Value", GH_ParamAccess.list);
            pManager.AddNumberParameter("STI", "STI", "Speech Transmittion Index", GH_ParamAccess.list);
            pManager.AddNumberParameter("Noise", "Noise", "Sound Pressure Level at point", GH_ParamAccess.list);

            pManager[0].Optional = false;
            pManager[1].Optional = true;
            pManager[2].Optional = false;
            pManager[3].Optional = true;
            pManager[4].Optional = true;
            pManager[5].Optional = true;
            pManager[6].Optional = true;
            pManager[7].Optional = true;
            */

            pManager.AddGenericParameter("Position", "Position", "Seat location", GH_ParamAccess.list);
            pManager.AddGenericParameter("Focues", "Focus", "Seat focus", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Seats", "Seats", "List of BHoM stadium seats", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Seat> seats = new List<Seat>();
            /*
            List<BHG.Point> position = new List<BHG.Point>();
            List<BHG.Vector> direction = new List<BHG.Vector>();
            List<string> ID = new List<string>();
            List<double?> cValue = new List<double?>();
            List<double?> aValue = new List<double?>();
            List<double?> sValue = new List<double?>();
            List<double?> sti = new List<double?>();
            List<double?> noise = new List<double?>();

            if (!DA.GetDataList(0, position)) { return; }
            if (!DA.GetDataList(1, direction)) { direction = null; }
            if (!DA.GetDataList(2, ID)) { ID = null; }
            if (!DA.GetDataList(3, cValue)) { cValue = null; }
            if (!DA.GetDataList(4, aValue)) { aValue = null; }
            if (!DA.GetDataList(5, sValue)) { sValue = null; }
            if (!DA.GetDataList(6, sti)) { sti = null; }
            if (!DA.GetDataList(7, noise)) { noise = null; }
            */

            List<BHG.Point> positions = new List<BHG.Point>();
            List<BHG.Point> focuses = new List<BHG.Point>();

            DA.GetDataList(0, positions);
            DA.GetDataList(1, focuses);



            for (int i = 0; i < positions.Count; i++)
            {
                seats.Add(new Seat(positions[i], focuses[i]));   
            }
            DA.SetDataList(0, seats);
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