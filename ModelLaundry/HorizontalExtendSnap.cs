using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using BHoM_Engine.ModelLaundry;
using BHoM.Geometry;
using Grasshopper.Kernel;
using Rhino.Geometry;
using BH = BHoM.Geometry;
using R = Rhino.Geometry;

namespace Alligator.ModelLaundry
{
    public class HorizontalExtendSnap : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the HorizontalPointSnap class.
        /// </summary>
        public HorizontalExtendSnap()
          : base("HorizontalExtendSnap", "HPtSnap",
              "Horizontal extention snapping",
              "Alligator", "ModelLaundry")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("GeometryToSnap", "GeomToSnap", "Input an an BHoM polyline", GH_ParamAccess.item);
            pManager.AddGenericParameter("GeometryToSnapTo", "GeomToSnapTo", "Input an an BHoM Brep", GH_ParamAccess.list);
            pManager.AddNumberParameter("Tolerance", "Tol", "Set a tolerance for the snapping", GH_ParamAccess.item, 0.2);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SnapedGeometry", "SnapedGeom", "New BHoM Polyline", GH_ParamAccess.item);
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
                output = Snapping.HorizontalPointSnap((BH.Curve)geometry, refGeom, tol);
            }
            else if (geometry is BH.Group<BH.Curve>)
            {
                output = Snapping.HorizontalPointSnap((BH.Group<BH.Curve>)geometry, refGeom, tol);
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

            /*BH.GeometryBase contour = Utils.GetGenericData<BH.GeometryBase>(DA, 0);
            double tol = Utils.GetData<double>(DA, 2);
            List<BH.Polyline> newPolyLines = new List<BHoM.Geometry.Polyline>();
            List<BH.Curve> refContour = Utils.GetGenericDataList<BH.Curve>(DA, 1);
            List<BH.Point> tempPts = new List<BHoM.Geometry.Point>();
            List<BH.Point> ptLinetoPLine = new List<BHoM.Geometry.Point>();
            BH.Polyline output = null;

            if (contour is BHoM.Geometry.Group<BH.Curve>)
            {
                Group<BH.Curve> groupOfCrvs = contour as Group<BH.Curve>;
                List<BH.Point> pts = new List<BH.Point>();
                List<BH.Curve> crvs = BH.Curve.Join(groupOfCrvs.ToList());
                contour = crvs[0] as BH.PolyCurve;
            }

            if (contour is BHoM.Geometry.Line)
            {
                BH.Line ln;
                ln = contour as BH.Line;
                ptLinetoPLine.Add(ln.PointAt(0));
                ptLinetoPLine.Add(ln.PointAt(0.5));
                ptLinetoPLine.Add(ln.PointAt(1));

                BH.Polyline lineToPolyLn = new BHoM.Geometry.Polyline(ptLinetoPLine);
                contour = lineToPolyLn;
            }

            if (contour is BHoM.Geometry.PolyCurve)
            {
                BH.PolyCurve pCrv = contour as BH.PolyCurve;
                List<BH.Point> pts = new List<BH.Point>();
                List<BH.Curve> crvs = pCrv.Explode();
                if (pCrv.IsClosed())
                {
                    for (int i = 0; i < crvs.Count; i++)
                    {
                        pts.Add(crvs[i].StartPoint);
                    }
                }

                if (!pCrv.IsClosed())
                {
                    for (int i = 0; i < crvs.Count; i++)
                    {
                        pts.Add(crvs[i].StartPoint);
                    }
                    pts.Add(crvs[crvs.Count - 1].EndPoint);
                }

                contour = new BH.Polyline(pts);
            }

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

                else if(refContour[i] is BHoM.Geometry.Polyline)
                {
                    newPolyLines.Add((BH.Polyline)refContour[i]);
                }
            }

            output = Snapping.HorizontalPointSnap((BH.Polyline)contour, newPolyLines, tol);

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
            get { return new Guid("{ad59f890-c387-4a68-a2cc-bc401725779d}"); }
        }
    }
}