using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using MLE = ModelLaundry_Engine;
using GHE = Grasshopper_Engine;
using BHG = BH.oM.Geometry;
using RG = Rhino.Geometry;

namespace Alligator.ModelLaundry
{
    public class PointSnapDiagnostic : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the HorizontalExtend class.
        /// </summary>
        public PointSnapDiagnostic() : base("PointSnapDiagnostic", "PSDiagnostic", "Check that oints within tolerance are correctly snapped", "Alligator", "ModelLaundry") {}

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("BH.oM Elements", "bhElements", "BH.oM object or geometry to diagnose", GH_ParamAccess.list);
            pManager.AddNumberParameter("Distance", "distance", "tolerance distance", GH_ParamAccess.item);
            pManager.AddNumberParameter("zero", "zero", "zero threshold", GH_ParamAccess.item, BH.oM.Base.Tolerance.MIN_DIST);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Error locations", "errors", "Locations where two points are not snapped properly", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Getting the inputs from GH
            List<object> elements = GHE.DataUtils.GetGenericDataList<object>(DA, 0);
            double dist = GHE.DataUtils.GetData<double>(DA, 1);
            double minDist = GHE.DataUtils.GetData<double>(DA, 2);

            List<BHG.Point> result = MLE.Diagnostic.CheckSnappedPoints(elements, dist, minDist);
            List<RG.Point3d> locations = new List<Rhino.Geometry.Point3d>();
            foreach (BHG.Point pt in result)
                locations.Add(new RG.Point3d(pt.X, pt.Y, pt.Z));
            DA.SetDataList(0, locations);
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
            get { return new Guid("42663B13-33AC-4E0D-8D5C-BF25F3955EAD"); }
        }
    }
}