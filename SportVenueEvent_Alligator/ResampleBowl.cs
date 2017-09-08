﻿using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using SportVenueEvent.En;
using SportVenueEvent.oM;

namespace SportVenueEvent_Alligator
{
    public class ResampleBowl : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ResampleBowl class.
        /// </summary>
        public ResampleBowl()
          : base("ResampleBowl", "ReBowl",
              "",
              "SporVenueEvent", "Stadium")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Tiers", "Tiers", "", GH_ParamAccess.list);
            pManager.AddNumberParameter("Seat width", "Width", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Tiers", "Tiers", "", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Tier> tiers = new List<Tier>();
            double width = 0;
            DA.GetDataList(0, tiers);
            DA.GetData(1, ref width);

            Bowl bowl = new Bowl(tiers);
            DA.SetDataList(0, bowl.ResampleBowl(width));
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
            get { return new Guid("d038b156-bc7c-4377-8d83-aae19775b444"); }
        }
    }
}