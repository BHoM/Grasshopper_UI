using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using ModelLaundry_Engine;
using BHoM.Geometry;
using Grasshopper.Kernel;
using Rhino.Geometry;
using BH = BHoM.Geometry;
using R = Rhino.Geometry;

namespace Alligator.ModelLaundry
{
    public class HorizontalSnapToShape : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the HorizontalPointSnap class.
        /// </summary>
        public HorizontalSnapToShape()
          : base("HorizontalSnapToShape", "HSnap2S",
              "Horizontal snapping to shapes",
              "Alligator", "ModelLaundry")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("BHoM Elements", "bhElements", "BHoM object or geometry to snap", GH_ParamAccess.item);
            pManager.AddGenericParameter("Reference BHoM Elements", "refBHElem", "reference BHoM object or geometry to snap to", GH_ParamAccess.list);
            pManager.AddNumberParameter("Tolerance", "tolerance", "Set a tolerance for the snapping", GH_ParamAccess.item, 0.2);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SnappedElement", "snapped", "resulting BHoM object or geometry", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            object element = Utils.GetGenericData<object>(DA, 0);
            List<object> refElements = Utils.GetGenericDataList<object>(DA, 1);
            double tol = Utils.GetData<double>(DA, 2);

            object result = Snapping.HorizontalSnapToShape(element, refElements, tol);
            DA.SetData(0, result);
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
            get { return new Guid("{ad59f890-c387-4a68-a2cc-bc401725779d}"); }
        }
    }
}