using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace SVG_Alligator
{
    public class SVGGroupComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the SVGGroupComponent class.
        /// </summary>
        public SVGGroupComponent()
          : base("SVG Group", "SVG Group",
              "Description",
              "Alligator", "SVG")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("SVG Objects", "O", "SVG Objects", GH_ParamAccess.list);
            pManager.AddTextParameter("Tag", "T", "Tag", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("SVG String", "S", "SVG String", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            
            List<String> Objects = new List<string>();
            string ID = null;

            if (!DA.GetDataList(0, Objects)) { return; }
            if (!DA.GetData(1, ref ID)) { return; }


            string g1 = "<g id=\"_ID\">" + System.Environment.NewLine;

            g1 = g1.Replace("_ID", ID);

            for (int i = 0; i < Objects.Count; i++)
            {
                g1 += Objects[i];
            }


            g1 += "</g>" + System.Environment.NewLine;

            DA.SetData(0, g1);

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
            get { return new Guid("73130dae-bacc-4967-a955-4d5cfc7bb420"); }
        }
    }
}