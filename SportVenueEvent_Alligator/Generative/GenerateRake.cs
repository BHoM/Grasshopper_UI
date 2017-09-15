using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using BH.oM.SportVenueEvent;
using BH.Engine.SportVenueEvent;

namespace BH.UI.Grasshopper.SportVenueEvent
{
    public class GenerateRake : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GenerateRake class.
        /// </summary>
        public GenerateRake()
          : base("GenerateRakes", "Rakes",
              "",
              "SportVenueEvent", "Generative")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Tier", "Tier", "", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Seats per row", "Count", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("GangwayWidth", "GangWidth", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("SeatWidth", "SeatWidth", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Tier", "Tier", "", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Tier> tiers = new List<Tier>();
            int seats = 0;
            double gangway = 0;
            double width = 0;

            DA.GetDataList(0, tiers);
            DA.GetData(1, ref seats);
            DA.GetData(2, ref gangway);
            DA.GetData(3, ref width);

            DA.SetDataList(0, tiers.GenRakes(seats, gangway, width));
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
            get { return new Guid("c62a1572-ef95-4b24-ba8a-cb70c7a825f7"); }
        }
    }
}