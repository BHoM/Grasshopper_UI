using System;
using System.Drawing;
using System.Collections.Generic;
using BHoM_Engine.ModelLaundry;
using BHoM.Geometry;
using Grasshopper.Kernel;
using Rhino.Geometry;
using BH = BHoM.Geometry;
using R = Rhino.Geometry;
using System.Linq;

namespace Alligator.ModelLaundry
{
    public class VisualizeLines : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the VisualizeLines class.
        /// </summary>
        public VisualizeLines()
          : base("VisualizeLines", "Nickname",
              "Description",
              "Alligator", "ModelLaundry")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("BHoMLines", "BHoMLines", "Insert BHoM lines", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "Crv", "Rhino Curve", GH_ParamAccess.list);
        }



        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<R.Line> newLn = new List<R.Line>();
            List<R.Polyline> newPLine = new List<R.Polyline>();
            List<R.Curve> newCrvs = new List<R.Curve>();
            //BH.Curve crv = Utils.GetGenericData<BH.Curve>(DA, 0);
            object crv = Utils.GetGenericData<object>(DA, 0);
            List<BH.Curve> explodedCrvs = new List<BHoM.Geometry.Curve>();
            List<BH.Curve> output = new List<BHoM.Geometry.Curve>();

            if (crv is BHoM.Geometry.Group<BH.Curve>)
            {
                Group<BH.Curve> newCrv = crv as Group<BH.Curve>;
                explodedCrvs = newCrv.ToList();

                for (int i = 0; i < explodedCrvs.Count; i++)
                {
                    output.Add(explodedCrvs[i]);
                }
            }

            else
            {
                output.Add((BH.Curve)crv);
            }

            for (int i = 0; i < output.Count; i++)
            {
                if (output[i] is BHoM.Geometry.Line)
                {
                    BH.Line ln = output[i] as BH.Line;
                    R.LineCurve tempLn = new R.LineCurve(new R.Line(ln.StartPoint.X, ln.StartPoint.Y, ln.StartPoint.Z, ln.EndPoint.X, ln.EndPoint.Y, ln.EndPoint.Z));
                    newCrvs.Add(tempLn);
                }

                if (output[i] is BHoM.Geometry.Polyline)
                {
                    BH.Polyline pLine = output[i] as BH.Polyline;
                    List<R.Point3d> pts = new List<Point3d>();
                    for (int j = 0; j < pLine.ControlPoints.Count; j++)
                    {
                        R.Point3d tempTp = new R.Point3d(pLine.ControlPoints[j].X, pLine.ControlPoints[j].Y, pLine.ControlPoints[j].Z);
                        pts.Add(tempTp);
                    }
                    R.PolylineCurve tempPLnCrv = new PolylineCurve(new R.Polyline(pts));
                    newCrvs.Add(tempPLnCrv);
                }

                if (output[i] is BHoM.Geometry.PolyCurve)
                {
                    BH.PolyCurve pCrv = output[i] as BH.PolyCurve;
                    List<BH.Curve> segments = pCrv.Explode();
                    for (int j = 0; j < segments.Count; j++)
                    {
                        BH.Line ln = segments[j] as BH.Line;
                        R.LineCurve tempLn = new R.LineCurve(new R.Line(ln.StartPoint.X, ln.StartPoint.Y, ln.StartPoint.Z, ln.EndPoint.X, ln.EndPoint.Y, ln.EndPoint.Z));
                        newCrvs.Add(tempLn);
                    }
                }


            }

            DA.SetDataList(0, newCrvs);
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
            get { return new Guid("{9ab916d1-e82c-4b1b-bead-86de2e7856bd}"); }
        }
    }
}