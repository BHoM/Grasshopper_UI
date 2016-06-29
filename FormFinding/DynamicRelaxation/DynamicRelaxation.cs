using System;
using System.Collections.Generic;

using BHoM.Structural;
using Grasshopper.Kernel;
using Rhino.Geometry;

using BHoM_Engine;

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
            pManager.AddGenericParameter("Locked Nodes", "lockedNodes", "Locked nodes", GH_ParamAccess.list);
            pManager.AddNumberParameter("Area", "area", "Cross section area for each bar", GH_ParamAccess.list);
            pManager.AddNumberParameter("Density", "density", "Density for each bar", GH_ParamAccess.list);
            pManager.AddNumberParameter("Young's modulus", "E", "Young's modulus for each bar", GH_ParamAccess.list);
            pManager.AddNumberParameter("Prestress", "prestress", "Prestress for each bar", GH_ParamAccess.list);
            pManager.AddNumberParameter("Nodal load", "nodalLoad", "Nodal load applied to all nodes", GH_ParamAccess.item);
            pManager.AddNumberParameter("Treshold", "treshold", "Max kinetic energy for convergence", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Max number of iterations", "maxItertations", "Maximum number of iterations to run", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Only Z DR", "onlyZ", "Set nodal XY-acceleration to zero without affecting kinectic energy or equilibrium", GH_ParamAccess.item);
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

            if (Utils.Run(DA, 10))
            {

                List<Bar> barsIn = Utils.GetGenericDataList<Bar>(DA, 0);
                List<Node> restrainedNodesIn = Utils.GetGenericDataList<Node>(DA, 1);

                //To clear custom data dictionary, to be changed:
                List<Bar> bars = new List<Bar>();
                foreach (Bar bar in barsIn)
                    bars.Add(new Bar(new Node(bar.StartNode.X, bar.StartNode.Y, bar.StartNode.Z), new Node(bar.EndNode.X, bar.EndNode.Y, bar.EndNode.Z)));
                List<Node> restrainedNodes = new List<Node>();
                foreach (Node node in restrainedNodesIn)
                    restrainedNodes.Add(new Node(node.X, node.Y, node.Z));

                //Inputs
                List<double> areas = Utils.GetDataList<double>(DA, 2);
                List<double> densities = Utils.GetDataList<double>(DA, 3);
                List<double> eModules = Utils.GetDataList<double>(DA, 4);
                List<double> prestresses = Utils.GetDataList<double>(DA, 5);
                double gravity = Utils.GetData<double>(DA, 6);
                double treshold = Utils.GetData<double>(DA, 7);
                int maxNoIt = Utils.GetData<int>(DA, 8);
                bool onlyZ = Utils.GetData<bool>(DA, 9);

                //Set structure
                BHoM_Engine.FormFinding.Structure structure = BHoM_Engine.FormFinding.DynamicRelaxation.SetStructure(bars, restrainedNodes, areas, densities, eModules, prestresses, onlyZ, treshold);

                //Draw lines
                for (int j = 0; j < structure.Bars.Count; j++)
                {
                    if (j < lines.Count)
                        lines[j] = new Line(new Point3d(structure.Bars[j].StartNode.Point.X, structure.Bars[j].StartNode.Point.Y, structure.Bars[j].StartNode.Point.Z), new Point3d(structure.Bars[j].EndNode.Point.X, structure.Bars[j].EndNode.Point.Y, structure.Bars[j].EndNode.Point.Z));
                    else
                        lines.Add(new Line(new Point3d(structure.Bars[j].StartNode.Point.X, structure.Bars[j].StartNode.Point.Y, structure.Bars[j].StartNode.Point.Z), new Point3d(structure.Bars[j].EndNode.Point.X, structure.Bars[j].EndNode.Point.Y, structure.Bars[j].EndNode.Point.Z)));
                }

                //Relax
                int counter = 0;
                for (int i = 0; i < maxNoIt; i++)
                {
                    BHoM_Engine.FormFinding.DynamicRelaxation.RelaxStructure(structure, gravity);

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
                    barForces.Add(new Vector3d(structure.barForceCollection[bar.Name + ":" + structure.t.ToString()].FX, structure.barForceCollection[bar.Name + ":" + structure.t.ToString()].FY, structure.barForceCollection[bar.Name + ":" + structure.t.ToString()].FZ).Length);

                List<Vector3d> nodeForces = new List<Vector3d>();
                foreach (Node node in structure.Nodes)
                    //if ((bool)node.CustomData["isLocked"])
                        nodeForces.Add(new Vector3d(structure.nodalResultCollection[node.Name + ":" + structure.t.ToString()].Force.X, structure.nodalResultCollection[node.Name + ":" + structure.t.ToString()].Force.Y, structure.nodalResultCollection[node.Name + ":" + structure.t.ToString()].Force.Z));

                double kineticEnergy = 0;
                foreach (Node node in structure.Nodes)
                    if (Math.Pow(structure.nodalResultCollection[node.Name + ":" + structure.t.ToString()].Velocity.Length, 2) * (double)node.CustomData["Mass"] / 2.0 > kineticEnergy)
                        kineticEnergy = Math.Pow(structure.nodalResultCollection[node.Name + ":" + structure.t.ToString()].Velocity.Length, 2) * (double)node.CustomData["Mass"] / 2.0;

                DA.SetDataList(0, lines);
                DA.SetDataList(1, barForces);
                DA.SetDataList(2, nodeForces);
                DA.SetData(3, counter);
                DA.SetData(4, kineticEnergy);
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
            get { return FormFinding.Properties.Resources.spider; }
        }
    }
}
