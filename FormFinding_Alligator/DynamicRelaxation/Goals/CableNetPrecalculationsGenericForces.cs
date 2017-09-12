using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using FormFinding_Engine.Structural.CableNetFormFinding;
using FormFinding_Engine.Structural.Goals;
using BG = BH.oM.Geometry;
using GHE = Grasshopper_Engine;


namespace FormFinding_Alligator.CableNetDesign
{
    public class CableNetPrecalculationsGenericForces : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CableNetPrecalculations class.
        /// </summary>
        public CableNetPrecalculationsGenericForces()
          : base("PstressGoalGen", "PstressGoal", "Prestress cable goals for cable net form finding", "Alligator", "Structure")
            
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("CR Points", "CRPts", "Compression ring points", GH_ParamAccess.list);
            pManager.AddPointParameter("TR Points", "TRPts", "Tension ring points", GH_ParamAccess.list);
            pManager.AddVectorParameter("LoadVector", "LoadV", "Load Vectors need to be in the same plane as their radial and a Z-vector", GH_ParamAccess.list);
            pManager.AddNumberParameter("Scale Factor", "sfac", "Scalingfactor for the prestressingforces", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PstressGoal", "PstGoal", "ConstantHorizontalPrestressGoal", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            List<Point3d> crPts = new List<Point3d>();
            List<Point3d> trPts = new List<Point3d>();
            List<Vector3d> lvs = new List<Vector3d>();
            double sFac = 0;


            if (!DA.GetDataList(0, crPts)) { return; }
            if (!DA.GetDataList(1, trPts)) { return; }
            if (!DA.GetDataList(2, lvs)) { return; }
            if (!DA.GetData(3, ref sFac)) { return; }


            List<BG.Point> bgCrPts = new List<BG.Point>();
            List<BG.Point> bgTrPts = new List<BG.Point>();
            List<BG.Vector> loadVs = new List<BG.Vector>();
            for (int i = 0; i < crPts.Count; i++)
            {
                BG.Point bgCrPt = GHE.GeometryUtils.Convert(crPts[i]);
                bgCrPts.Add(bgCrPt);

                BG.Point bgTrPt = GHE.GeometryUtils.Convert(trPts[i]);
                bgTrPts.Add(bgTrPt);

                BG.Vector loadV = GHE.GeometryUtils.Convert(lvs[i]);
                loadVs.Add(loadV);
            }


            List<ConstantHorizontalPrestressGoal> goals = CableNetPrecalculations.HorForceCalcGeneric(bgCrPts, bgTrPts, loadVs, sFac);

            DA.SetDataList(0, goals);
        }


        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("3D7F1431-38E9-4AD1-B445-390917EA1445"); }
        }
    }
}