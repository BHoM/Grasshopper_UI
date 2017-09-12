//using System;
//using System.Collections.Generic;
//using Grasshopper.Kernel;
//using RG = Rhino.Geometry;
//using FormFinding_Engine.Structural;
//using GHE = Grasshopper_Engine;
//using BG = BHoM.Geometry;

//namespace FormFinding_Alligator.DynamicRelaxation
//{
//    public class RotationalSpring : GH_Component
//    {
//        /// <summary>
//        /// Initializes a new instance of the MyComponent1 class.
//        /// </summary>
//        public RotationalSpring() : base("RotationalSpring", "RS", "Rotational Spring Goal angle between 3 points", "Alligator", "Formfinding")
//        {
//        }

//        /// <summary>
//        /// Registers all the input parameters for this component.
//        /// </summary>
//        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
//        {
//            pManager.AddPointParameter("Point_0", "Pt0", "Startpoint", GH_ParamAccess.item);
//            pManager.AddPointParameter("Point_1", "Pt1", "Midpoint", GH_ParamAccess.item);
//            pManager.AddPointParameter("Point_2", "Pt2", "Endpoint", GH_ParamAccess.item);
//            pManager.AddNumberParameter("RotationalStiffness", "RKs", "Rotational stiffnesses of the springs", GH_ParamAccess.item);
//        }

//        /// <summary>
//        /// Registers all the output parameters for this component.
//        /// </summary>
//        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
//        {
//            pManager.AddGenericParameter("SpringGoal", "Goal", "Spring Goal", GH_ParamAccess.item);

//        }

//        /// <summary>
//        /// This is the method that actually does the work.
//        /// </summary>
//        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
//        protected override void SolveInstance(IGH_DataAccess DA)
//        {
//           RG.Point3d pt0 = RG.Point3d.Unset;
//           RG.Point3d pt1 = RG.Point3d.Unset;
//           RG.Point3d pt2 = RG.Point3d.Unset;

//            double rks = 0;

//            if (!DA.GetData(0, ref pt0)) { return; }
//            if (!DA.GetData(1, ref pt1)) { return; }
//            if (!DA.GetData(2, ref pt2)) { return; }
//            if (!DA.GetData(3, ref rks)) { return; }

//            BG.Point BHpt0 = GHE.GeometryUtils.Convert(pt0);
//            BG.Point BHpt1 = GHE.GeometryUtils.Convert(pt1);
//            BG.Point BHpt2 = GHE.GeometryUtils.Convert(pt2);

//            FormFinding_Engine.Structural.Goals.RotationalSpring rotationalSpring = new FormFinding_Engine.Structural.Goals.RotationalSpring(BHpt0, BHpt1, BHpt2, rks);
           

//            DA.SetData(0, rotationalSpring);
//        }


//        /// <summary>
//        /// Gets the unique ID for this component. Do not change this ID after release.
//        /// </summary>
//        public override Guid ComponentGuid
//        {
//            get { return new Guid("B4C7AA5D-464A-4B50-A080-02A15CD73F1F"); }
//        }
//    }
//}