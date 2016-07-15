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
            pManager.AddNumberParameter("Distance", "Dist", "Extention distance", GH_ParamAccess.item);
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
            // Getting the inputs from GH
            object element = Utils.GetGenericData<object>(DA, 0);
            double dist = Utils.GetData<double>(DA, 1);

            // Get the geometry
            BH.GeometryBase geometry = null;
            if (element is BHoM.Global.BHoMObject)
                geometry = ((BHoM.Global.BHoMObject)element).GetGeometry();
            else if (element is BH.GeometryBase)
                geometry = element as BH.GeometryBase;

            BH.GeometryBase output = null;
            if (geometry is BH.Line)
            {
                output = Util.HorizontalExtend((BH.Line)geometry, dist);
            }
            else if (geometry is BH.Curve)
            {
                output = Util.HorizontalExtend((BH.Curve)geometry, dist);
            }
            else if (geometry is BH.Group<BH.Curve>)
            {
                output = Util.HorizontalExtend((BH.Group<BH.Curve>)geometry, dist);
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
                
            /*else if(geometry is BH.PolyCurve)
            {
                BH.PolyCurve curve = geometry as BH.PolyCurve;
                List<BH.Curve> tempCrv = curve.Explode();
                List<BH.Point> tempPts = new List<BH.Point>();
                for (int j = 0; j < tempCrv.Count; j++)
                {
                    tempPts.Add(tempCrv[j].StartPoint);
                }

                if (curve.IsClosed())
                {
                    tempPts.Add(tempCrv[tempCrv.Count - 1].EndPoint);
                }

                BH.Polyline newPolyLine = new BH.Polyline(tempPts);
                output = Util.HorizontalExtend(newPolyLine, dist);
            }*/

                // Setting the GH outputs
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
            get { return new Guid("{486fee2f-8167-416d-a4f1-5f9dda0e0570}"); }
        }
    }
}