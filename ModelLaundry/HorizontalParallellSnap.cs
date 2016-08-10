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

            // Get the geometry of the element
            BHG.GeometryBase geometry = null;
            if (element is BHB.BHoMObject)
                geometry = ((BHB.BHoMObject)element).GetGeometry();
            else if (element is BHG.GeometryBase)
                geometry = element as BHG.GeometryBase;
            BHG.BoundingBox ROI = geometry.Bounds();
            ROI.Inflate(tol);

            // Get the geometry of the ref elements
            List<BHG.Curve> refGeom = new List<BHG.Curve>();
            foreach (object refElem in refElements)
            {
                BHG.GeometryBase geom = null;
                if (refElem is BHB.BHoMObject)
                    geom = ((BHB.BHoMObject)refElem).GetGeometry();
                else if (refElem is BHG.GeometryBase)
                    geom = refElem as BHG.GeometryBase;

                if (BHG.BoundingBox.InRange(ROI, geom.Bounds()))
                {
                    if (geom is BHG.Curve)
                        refGeom.Add((BHG.Curve)geom);
                    else if (geom is BHG.Group<BHG.Curve>)
                    {
                        List<BHG.Curve> list = BHG.Curve.Join((BHG.Group<BHG.Curve>)geom);
                        refGeom.Add(list[0]);
                    }
                }
            }

            // Do the actal snapping
            BHG.GeometryBase output = null;
            if (geometry is BHG.Curve)
            {
                output = MLE.Snapping.HorizontalParallelSnap((BHG.Curve)geometry, refGeom, tol);
            }
            else if (geometry is BHG.Group<BHG.Curve>)
            {
                output = MLE.Snapping.HorizontalParallelSnap((BHG.Group<BHG.Curve>)geometry, refGeom, tol);
            }

            // Prepare the result
            object result = element;
            if (element is BHB.BHoMObject)
            {
                result = ((BHB.BHoMObject)element).ShallowClone();
                ((BHB.BHoMObject)result).SetGeometry(output);
            }
            else if (element is BHG.GeometryBase)
            {
                result = output;
            }

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