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
    public class HorizontalExtend : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the HorizontalExtend class.
        /// </summary>
        public HorizontalExtend()
          : base("HorizontalExtend", "HExtend",
              "Description",
              "Alligator", "ModelLaundry")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("GeometryToExtend", "GeomToExtend", "Line or Polyline to extend", GH_ParamAccess.item);
            pManager.AddGenericParameter("Distance", "Dist", "Extention distance", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ExtendedGeometry", "ExtendedGeom", "New BHoMLine or BHoMPolyline", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BH.Curve contour = Utils.GetGenericData<BH.Curve>(DA, 0);
            double dist = Utils.GetData<double>(DA, 1);
            List<BH.Point> tempPts = new List<BHoM.Geometry.Point>();
            BH.Polyline newPolyLine;
            BH.Curve output = null;



            if (contour is BH.Polyline)
            {
                output = Util.HorizontalExtend((BH.Polyline)contour, dist);
            }

            else if (contour is BH.Line)
            {
                output = Util.HorizontalExtend((BH.Line)contour, dist);
            }

            else if(contour is BH.PolyCurve)
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

                newPolyLine = new BH.Polyline(tempPts);
                output = Util.HorizontalExtend(newPolyLine, dist);
            }

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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{486fee2f-8167-416d-a4f1-5f9dda0e0570}"); }
        }
    }
}