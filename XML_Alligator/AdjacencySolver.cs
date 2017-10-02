using System;
using System.IO;
using System.Collections.Generic;
using Grasshopper.Kernel;
using XML_Adapter.gbXML;
using BHoM;
using BHE = BHoM.Environmental.Elements;

namespace XML_Alligator
{
    public class AdjacencySolver : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public AdjacencySolver()
          : base("Adjacency Solver", "AS",
              "Solves adjacencies with environmental Spaces and Panels",
              "Alligator", "Environmental")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("BHoM Panels", "P", "Panels to solve adjacencies for.", GH_ParamAccess.list);
            pManager.AddGenericParameter("BHoM Spaces", "S", "Spaces to use as adjacent", GH_ParamAccess.list);
            pManager.AddBooleanParameter("RUN", "RUN", "RUN", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("BHoM Panels", "P", "Panels with solved adjacencies.", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<BHE.Panel> pans = new List<BHE.Panel>(); DA.GetDataList<BHE.Panel>(0, pans);
            List<BHE.Space> spaces = new List<BHE.Space>(); DA.GetDataList<BHE.Space>(1, spaces);
            bool active = false; DA.GetData<bool>(2, ref active);

            if (!active) return;

            List<BHE.Panel> pansout = XML_Adapter.gbXML.AdjSolver.AdjacensiesSolver(pans, spaces);

            DA.SetDataList(0, pansout);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{86abdecd-4b9d-46bd-af8f-0a508929ceb3}"); }
        }
    }
}