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
    public class CreateSVGObject : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the SVGObject class.
        /// </summary>
        
        public CreateSVGObject()
          : base("SVGObject", "SVGObject",
              "Create SVGObject",
              "Alligator", "SVG")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new BHoMGeometryParameter(), "Geometry", "Geometry", "Rhino Geometry", GH_ParamAccess.list);
            pManager.AddParameter(new BHoMObjectParameter(), "SVGStyle", "SVGStyle", "Optional SVGStyle", GH_ParamAccess.item);
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "SVGObject", "SVGObject", "SVGObject", GH_ParamAccess.item);
            pManager.AddTextParameter("SVGString", "SVGString", "Returns string in SVG format", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<B.IBHoMGeometry> geometry = new List<B.IBHoMGeometry>(); DA.GetDataList(0, geometry);
            SVGStyle style = new SVGStyle(); DA.GetData(1, ref style);
            SVGObject obj = new SVGObject(geometry, style);
            DA.SetData(0, obj);
            DA.SetData(1, Transform.ToSVGString(obj));
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
            get { return new Guid("84fcde5e-89d7-462c-9daf-87ccba3d6d73"); }
        }
    }
}