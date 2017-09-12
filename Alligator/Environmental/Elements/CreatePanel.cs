//using BH.oM.Environmental;
//using System;
//using Grasshopper.Kernel;
//using System.Collections.Generic;
//using GHE = Grasshopper_Engine;
//using Alligator.Components;
//using BHE = BH.oM.Environmental.Elements;
//using BHG = BH.oM.Geometry;
//using System.Windows.Forms;
//using R = Rhino.Geometry;
//using Grasshopper;
//using GHKT = Grasshopper.Kernel.Types;
//using Grasshopper_Engine.Components;
//using ASP = Alligator.Structural.Properties;

//namespace Alligator.Environmental.Elements
//{
//    public class CreatePanel : GH_Component
//    {
//        /// <summary>
//        /// Initializes a new instance of the MyComponent1 class.
//        /// </summary>
//        public CreatePanel() : base("CreatePanel", "CreatePanel", "Create a Panel", "Alligator", "Environmental")
//        {
//        }

//        /// <summary>
//        /// Registers all the input parameters for this component.
//        /// </summary>
//        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
//        {
//            pManager.AddGenericParameter("Boundary Lines", "BL", "Lines defining the Panel", GH_ParamAccess.list);
//            pManager.AddTextParameter("Name", "N", "Name of the element", GH_ParamAccess.item);
//            pManager.AddGenericParameter("Custom Data", "CD", "Custom data to add to the panel", GH_ParamAccess.item);
//            pManager[1].Optional = true;
//            pManager[2].Optional = true;
//        }

//        /// <summary>
//        /// Registers all the output parameters for this component.
//        /// </summary>
//        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
//        {
//            pManager.AddGenericParameter("Panel", "P", "The Created Panel", GH_ParamAccess.item);
//        }

//        /// <summary>
//        /// This is the method that actually does the work.
//        /// </summary>
//        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
//        protected override void SolveInstance(IGH_DataAccess DA)
//        {

//            BHE.Panel panel = new BHE.Panel();
//            List<BHG.Line> Lines = new List<BHG.Line>();
//            DA.GetDataList<BHG.Line>(0, Lines);
//            panel.Lines = Lines;


//            string name = "";

//            if (DA.GetData(1, ref name))
//            {
//                panel.Name = name;
//            }

//            Dictionary<string, object> customData = GHE.DataUtils.GetData<Dictionary<string, object>>(DA, 2);

//            if (customData != null)
//            {
//                foreach (KeyValuePair<string, object> item in customData)
//                {
//                    panel.CustomData.Add(item.Key, item.Value);
//                }
//            }
//            DA.SetData(0, panel);
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
//            get { return new Guid("{7a792709-e232-4cf7-a209-219c2188eabe}"); }
//        }
//    }
//}