using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using FormFinding_Engine.Base;
using FormFinding_Engine.Structural;
using FormFinding_Engine.Structural.Goals;
using Rhino.Geometry;
using GHE = Grasshopper_Engine;

namespace FormFinding_Alligator
{
    public class StructuralDRDynamic : GH_Component
    {
        private List<Point3d> m_rPts;
        private int m_iterations;
        List<double> m_results;
        private StructuralDynamicRelaxation m_relaxEngine;
        private Action RunAction;
        private Task m_task;
        private CancellationTokenSource m_tokenSource;

        bool m_updateOutputsOnly;

        public StructuralDRDynamic() : base("Structural Dynamic DR", "Structural Dynamic DR", "Dynamic Relaxation ", "Alligator", "Formfinding")
        {
            m_rPts = new List<Point3d>();
            m_iterations = 0;
            m_results = new List<double>();
            m_updateOutputsOnly = false;
            m_tokenSource = new CancellationTokenSource();
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("4E1CF153-D7D2-45DD-8D2C-92C6264B282E");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
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

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geometry", "Geometry", "Geometrical representation of the results", GH_ParamAccess.list);
            pManager.AddNumberParameter("Iterations", "Iterations", "Iterations", GH_ParamAccess.item);
            pManager.AddNumberParameter("Goal forces", "Forces", "A list of all the forces in all the goals", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {


            if (!m_updateOutputsOnly && GHE.DataUtils.Run(DA, 7))
            {
                m_tokenSource.Cancel();

                m_tokenSource.Dispose();

                m_tokenSource = new CancellationTokenSource();

                double dt = 0;
                DA.GetData(3, ref dt);
                double treshold = 0;
                DA.GetData(4, ref treshold);
                double damping = 0;
                DA.GetData(5, ref damping);
                double maxiterations = 0;
                DA.GetData(6, ref maxiterations);

                m_relaxEngine = new StructuralDynamicRelaxation(dt, treshold, damping, maxiterations);
                m_relaxEngine.ResultCallback += UpdateResults;
                RunAction += m_relaxEngine.Run;

                List<IRelaxPositionGoal> goals = GHE.DataUtils.GetGenericDataList<IRelaxPositionGoal>(DA, 0);
                List<IRelaxPositionBC> bcs = GHE.DataUtils.GetGenericDataList<IRelaxPositionBC>(DA, 1);
                List<IRelaxMass> masses = GHE.DataUtils.GetGenericDataList<IRelaxMass>(DA, 2);

                m_relaxEngine.AddGoals(goals);
                m_relaxEngine.AddBCs(bcs);
                m_relaxEngine.AddMasses(masses);


                //Task t = Task.Run(RunAction);
                m_task = Task.Factory.StartNew(RunAction, m_tokenSource.Token);
            }

            //lock (lockObj)
            //{
                DA.SetDataList(0, m_rPts);
                DA.SetData(1, m_iterations);
                DA.SetDataList(2, m_results);
            //}
            m_updateOutputsOnly = false;
        }


        object lockObj = new object();

        public void UpdateResults(List<RelaxNode> nodes)
        {
            //lock (lockObj)
            //{
                m_rPts = new List<Point3d>();

                foreach (RelaxNode node in nodes)
                {
                    double[] pos = node.NewPosition();
                    m_rPts.Add(new Point3d(pos[0], pos[1], pos[2]));
                }

                m_results = new List<double>();

                foreach (IRelaxGoal goal in m_relaxEngine.Goals)
                {
                    m_results.Add(goal.Result()[0]);

                }

                m_iterations = m_relaxEngine.Iterations();

                m_updateOutputsOnly = true;
                ExpireSolution(true);
            //}
        }
    }


}
