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
    public class VerticalPointSnaping : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the VerticalPointSnaping class.
        /// </summary>
        public VerticalPointSnaping() : base("VerticalPointSnaping", "VPtSnap", "Description", "Alligator", "ModelLaundry") { }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Polyline", "Polyln", "Input an an BHoM polyline", GH_ParamAccess.item);
            pManager.AddGenericParameter("Surface", "Srf", "Input an an BHoM Brep", GH_ParamAccess.list);
            pManager.AddNumberParameter("Tolerance", "Tol", "Set a tolerance for the snapping", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Result", "res", "New BHoM Polyline", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            object BHoMInputGeom = null;
            List<object> BHoMBrep = new List<object>();
            List<double> references = new List<double>();
            double tol = 10;
            object outputBHoMGeom = new object();

            //BH.Curve c = GeometryUtils.Convert(Utils.GetData<R.Curve>(DA, 0)) as BH.Curve;
            //List<BH.GeometryBase> BHoMBrep = Utils.GetGenericDataList<BH.GeometryBase>(DA, 1);
            //references = Utils.GetData<double>(DA, 2);
            if (!DA.GetData(0, ref BHoMInputGeom)) return;
            if (!DA.GetDataList(1, BHoMBrep)) return;
            if (!DA.GetData(2, ref tol)) return;


            if (BHoMInputGeom.GetType().ToString() == "BHoM.Geometry.Polyline")
            {

                List<BHoM.Geometry.Polyline> BHoMPolyline = new List<BHoM.Geometry.Polyline>();
                foreach (object o in BHoMBrep)
                {
                    if (o.GetType().ToString() == "BHoM.Geometry.Polyline")
                    {
                        BHoMPolyline.Add(o as BHoM.Geometry.Polyline);
                        outputBHoMGeom = Snapping.VerticalEndSnap((BHoM.Geometry.Polyline)BHoMInputGeom, BHoMPolyline, tol);
                    }

                    if (o.GetType().ToString() == "double")
                    {
                        references.Add((double)o);
                        outputBHoMGeom = Snapping.VerticalEndSnap((BHoM.Geometry.Polyline)BHoMInputGeom, references, tol);
                    }
                }
            }

            DA.SetData(0, outputBHoMGeom);

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