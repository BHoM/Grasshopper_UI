using System;
using System.Collections.Generic;
using BHoM.Structural.Elements;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper_Engine;

namespace Alligator.Components
{
    public class DynamicRelaxation : GH_Component
    {
       
        public DynamicRelaxation() : base("DynamicRelaxation", "DynamicRelaxation", "Dynamically relaxes structure", "Alligator", "FormFinding") { }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Bars", "bars", "Bars of structure", GH_ParamAccess.list);
            pManager.AddGenericParameter("Locked Nodes", "lockedNodes", "Nodes with restraints", GH_ParamAccess.list);
            pManager.AddNumberParameter("Area", "area", "Cross section area for each bar", GH_ParamAccess.list);
            pManager.AddNumberParameter("Prestress", "prestress", "Prestress for each bar", GH_ParamAccess.list);
            pManager.AddNumberParameter("Nodal load", "nodalLoad", "Nodal load applied to all nodes", GH_ParamAccess.item);
            pManager.AddNumberParameter("TimeStep", "timeStep", "Time step for each iteration", GH_ParamAccess.item);
            pManager.AddNumberParameter("Velocity damping", "velocityDamping", "Damping applied to velocities", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Max number of iterations", "maxItertations", "Maximum number of iterations to run", GH_ParamAccess.item);
            pManager.AddNumberParameter("Treshold", "treshold", "Max kinetic energy for convergence", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Use mass damping", "useMassDamping", "Set fictional nodal masses based on bar stiffnesses to stabilize relaxation. Does not affect forces", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Calc safe time step", "calcSafeTimeStep", "Calculate safe time step based on bar stiffnesses and node masses", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Run", "run", "Run if true", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {

            pManager.Register_LineParam("Relaxed Lines", "lines", "Relaxed lines", GH_ParamAccess.list);
            pManager.Register_DoubleParam("Axial Bar Forces", "barForces", "Axial force in bars", GH_ParamAccess.list);
            pManager.Register_VectorParam("Node Forces", "nodeForces", "Node forces", GH_ParamAccess.list);
            pManager.Register_IntegerParam("iterations", "iterations", "Number of iterations run", GH_ParamAccess.item);
            pManager.Register_DoubleParam("KineticEnegry", "kineticEngergy", "Maximal kinetic energy at latest timestep", GH_ParamAccess.item);
        }

        protected override void BeforeSolveInstance()
        {
            if (lines != null)
                lines.Clear();

            lines = new List<Rhino.Geometry.Line>();
            _clippingBox = Rhino.Geometry.BoundingBox.Empty;
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Rhino.Geometry.BoundingBox box = new Rhino.Geometry.BoundingBox(-100, -100, -100, 100, 100, 100);
            _clippingBox.Union(box);

            if (DataUtils.Run(DA, 11))
            {
                List<Bar> bars = DataUtils.GetGenericDataList<Bar>(DA, 0);
                List<Node> lockedNodes = DataUtils.GetGenericDataList<Node>(DA, 1);
                List<double> areas = DataUtils.GetDataList<double>(DA, 2);
                List<double> prestresses = DataUtils.GetDataList<double>(DA, 3);
                double unaryNodalLoad = DataUtils.GetData<double>(DA, 4);
                double dt = DataUtils.GetData<double>(DA, 5);
                double c = DataUtils.GetData<double>(DA, 6);
                int maxNoIt = DataUtils.GetData<int>(DA, 7);
                double treshold = DataUtils.GetData<double>(DA, 8);
                bool useMassDamping = DataUtils.GetData<bool>(DA, 9);
                bool calcSafeTimeStep = DataUtils.GetData<bool>(DA, 10);



                FormFinding_Engine.Structure structure = FormFinding_Engine.DynamicRelaxation.SetStructure(bars, lockedNodes, areas, prestresses, dt, c, useMassDamping, calcSafeTimeStep, treshold);

                int counter = 0;
                for (int i = 0; i < maxNoIt; i++)
                {
                    FormFinding_Engine.DynamicRelaxation.RelaxStructure(structure, unaryNodalLoad, useMassDamping);

                    //Draw lines
                    for (int j = 0; j < structure.Bars.Count; j++)
                    {
                        if (j < lines.Count)
                            lines[j] = new Line(new Point3d(structure.Bars[j].StartNode.Point.X, structure.Bars[j].StartNode.Point.Y, structure.Bars[j].StartNode.Point.Z), new Point3d(structure.Bars[j].EndNode.Point.X, structure.Bars[j].EndNode.Point.Y, structure.Bars[j].EndNode.Point.Z));
                        else
                            lines.Add(new Line(new Point3d(structure.Bars[j].StartNode.Point.X, structure.Bars[j].StartNode.Point.Y, structure.Bars[j].StartNode.Point.Z), new Point3d(structure.Bars[j].EndNode.Point.X, structure.Bars[j].EndNode.Point.Y, structure.Bars[j].EndNode.Point.Z)));
                    }
                    Rhino.RhinoDoc.ActiveDoc.Views.Redraw();


                    if (structure.HasConverged())
                        break;
                   else
                        counter++;
                }

                List<double> barForces = new List<double>();
                foreach (Bar bar in structure.Bars)
                    barForces.Add(new Vector3d(structure.barForceCollection[bar.Name + ":" + structure.m_t.ToString()].FX, structure.barForceCollection[bar.Name + ":" + structure.m_t.ToString()].FY, structure.barForceCollection[bar.Name + ":" + structure.m_t.ToString()].FZ).Length);

                List<Vector3d> nodeForces = new List<Vector3d>();
                foreach (Node node in structure.Nodes)
                    nodeForces.Add(new Vector3d(structure.nodalResultCollection[node.Name + ":" + structure.m_t.ToString()].Force.X, structure.nodalResultCollection[node.Name + ":" + structure.m_t.ToString()].Force.Y, structure.nodalResultCollection[node.Name + ":" + structure.m_t.ToString()].Force.Z));

                structure.Bars.Clear();
                structure.Nodes.Clear();

                DA.SetDataList(0, lines);
                DA.SetDataList(1, barForces);
                DA.SetDataList(2, nodeForces);
                DA.SetData(3, counter);
                DA.SetData(4, structure.m_kineticEnergy[structure.m_t.ToString()]);
            }
        }

        #region display
        private List<Rhino.Geometry.Line> lines;
        private Rhino.Geometry.BoundingBox _clippingBox;

        public override Rhino.Geometry.BoundingBox ClippingBox
        {
            get
            {
                Rhino.Geometry.BoundingBox box = base.ClippingBox;
                box.Union(_clippingBox);
                return box;
            }
        }

        public override void DrawViewportWires(IGH_PreviewArgs args)
        {
            base.DrawViewportWires(args);

            if (lines == null)
                return;

            if (lines.Count > 0)
                foreach (Rhino.Geometry.Line ln in lines)
                {
                    args.Display.DrawLine(ln, System.Drawing.Color.Orange, 2);
                }

        }
        #endregion

        public override Guid ComponentGuid
        {
            get { return new Guid("ed69838f-bec3-4976-a183-f075f48859b8"); }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return FormFinding_Alligator.Properties.Resources.spider; }
        }
    }
}
