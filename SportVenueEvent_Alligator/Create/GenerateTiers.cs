using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using RHG = Rhino.Geometry;

using BHG = BH.oM.Geometry;
using GHE = Grasshopper_Engine;
using BH.oM.SportVenueEvent;
using BH.Engine.SportVenueEvent;
using BH.UI.Alligator.Base;

namespace BH.UI.Grasshopper.SportVenueEvent
{
    public class GenerateTiers : GH_Component, IGH_PreviewObject
    {
        /// <summary>
        /// Initializes a new instance of the GenerateTiers class.
        /// </summary>
        public GenerateTiers()
          : base("Generative Tier", "Tiers",
              "Generates a set of tiers according to a certain c-Value",
              "SportVenueEvent", "Generative")
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
            pManager.AddParameter(new BHoMObjectParameter(), "First Row of seats", "Seats", "", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Tiers number", "Tiers", "", GH_ParamAccess.item);
            pManager.AddParameter(new BHoMGeometryParameter(), "BH Vector", "Vector", "Vector list that defines the position of the new tier", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Rows", "Rows", "", GH_ParamAccess.list);
            pManager.AddNumberParameter("Seat Depth", "Depth", "", GH_ParamAccess.list);
            pManager.AddNumberParameter("C-Value", "C", "", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "Tier", "Tier", "", GH_ParamAccess.list);
            pManager.AddParameter(new BHoMObjectParameter(), "Samples", "Samples", "", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Seat> seats = new List<Seat>();
            int tierNo = 1;
            List<int> rowsNos = new List<int>();
            List<double> depths = new List<double>();
            List<double> cValues = new List<double>();
            List<BHG.Vector> tierPos = new List<BHG.Vector>();

            DA.BH_GetDataList(0, seats);
            DA.GetData(1, ref tierNo);
            DA.BH_GetDataList(2, tierPos);
            DA.GetDataList(3, rowsNos);
            DA.GetDataList(4, depths);
            DA.GetDataList(5, cValues);

            List<Tier> tiers = Create.GenTiers(seats, tierNo, tierPos, rowsNos, depths, cValues);
            DA.BH_SetDataList(0, tiers);
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
            get { return new Guid("73b808bb-8107-41a8-b751-319b92f1f175"); }
        }
    }
}