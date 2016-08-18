using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using BHB = BHoM.Base;
using MLE = ModelLaundry_Engine;
using GHE = Grasshopper_Engine;
using BHG = BHoM.Geometry;


namespace Alligator.ModelLaundry
{
    public class HorizontalParallellSnap : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the HorizontalParallellSnap class.
        /// </summary>
        public HorizontalParallellSnap()
          : base("HorizontalParallellSnap", "HPSnap",
              "Description",
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
            pManager.AddBooleanParameter("AnyHeight", "anyheight", "Snap to a vertical projection of the reference elements", GH_ParamAccess.item, false);
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
            object element = GHE.DataUtils.GetGenericData<object>(DA, 0);
            List<object> refElements = GHE.DataUtils.GetGenericDataList<object>(DA, 1);
            double tol = GHE.DataUtils.GetData<double>(DA, 2);
            bool anyHeight = GHE.DataUtils.GetData<bool>(DA, 3);

            object result = MLE.Snapping.HorizontalParallelSnap(element, refElements, tol, anyHeight);
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
            get { return new Guid("{2e6985fe-02ec-4804-ac68-1d3dec838e70}"); }
        }
    }
}