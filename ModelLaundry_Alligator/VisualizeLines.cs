using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using System.Linq;
using GHE = Grasshopper_Engine;
using BHG = BHoM.Geometry;
using RG = Rhino.Geometry;

namespace Alligator.ModelLaundry
{
    public class VisualizeLines : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the VisualizeLines class.
        /// </summary>
        public VisualizeLines()
          : base("ToGHLine", "ToGHLine",
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
            List<RG.Line> newLn = new List<RG.Line>();
            List<RG.Polyline> newPLine = new List<RG.Polyline>();
            List<RG.Curve> newCrvs = new List<RG.Curve>();
            //BH.Curve crv = Utils.GetGenericData<BH.Curve>(DA, 0);
            object crv = GHE.DataUtils.GetGenericData<object>(DA, 0);
            List<BHG.Curve> explodedCrvs = new List<BHG.Curve>();
            List<BHG.Curve> output = new List<BHG.Curve>();

            if (crv is BHoM.Geometry.Group<BHG.Curve>)
            {
                BHG.Group<BHG.Curve> newCrv = crv as BHG.Group<BHG.Curve>;
                explodedCrvs = newCrv.ToList();

                for (int i = 0; i < explodedCrvs.Count; i++)
                {
                    output.Add(explodedCrvs[i]);
                }
            }

            else
            {
                output.Add((BHG.Curve)crv);
            }

            for (int i = 0; i < output.Count; i++)
            {
                if (output[i] is BHoM.Geometry.Line)
                {
                    BHG.Line ln = output[i] as BHG.Line;
                    RG.LineCurve tempLn = new RG.LineCurve(new RG.Line(ln.StartPoint.X, ln.StartPoint.Y, ln.StartPoint.Z, ln.EndPoint.X, ln.EndPoint.Y, ln.EndPoint.Z));
                    newCrvs.Add(tempLn);
                }

                if (output[i] is BHoM.Geometry.Polyline)
                {
                    BHG.Polyline pLine = output[i] as BHG.Polyline;
                    List<RG.Point3d> pts = new List<RG.Point3d>();
                    for (int j = 0; j < pLine.ControlPoints.Count; j++)
                    {
                        RG.Point3d tempTp = new RG.Point3d(pLine.ControlPoints[j].X, pLine.ControlPoints[j].Y, pLine.ControlPoints[j].Z);
                        pts.Add(tempTp);
                    }
                    RG.PolylineCurve tempPLnCrv = new RG.PolylineCurve(new RG.Polyline(pts));
                    newCrvs.Add(tempPLnCrv);
                }

                if (output[i] is BHoM.Geometry.PolyCurve)
                {
                    BHG.PolyCurve pCrv = output[i] as BHG.PolyCurve;
                    List<BHG.Curve> segments = pCrv.Explode();
                    for (int j = 0; j < segments.Count; j++)
                    {
                        BHG.Curve ln = segments[j];
                        RG.LineCurve tempLn = new RG.LineCurve(new RG.Line(ln.StartPoint.X, ln.StartPoint.Y, ln.StartPoint.Z, ln.EndPoint.X, ln.EndPoint.Y, ln.EndPoint.Z));
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