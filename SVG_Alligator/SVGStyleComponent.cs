using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace SVG_Alligator
{
    public class SVGStyleComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the StyleComponent class.
        /// </summary>
        public SVGStyleComponent()
          : base("SVG Style", "SVG Style",
              "Description",
              "Alligator", "SVG")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Thickness", "T", "Thickness", GH_ParamAccess.item);
            pManager.AddColourParameter("Stroke", "S", "Stroke", GH_ParamAccess.item);
            pManager.AddColourParameter("Fill", "F", "Fill", GH_ParamAccess.item);

            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[2].Optional = true;

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Style", "S", "Style", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            System.Drawing.Color strokeCol = new System.Drawing.Color();
            System.Drawing.Color fillCol = new System.Drawing.Color();
            double thickness = 0;

            Dictionary<string, object> StyleData = new Dictionary<string, object>();

            StyleData.Add("Thickness", null);
            StyleData.Add("Stroke", null);
            StyleData.Add("Fill", null);

            if (DA.GetData(0, ref thickness)) { StyleData["Thickness"] = thickness; } ;
            if (DA.GetData(1, ref strokeCol)) { StyleData["Stroke"] = strokeCol; };
            if (DA.GetData(2, ref fillCol)) { StyleData["Fill"] = fillCol; }; 
                                    
            DA.SetData(0, StyleData);

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
            get { return new Guid("de39c7dd-dec4-4568-af37-6454764412c0"); }
        }
    }
}