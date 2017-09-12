using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using FormFinding_Engine.Structural;
using GHE = Grasshopper_Engine;
using BH =BH.oM.Geometry;

namespace FormFinding_Alligator.DynamicRelaxation.NodeProps
{
    public class Constraints : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public Constraints() : base("Constrained Nodes", "Constraints", "Constrained nodes will not move", "Alligator", "Formfinding")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Pinnedpoints", "PinnPts", "The ponts that represent the pinned nodes", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Constraints", "Constraints", "Constraints", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Point3d> pts = new List<Point3d>();

            if (!DA.GetDataList(0, pts)) { return; }


            List<BH.Point> bhPts = new List<BH.oM.Geometry.Point>();

            foreach (Point3d pt in pts)
            {
                bhPts.Add(GHE.GeometryUtils.Convert(pt));
            }

            NodeConstraint con = new NodeConstraint(bhPts);

            DA.SetData(0, con);
        }


        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("8C5EDB41-CA3A-4650-A130-6F9765C4D09A"); }
        }
    }

}