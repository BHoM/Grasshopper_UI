using System;
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
            object element = Utils.GetGenericData<object>(DA, 0);
            List<object> refElements = Utils.GetGenericDataList<object>(DA, 1);
            double tol = Utils.GetData<double>(DA, 2);

            // Get the geometry of the element
            BH.GeometryBase geometry = null;
            if (element is BHoM.Global.BHoMObject)
                geometry = ((BHoM.Global.BHoMObject)element).GetGeometry();
            else if (element is BH.GeometryBase)
                geometry = element as BH.GeometryBase;
            BH.BoundingBox ROI = geometry.Bounds();
            ROI.Inflate(tol);

            // Get the geometry of the ref elements
            List<BH.Curve> refGeom = new List<BH.Curve>();
            foreach (object refElem in refElements)
            {
                BH.GeometryBase geom = null;
                if (refElem is BHoM.Global.BHoMObject)
                    geom = ((BHoM.Global.BHoMObject)refElem).GetGeometry();
                else if (refElem is BH.GeometryBase)
                    geom = refElem as BH.GeometryBase;

                if (BH.BoundingBox.InRange(ROI, geom.Bounds()))
                {
                    if (geom is BH.Curve)
                        refGeom.Add((BH.Curve)geom);
                    else if (geom is BH.Group<BH.Curve>)
                    {
                        List<BH.Curve> list = BH.Curve.Join((BH.Group<BH.Curve>)geom);
                        refGeom.Add(list[0]);
                    }
                }
            }

            // Do the actal snapping
            BH.GeometryBase output = null;
            if (geometry is BH.Curve)
            {
                output = Snapping.HorizontalParallelSnap((BH.Curve)geometry, refGeom, tol);
            }
            else if (geometry is BH.Group<BH.Curve>)
            {
                output = Snapping.HorizontalParallelSnap((BH.Group<BH.Curve>)geometry, refGeom, tol);
            }

            // Prepare the result
            object result = element;
            if (element is BHoM.Global.BHoMObject)
            {
                result = (BHoM.Global.BHoMObject)((BHoM.Global.BHoMObject)element).ShallowClone();
                ((BHoM.Global.BHoMObject)result).SetGeometry(output);
            }
            else if (element is BH.GeometryBase)
            {
                result = output;
            }

            DA.SetData(0, result);

            /*BH.Polyline contour = Utils.GetGenericData<BH.Polyline>(DA, 0);
            double tol = Utils.GetData<double>(DA, 2);
            List<BH.Polyline> newPolyLines = new List<BHoM.Geometry.Polyline>();
            List<BH.Curve> refContour = Utils.GetGenericDataList<BH.Curve>(DA, 1);
            List<BH.Point> tempPts = new List<BHoM.Geometry.Point>();
            BH.Polyline output = null;

            for (int i = 0; i < refContour.Count; i++)
            {
                if (refContour[i] is BHoM.Geometry.PolyCurve)
                {
                    List<BH.Curve> tempCrv = refContour[i].Explode();
                    tempPts = new List<BH.Point>();
                    for (int j = 0; j < tempCrv.Count; j++)
                    {
                        tempPts.Add(tempCrv[j].StartPoint);
                    }

                    if (refContour[i].IsClosed())
                    {
                        tempPts.Add(tempCrv[tempCrv.Count - 1].EndPoint);
                    }

                    newPolyLines.Add(new BH.Polyline(tempPts));
                }

                else if (refContour[i] is BHoM.Geometry.Polyline)
                {
                    newPolyLines.Add((BH.Polyline)refContour[i]);
                }
            }

            output = Snapping.HorizontalParallelSnap(contour, newPolyLines, tol);

            DA.SetData(0, output);*/
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