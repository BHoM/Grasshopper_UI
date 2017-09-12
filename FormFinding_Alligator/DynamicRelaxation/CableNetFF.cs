using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;
using Rhino.Geometry;
using FormFinding_Engine.Structural.CableNetFormFinding;
using FormFinding_Engine.Structural.Goals;
using BG = BH.oM.Geometry;
using GHE = Grasshopper_Engine;


namespace FormFinding_Alligator.CableNetDesign
{
    public class CableNetFF : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CableNetPrecalculations class.
        /// </summary>
        public CableNetFF()
          : base("CableNetFF", "CableNetFF", "Formfinds ", "Alligator", "Structure")

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
            pManager.AddNumberParameter("Min Height", "Min Height", "Minimal allowed height of the tension ring", GH_ParamAccess.item);
            pManager.AddNumberParameter("Scale Factor", "sfac", "Scalingfactor for the prestressingforces", GH_ParamAccess.item);
            pManager.AddNumberParameter("Initial Step", "initStep", "First step used when searching for scalefactor in outer formfinding loop", GH_ParamAccess.item, 0.001);
            pManager.AddNumberParameter("dt", "dt", "timestep for relaxation", GH_ParamAccess.item);
            pManager.AddNumberParameter("thresh", "thresh", "Threshold limit for relaxiation", GH_ParamAccess.item);
            pManager.AddNumberParameter("Damping", "damp", "Damping for formfinding", GH_ParamAccess.item);
            pManager.AddIntegerParameter("MaxIter", "MaxIter", "Max iteration for formfinding", GH_ParamAccess.item);
            
        }


        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Cable forces", "Cable forces", "Cable forces", GH_ParamAccess.list);
            pManager.AddNumberParameter("Cr forces", "Cr forces", "Cr forces", GH_ParamAccess.list);
            pManager.AddPointParameter("FF Pts", "ffPts", "Form found pts", GH_ParamAccess.list);
            pManager.AddNumberParameter("Final Sfac", "sfac", "Final scale factor used in outer loop of formfinding", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            List<Point3d> crPts = new List<Point3d>();
            List<Point3d> trPts = new List<Point3d>();
            List<Vector3d> trForce = new List<Vector3d>();
            List<Vector3d> crForce = new List<Vector3d>();
            double minHeight = 0;
            double sFac = 0;
            double initStep = 0;
            int maxIter = 1000;

            double dt = 0;
            double treshold = 0;
            double damping = 0;
            int maxiterations = 0;




            if (!DA.GetDataList(0, crPts)) { return; }
            if (!DA.GetDataList(1, trPts)) { return; }
            if (!DA.GetDataList(2, trForce)) { return; }
            if (!DA.GetDataList(3, crForce)) { return; }
            if (!DA.GetData(4, ref minHeight)) { return; }
            if (!DA.GetData(5, ref sFac)) { return; }
            if (!DA.GetData(6, ref initStep)) { return; }
            if (!DA.GetData(7, ref dt)) { return; }
            if (!DA.GetData(8, ref treshold)) { return; }
            if (!DA.GetData(9, ref damping)) { return; }
            if (!DA.GetData(10, ref maxiterations)) { return; }


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

            List<double> cablePrestressForce;
            List<double> crPrestressForce;
            List<BG.Point> bgFFPts;

            double finalSfac = 0;

            CableNetFormFinding.Run(bgCrPts, bgTrPts, trLoad, crLoad, minHeight, sFac, initStep, dt, treshold, damping, maxiterations, out crPrestressForce, out cablePrestressForce, out bgFFPts, out finalSfac);

            DA.SetDataList(0, cablePrestressForce);
            DA.SetDataList(1, crPrestressForce);
            DA.SetDataList(2, bgFFPts.Select(x => GHE.GeometryUtils.Convert(x)).ToList());
            DA.SetData(3, finalSfac);
        }


        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("BEE0F9B0-56B4-4CEB-BAAB-3567F13AE817");
            }
        }
    }
}