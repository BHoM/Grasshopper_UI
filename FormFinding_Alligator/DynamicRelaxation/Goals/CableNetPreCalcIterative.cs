using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using FormFinding_Engine.Structural.CableNetFormFinding;
using FormFinding_Engine.Structural.Goals;
using BG = BHoM.Geometry;
using GHE = Grasshopper_Engine;


namespace FormFinding_Alligator.CableNetDesign
{
    public class CableNetPreCalcIterative : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CableNetPrecalculations class.
        /// </summary>
        public CableNetPreCalcIterative()
          : base("PstressGoalIter", "PstressGoal", "Prestress cable goals for cable net form finding with iterative setout of points", "Alligator", "Structure")
            
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("CR Points", "CRPts", "Compression ring points", GH_ParamAccess.list);
            pManager.AddPointParameter("TR Points", "TRPts", "Tension ring points", GH_ParamAccess.list);
            pManager.AddVectorParameter("Tension ring force", "TRForce", "Load Vectors need to be in the same plane as their radial and a Z-vector", GH_ParamAccess.list);
            pManager.AddVectorParameter("Compression ring force", "CRForce", "Load Vectors need to be in the same plane as their radial and a Z-vector", GH_ParamAccess.list);
            pManager.AddNumberParameter("Scale Factor", "sfac", "Scalingfactor for the prestressingforces", GH_ParamAccess.item);
            pManager.AddIntegerParameter("MaxIterations", "Iter", "Maximum iterations to run", GH_ParamAccess.item, 1000);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PstressGoal", "PstGoal", "ConstantHorizontalPrestressGoal", GH_ParamAccess.list);
            pManager.AddPointParameter("New Tr Pts", "TrPts", "New tensionring points", GH_ParamAccess.list);
            pManager.AddNumberParameter("Cr forces", "Cr forces", "Cr forces", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            List<Point3d> crPts = new List<Point3d>();
            List<Point3d> trPts = new List<Point3d>();
            List<Vector3d> trForce = new List<Vector3d>();
            List<Vector3d> crForce = new List<Vector3d>();
            double sFac = 0;
            int maxIter = 1000;

            if (!DA.GetDataList(0, crPts)) { return; }
            if (!DA.GetDataList(1, trPts)) { return; }
            if (!DA.GetDataList(2, trForce)) { return; }
            if (!DA.GetDataList(3, crForce)) { return; }
            if (!DA.GetData(4, ref sFac)) { return; }

            DA.GetData(5, ref maxIter);


            List<BG.Point> bgCrPts = new List<BG.Point>();
            List<BG.Point> bgTrPts = new List<BG.Point>();
            List<BG.Vector> trLoad = new List<BG.Vector>();
            List<BG.Vector> crLoad = new List<BG.Vector>();
            for (int i = 0; i < crPts.Count; i++)
            {
                BG.Point bgCrPt = GHE.GeometryUtils.Convert(crPts[i]);
                bgCrPts.Add(bgCrPt);

                BG.Point bgTrPt = GHE.GeometryUtils.Convert(trPts[i]);
                bgTrPts.Add(bgTrPt);

                BG.Vector loadV = GHE.GeometryUtils.Convert(trForce[i]);
                trLoad.Add(loadV);

                BG.Vector loadVCr = GHE.GeometryUtils.Convert(crForce[i]);
                crLoad.Add(loadVCr);
            }

            List<BG.Point> newTrPts;
            List<double> crPrestressForces;
            List<ConstantHorizontalPrestressGoal> goals = CableNetPrecalculations.HorForceCalcGenericIterative(bgCrPts, bgTrPts, trLoad, crLoad, sFac, out newTrPts,out crPrestressForces, maxIter);

            List<Point3d> nTrPts = new List<Point3d>();

            for (int i = 0; i < newTrPts.Count; i++)
            {
                nTrPts.Add(GHE.GeometryUtils.Convert(newTrPts[i]));
            }


            DA.SetDataList(0, goals);
            DA.SetDataList(1, nTrPts);
            DA.SetDataList(2, crPrestressForces);
        }


        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("6CA1157A-39E1-492B-9757-6E084390B143"); }
        }
    }
}