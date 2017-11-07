using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using BH.oM.Base;
using B = BH.oM.Geometry;
using BH.Engine.Graphics;
using BH.UI.Alligator.Base;
using BH.oM.Graphics;

namespace BH.UI.Alligator.SVG
{
    public class CreateSVGStyle : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the SVGStyle class.
        /// </summary>

        public CreateSVGStyle()
          : base("SVGStyle", "SVGStyle",
              "Create SVGStyle",
              "Alligator", "SVG")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Stroke Width", "StrokeWidth", "double", GH_ParamAccess.item, 1);
            pManager.AddTextParameter("Stroke Color", "StrokeColor", "string", GH_ParamAccess.item, "black");
            pManager.AddTextParameter("Fill Color", "FillColor", "string", GH_ParamAccess.item, "none");
            pManager.AddNumberParameter("Stroke Opacity", "StrokeOpac", "double: 0=transparent, 1=completely filled", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("Fill Opacity", "FillOpac", "double: 0=transparent, 1=completely filled", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("Stroke DashArray", "StrokeDash", "list of doubles", GH_ParamAccess.list, new List<double>() { 0 });

            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
            pManager[4].Optional = true;
            pManager[5].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "SVGStyle", "SVGStyle", "SVGStyle", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double strokeWidth = 0; DA.GetData(0, ref strokeWidth);
            string strokeColor = ""; DA.GetData(1, ref strokeColor);
            string fillColor = ""; DA.GetData(2, ref fillColor);
            double strokeOpacity = 1; DA.GetData(3, ref strokeOpacity);
            double fillOpacity = 1; DA.GetData(4, ref fillOpacity);
            List<double> strokeDash = new List<double>(); DA.GetDataList(5, strokeDash);

            DA.SetData(0, new SVGStyle(strokeWidth, strokeColor, fillColor, strokeOpacity, fillOpacity, strokeDash));
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>

        //protected override System.Drawing.Bitmap Icon
        //{
        //    get
        //    {
        //        //You can add image files to your project resources and access them like this:
        //        // return Resources.IconForThisComponent;
        //        return null;
        //    }
        //}

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>

        public override Guid ComponentGuid
        {
            get { return new Guid("b9fa48aa-f8dc-44ff-b8a0-1102536af5a3"); }
        }
    }
}