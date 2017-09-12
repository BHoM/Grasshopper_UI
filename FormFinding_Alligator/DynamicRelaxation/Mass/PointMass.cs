using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using FormFinding_Engine.Structural;
using GHE = Grasshopper_Engine;
using BH = BH.oM.Geometry;

namespace FormFinding_Alligator.DynamicRelaxation.Mass
{
    public class CreatePointMass : GH_Component
    {
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("63163283-9A53-4859-8B8F-F7B1B294EE7C");
            }
        }

        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public CreatePointMass() : base("PointMass", "P-Mass", "Point Mass", "Alligator", "Formfinding")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Mass", "M", "Mass to apply to the node", GH_ParamAccess.item);
            pManager.AddPointParameter("Points", "P", "Points on which the mass act", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Point Mass", "PM", "Point mass", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double m = 0;
            Point3d p = Point3d.Unset;

            if (!DA.GetData(0, ref m)) { return; }
            if (!DA.GetData(1, ref p)) { return; }

            PointMass pm = new PointMass(GHE.GeometryUtils.Convert(p), m);

            DA.SetData(0, pm);
        }
    }
}
