using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class CreateSVGDocument : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the SVGDocument class.
        /// </summary>

        public CreateSVGDocument()
          : base("SVGDocument", "SVGDocument",
              "Create SVGDocument",
              "Alligator", "SVG")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "SVGObjects", "SVGObjects", "List of SVGObjects", GH_ParamAccess.list);
            pManager.AddParameter(new BHoMObjectParameter(), "SVGStyle", "SVGStyle", "Optional Override SVGStyle", GH_ParamAccess.item);
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "SVGDocument", "SVGDocument", "SVGDocument", GH_ParamAccess.item);
            pManager.AddTextParameter("SVGString", "SVGString", "Returns SVGString", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<SVGObject> svgObjects = new List<SVGObject>(); DA.GetDataList(0, svgObjects);
            SVGStyle svgStyle = null; DA.GetData(1, ref svgStyle);

            List<SVGObject> newSVGObjects = new List<SVGObject>();

            if (svgStyle != null)
            {
                for (int i = 0; i < svgObjects.Count; i++)
                {
                    SVGObject newSVGObject = new SVGObject(svgObjects[i].Geometry, svgStyle);
                    newSVGObjects.Add(newSVGObject);
                }
            }
            else
            {
                newSVGObjects = svgObjects;
            }

            SVGDocument svgDoc = Create.SVGDocument(newSVGObjects);

            DA.SetData(0, svgDoc);
            DA.SetData(1, Transform.ToSVGString(svgDoc));
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
            get { return new Guid("5a7be7c6-c28f-4a00-a28a-ce2d87abea9f"); }
        }
    }
}