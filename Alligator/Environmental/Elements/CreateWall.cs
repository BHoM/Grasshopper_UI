//using BHoM.Environmental;
//using System;
//using Grasshopper.Kernel;
//using System.Collections.Generic;
//using GHE = Grasshopper_Engine;
//using Alligator.Components;
//using BHE = BHoM.Environmental.Elements;
//using BHG = BHoM.Geometry;
//using System.Windows.Forms;
//using R = Rhino.Geometry;
//using Grasshopper;
//using GHKT = Grasshopper.Kernel.Types;
//using Grasshopper_Engine.Components;
//using ASP = Alligator.Structural.Properties;

//namespace Alligator.Environmental.Elements
//{
//    public class CreateWall : GH_Component
//    {
//        /// <summary>
//        /// Initializes a new instance of the MyComponent1 class.
//        /// </summary>
//        public CreateWall() : base("CreateWall", "CreateWall", "Create a Wall", "Alligator", "Environmental")
//        {
//        }

//        /// <summary>
//        /// Registers all the input parameters for this component.
//        /// </summary>
//        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
//        {
//            pManager.AddGenericParameter("Boundary Line", "BL", "Line defining the Wall", GH_ParamAccess.item);
//            pManager.AddGenericParameter("Panels", "Panels", "Panels inside the Wall", GH_ParamAccess.list);
//            pManager.AddTextParameter("Name", "N", "Name of the element", GH_ParamAccess.item);
//            pManager.AddGenericParameter("Custom Data", "CD", "Custom data to add to the wall", GH_ParamAccess.item);

//            pManager[1].Optional = true;
//            pManager[2].Optional = true;
//            pManager[3].Optional = true;
//        }

//        /// <summary>
//        /// Registers all the output parameters for this component.
//        /// </summary>
//        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
//        {
//            pManager.AddGenericParameter("Wall", "W", "The Created Wall", GH_ParamAccess.item);
//        }

//        /// <summary>
//        /// This is the method that actually does the work.
//        /// </summary>
//        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
//        protected override void SolveInstance(IGH_DataAccess DA)
//        {

//            BHE.Wall wall = new BHE.Wall();

//            BHG.Line Line = null;
//            DA.GetData< BHG.Line > (0, ref Line);
//            wall.Line = Line;

//            List<BHE.Panel> panels = new List<BHE.Panel>();
//            DA.GetData<List<BHE.Panel>>(1, ref panels);
//            wall.Panels = panels;

//            string name = "";

//            if (DA.GetData(1, ref name))
//            {
//                wall.Name = name;
//            }

//            Dictionary<string, object> customData = GHE.DataUtils.GetData<Dictionary<string, object>>(DA, 2);

//            if (customData != null)
//            {
//                foreach (KeyValuePair<string, object> item in customData)
//                {
//                    wall.CustomData.Add(item.Key, item.Value);
//                }
//            }

//            DA.SetData(0, wall);
//        }

//        /// <summary>
//        /// Provides an Icon for the component.
//        /// </summary>
//        protected override System.Drawing.Bitmap Icon
//        {
//            get
//            {
//                //You can add image files to your project resources and access them like this:
//                // return Resources.IconForThisComponent;
//                return null;
//            }
//        }

//        /// <summary>
//        /// Gets the unique ID for this component. Do not change this ID after release.
//        /// </summary>
//        public override Guid ComponentGuid
//        {
//            get { return new Guid("{df8f20ab-72e1-4247-ac0b-fdcff94d7124}"); }
//        }
//    }
//}