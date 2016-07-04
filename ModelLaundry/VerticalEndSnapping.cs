using System;
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
    public class VerticalEndSnapping : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the VerticalPointSnaping class.
        /// </summary>
        public VerticalEndSnapping() : base("VerticalEndSnapping", "VEndSnap", "Description", "Alligator", "ModelLaundry") { }
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
            List<BH.Curve> refContour = new List<BH.Curve>();
            List<double> refHeight = new List<double>();
            BH.Polyline snapGeomPoly = null;
            BH.Curve output = null;
            BH.Curve contour = Utils.GetGenericData<BH.Curve>(DA, 0);
            double tol = Utils.GetData<double>(DA, 2);
            List<BH.Point> tempPts = new List<BHoM.Geometry.Point>();
            List<BH.Polyline> newPolyLines = new List<BHoM.Geometry.Polyline>();

            if(contour is BHoM.Geometry.PolyCurve)
            {
                List<BH.Curve> tempCrv = contour.Explode();
                tempPts = new List<BH.Point>();
                for (int j = 0; j < tempCrv.Count; j++)
                {
                    tempPts.Add(tempCrv[j].StartPoint);
                }

                if (contour.IsClosed())
                {
                    tempPts.Add(tempCrv[tempCrv.Count - 1].EndPoint);
                }

                snapGeomPoly = new BH.Polyline(tempPts);
            }

            try
            {
                refContour = Utils.GetGenericDataList<BH.Curve>(DA, 1);

                for (int i = 0; i < refContour.Count; i++)
                {
                    tempPts = new List<BHoM.Geometry.Point>();
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

                if (contour is BHoM.Geometry.PolyCurve)
                {
                    output = Snapping.VerticalEndSnap(snapGeomPoly, newPolyLines, tol);
                }
                else if (contour is BHoM.Geometry.Line)
                {
                    output = Snapping.VerticalEndSnap((BH.Line)contour, newPolyLines, tol);
                }

            }
            catch{}

            try
            {
                refHeight = Utils.GetGenericDataList<double>(DA, 1);
                if (contour is BHoM.Geometry.PolyCurve)
                {
                    output = Snapping.VerticalEndSnap((BH.Polyline)contour, refHeight, tol);
                }
                else if (contour is BHoM.Geometry.Line)
                {
                    output = Snapping.VerticalEndSnap(snapGeomPoly, refHeight, tol);
                }

            }
            catch { }

            DA.SetData(0, output);
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
                return base.Icon;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("d1634fe1-9e9a-47c0-9728-6566e76e524d"); }
        }
    }
}