using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace SVG_Alligator
{
    public class BoundaryComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GetBoundaryComponent class.
        /// </summary>
        public BoundaryComponent()
          : base("Get Boundary", "Get Boundary",
              "Description",
              "Alligator", "SVG")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curves", "C", "Curves", GH_ParamAccess.list);
            
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Boundary", "B", "Boundary", GH_ParamAccess.list);
            pManager.AddBrepParameter("Surfaces", "S", "Surfaces", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Curve> crvList = new List<Curve>();
            if (!DA.GetDataList(0, crvList)) { return; }


            Brep[] bList = Rhino.Geometry.Brep.CreatePlanarBreps(crvList);
            //Rhino.Geometry.Brep.JoinBreps(bList,10);
            List<Curve> cList = new List<Curve>();

            for (int i = 0; i < bList.Length; i++)
            {
                Curve[] boundary = Curve.JoinCurves(bList[i].DuplicateEdgeCurves());

                cList.AddRange(boundary);
            }

            DA.SetDataList(0, cList);
            DA.SetDataList(1, bList);
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
            get { return new Guid("743c7255-8f29-4fb2-98b7-3164f4f10af8"); }
        }
    }
}