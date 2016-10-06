using System;
using System.Collections.Generic;
using BHG = BHoM.Geometry;
using Grasshopper.Kernel;
using RHG = Rhino.Geometry;
using GHE = Grasshopper_Engine;

namespace FormFinding_Alligator.DynamicRelaxation.NodeProps
{
    public class UnaryForce : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public UnaryForce() : base("UnaryForce", "U-Force", "Unary force for points", "Alligator", "Formfinding")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddVectorParameter("Forcevectors", "ForceVs", "The Forcevectors that act as loads on the points", GH_ParamAccess.item);
            pManager.AddPointParameter("Points", "Points", "Points on which the unary forces act", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("UFGoal", "Goal", "Unary Force Goal", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            RHG.Vector3d v = RHG.Vector3d.Unset;
            RHG.Point3d pt = RHG.Point3d.Unset;
            
            if (!DA.GetData(0, ref v)) { return; }
            if (!DA.GetData(1, ref pt)) { return; }

            FormFinding_Engine.Structural.Goals.UnaryForce unaryforce = new FormFinding_Engine.Structural.Goals.UnaryForce(GHE.GeometryUtils.Convert(v), GHE.GeometryUtils.Convert(pt));
            
            DA.SetData(0, unaryforce);
        }

     

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("56FCA4E6-F311-4125-A81A-A5DDC3407661"); }
        }
    }
}