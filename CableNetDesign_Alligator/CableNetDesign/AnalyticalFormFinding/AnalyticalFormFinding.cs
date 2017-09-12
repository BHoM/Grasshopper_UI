using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using BH.oM.Geometry;
using CableNetDesignToolkit;

namespace Alligator.Components
{
    public class AnalyticalFormFinding : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the TensionRingFormFinding class.
        /// </summary>
        public AnalyticalFormFinding()
          : base("AnalyticalFormFinding", "AFF",
              "Analytic form-finding of tension ring nodes",
              "Alligator", "FormFinding")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "CompressionPts", "Compression ring points", GH_ParamAccess.list);
            pManager.AddPointParameter("Points", "TensionPts", "Tension ring points", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Start Grid", "StartGridNumber", "Grid line on which to get initial elevation and angle", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Fixed grids", "Numbers of Fixed Grids", "Grid lines on which to enforce elevation and angle", GH_ParamAccess.list);
            pManager.AddNumberParameter("Angle", "Alpha", "Declination angle of cables on grid line N", GH_ParamAccess.item);
            pManager.AddNumberParameter("Surcharge", "Surcharge", "Tension ring point load", GH_ParamAccess.list, 1.0);
            pManager.AddTextParameter("Level TR or CR?", "TRCR", "Set TR for level tension ring or CR for level compression ring", GH_ParamAccess.item, "CR");
            pManager.AddIntegerParameter("No. iterations", "i", "Number of iterations for which the process runs", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("Step", "Step", "Distance step for each iteration", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_PointParam("Points", "cPtFF", "Recalculated compression ring points", GH_ParamAccess.list);
            pManager.Register_PointParam("Points", "tPtFF", "Recalculated tension ring points", GH_ParamAccess.list);
            pManager.Register_DoubleParam("Forces", "CRing", "Compression ring forces", GH_ParamAccess.list);
            pManager.Register_DoubleParam("Forces", "TRadial", "Radial cable tensions", GH_ParamAccess.list);
            pManager.Register_DoubleParam("Forces", "TRing", "Tension ring forces", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Point3d> cPts = new List<Point3d>();
            List<Point3d> tPts = new List<Point3d>();

            List<BH.oM.Geometry.Point> _cPts = new List<BH.oM.Geometry.Point>();
            List<BH.oM.Geometry.Point> _tPts = new List<BH.oM.Geometry.Point>();
            int startGrid = 0;
            List<int> fixedGrids = new List<int>();

            double alpha = 0.0;
            List<double> F = new List<double>();
            string levelTRCR = "";
            int iterations = 1;
            double heightStep = 0.0;

            if (!DA.GetDataList(0, cPts)) return;
            if (!DA.GetDataList(1, tPts)) return;
            if (!DA.GetData(2, ref startGrid)) return;
            if (!DA.GetDataList(3, fixedGrids)) return;
            if (!DA.GetData(4, ref alpha)) return;
            if (!DA.GetDataList(5, F)) return;
            if (!DA.GetData(6, ref levelTRCR)) return;
            if (!DA.GetData(7, ref iterations)) return;
            if (!DA.GetData(8, ref heightStep)) return;

            foreach (Point3d pt in cPts)
                _cPts.Add(new BH.oM.Geometry.Point(pt.X, pt.Y, pt.Z));

            foreach (Point3d pt in tPts)
                _tPts.Add(new BH.oM.Geometry.Point(pt.X, pt.Y, pt.Z));

            CableNet cableNet = new CableNet(_cPts, _tPts);

            if (levelTRCR == "CR")
                cableNet.FormFindTensionRing(startGrid, fixedGrids, iterations, heightStep, alpha, F);
            else if (levelTRCR == "TR")
                cableNet.FormFindCompressionRing(startGrid, fixedGrids, iterations, heightStep, alpha, F);

            cPts.Clear();
            tPts.Clear();

            foreach (BH.oM.Geometry.Point pt in cableNet.GetCompressionRingPoints())
                cPts.Add(new Point3d(pt.X, pt.Y, pt.Z));

            foreach (BH.oM.Geometry.Point pt in cableNet.GetTensionRingPoints())
                tPts.Add(new Point3d(pt.X, pt.Y, pt.Z));

            DA.SetDataList(0, cPts);
            DA.SetDataList(1, tPts);
            DA.SetDataList(2, cableNet.CompressionRingForces);
            DA.SetDataList(3, cableNet.RadialForces);
            DA.SetDataList(4, cableNet.TensionRingForces);
        }


        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{7b26a485-4fa9-4b25-8145-2a1ff239c51b}"); }
        }
    }
}