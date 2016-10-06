using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FormFinding_Engine.Structural;
using GHE = Grasshopper_Engine;

namespace FormFinding_Alligator.DynamicRelaxation
{
    public class Spring : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public Spring() : base("Spring", "Spring", "Spring Goal", "Alligator", "Formfinding")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddLineParameter("Lines", "Lines", "Lines that will represent strings", GH_ParamAccess.item);
            pManager.AddNumberParameter("stiffness", "Ks", "stiffnesses of the springs", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SpringGoal", "Goal", "Spring Goal", GH_ParamAccess.item);
            
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Line line = Line.Unset;
            double ks = 0;

            if (!DA.GetData(0, ref line)) { return; }
            if (!DA.GetData(1, ref ks)) { return; }

            FormFinding_Engine.Structural.Goals.Spring spring = new FormFinding_Engine.Structural.Goals.Spring(GHE.GeometryUtils.Convert(line), ks);

            DA.SetData(0, spring);
        }

        
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("EAC86434-62C3-4ECD-8CF4-3EEF88ACFE1B"); }
        }
    }
}