using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FormFinding_Engine.Base;
using Grasshopper.Kernel.Data;

using FormFinding_Engine.Structural;
using GHE = Grasshopper_Engine;

namespace FormFinding_Alligator.DynamicRelaxation
{
    public class CableNetFormFinder : GH_Component
    {

        private List<Point3d> m_rPts;
        private int m_iterations;
        List<double> m_results = new List<double>();


        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public CableNetFormFinder() : base("Structural_DR", "Structural_DR", "Dynamic Relaxation solver for simple spring systems", "Alligator", "Formfinding")
        {
            m_rPts = new List<Point3d>();
            m_iterations = 0;
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Goals", "Goals", "All goals that do something on the nodes", GH_ParamAccess.list);
            pManager.AddGenericParameter("Constraints", "Constraints", "Constraints (Like Pinned Points", GH_ParamAccess.list);
            pManager.AddGenericParameter("Node Masses", "Mass", "Masses for ALL!! nodes", GH_ParamAccess.list);
            pManager.AddNumberParameter("Timestep", "dt", "Timestep, keep it small...", GH_ParamAccess.item, 0.01);
            pManager.AddNumberParameter("Treshold", "Treshold", "solver stops when average displacement is smaller than treshold", GH_ParamAccess.item, 0.01);
            pManager.AddNumberParameter("Damping", "dmp", "Damping of the stuff, slowing it down", GH_ParamAccess.item);
            pManager.AddNumberParameter("maxiterations", "mi", "Maximum allowed iterations", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Go", "Go", "Go", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geometry", "Geometry", "Geometrical representation of the results", GH_ParamAccess.list);
            pManager.AddNumberParameter("Iterations", "Iterations", "Iterations", GH_ParamAccess.item);
            pManager.AddNumberParameter("Goal forces", "Forces", "A list of all the forces in all the goals", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (GHE.DataUtils.Run(DA, 7))
            {
                double dt = 0;
                DA.GetData(3, ref dt);
                double treshold = 0;
                DA.GetData(4, ref treshold);
                double damping = 0;
                DA.GetData(5, ref damping);
                double maxiterations = 0;
                DA.GetData(6, ref maxiterations);

                StructuralDynamicRelaxation relaxEngine = new StructuralDynamicRelaxation(dt, treshold, damping, maxiterations);
                relaxEngine.ResultCallback += ProcessResults;

                List<IRelaxPositionGoal> goals = GHE.DataUtils.GetGenericDataList<IRelaxPositionGoal>(DA, 0);
                List<IRelaxPositionBC> bcs = GHE.DataUtils.GetGenericDataList<IRelaxPositionBC>(DA, 1);
                List<IRelaxMass> masses = GHE.DataUtils.GetGenericDataList<IRelaxMass>(DA, 2);

                relaxEngine.AddGoals(goals);
                relaxEngine.AddBCs(bcs);
                relaxEngine.AddMasses(masses);

                relaxEngine.Run();
                

                m_results = new List<double>();

                foreach (IRelaxGoal goal in goals)
                {
                    m_results.Add(goal.Result()[0]);
                   
                }

                List<BH.oM.Geometry.Point> bhPts = relaxEngine.GetPoints();

                m_rPts = new List<Point3d>();

                foreach (BH.oM.Geometry.Point bhP in bhPts)
                {
                    m_rPts.Add(GHE.GeometryUtils.Convert(bhP));
                }

                m_iterations = relaxEngine.Iterations();

            }

            DA.SetDataList(0, m_rPts);
            DA.SetData(1, m_iterations);
            DA.SetDataList(2, m_results);
        }


        public void ProcessResults(List<RelaxNode> nodes)
        {
            m_rPts = new List<Point3d>();

            foreach (RelaxNode node in nodes)
            {
                double[] pos = node.NewPosition();
                m_rPts.Add(new Point3d(pos[0], pos[1], pos[2]));
            }

            Rhino.RhinoDoc.ActiveDoc.Views.Redraw();

        }

        public override void DrawViewportWires(IGH_PreviewArgs args)
        {

            foreach (Point3d pt in m_rPts)
            {
                args.Display.DrawPoint(pt, System.Drawing.Color.Aqua);
            }

            base.DrawViewportWires(args);
        }


        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("884701CD-DBC5-4AC5-9A5F-9F89B89CEACD"); }
        }
    }
}