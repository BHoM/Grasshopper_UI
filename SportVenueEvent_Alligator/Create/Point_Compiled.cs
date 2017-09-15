using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using BH.UI.Alligator.Base;
using BH.oM.Base;

namespace BH.UI.Grasshopper.SportVenueEvent
{
    public class Point_Compiled : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Point class.
        /// </summary>
        public Point_Compiled()
          : base("Point_Compiled", "Nickname",
              "Description",
              "SportVenueEvent", "Subcategory")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("A", "A", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("B", "B", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("C", "C", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("C", "C", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double x = 0, y = 0, z = 0;
            DA.GetData(0, ref x);
            DA.GetData(1, ref y);
            DA.GetData(2, ref z);
            DA.SetData(0, new BH.oM.Geometry.Point(x, y, z));
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
            get { return new Guid("f58313e2-3220-47fb-9e26-35cda777e129"); }
        }
    }
}