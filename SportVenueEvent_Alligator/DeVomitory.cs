﻿using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using RHG = Rhino.Geometry;

using BHG = BHoM.Geometry;
using SportVenueEvent.oM;

namespace SportVenueEvent_Alligator
{
    public class DeVomitory : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DeVomitory class.
        /// </summary>
        public DeVomitory()
          : base("Deconstruct Vomitory", "DeVom",
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
            pManager.AddGenericParameter("Vomitory", "Vom", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Tier", "Tier", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("Centre", "Centre", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("Width", "Width", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Vomitory vomitory = new Vomitory();

            DA.GetData(0, ref vomitory);
            DA.SetData(0, vomitory.Tier);
            DA.SetData(1, vomitory.Centre);
            DA.SetData(2, vomitory.Width);
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
            get { return new Guid("cf49230a-d626-4850-94d5-0505432e2908"); }
        }
    }
}