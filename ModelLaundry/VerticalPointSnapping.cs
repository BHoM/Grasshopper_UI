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
    public class VerticalPointSnapping : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the VerticalPointSnapping class.
        /// </summary>
        public VerticalPointSnapping() : base("VerticalPointSnapping", "VPtSnap", "Description", "Alligator", "ModelLaundry") { }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("SnapGeometry", "SnapGeom", "Input an an BHoM polyline", GH_ParamAccess.item);
            pManager.AddGenericParameter("ReferenceGeometry", "RefGeom", "Input an an BHoM Brep", GH_ParamAccess.list);
            pManager.AddNumberParameter("Tolerance", "Tol", "Set a tolerance for the snapping", GH_ParamAccess.item, 0.2);
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
            BH.Point pt = Utils.GetGenericData<BH.Point>(DA, 0);
            List<BH.Polyline> BHoMBrep = Utils.GetGenericDataList<BH.Polyline>(DA, 1);
            double tol = Utils.GetData<double>(DA, 2);

            BH.Point outPt = Snapping.VerticalPointSnap(pt, BHoMBrep, tol);
            DA.SetData(0, outPt);

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
            get { return new Guid("{734dea33-7c5b-4a19-bdba-3367ace95d49}"); }
        }
    }
}