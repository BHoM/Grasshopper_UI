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

namespace BH.UI.Alligator.SVG
{
    public class ToSVG : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>

        public ToSVG()
          : base("SVGString", "SVGString",
              "To SVG-string",
              "Alligator", "SVG")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new BHoMGeometryParameter(), "Geometry", "Geometry", "Rhino Geometry", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("SVGString", "SVGString", "SVGString", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            B.IBHoMGeometry geometry = default(B.IBHoMGeometry);
            DA.GetData(0, ref geometry);
            DA.SetData(0, geometry.IToSVG());
        }

        //B.Circle circle = new B.Circle();
        //DA.GetData(0, ref circle);
        //DA.SetData(0, circle.ToSVG());

        //B.Polyline polyline = new B.Polyline();
        //DA.GetData(0, ref polyline);
        //DA.SetData(0, polyline.ToSVG());

        //B.PolyCurve polycurve = new B.PolyCurve();
        //DA.GetData(0, ref polycurve);
        //DA.SetData(0, polycurve.IToSVG());

        //    B.NurbCurve nurbcurve = new B.NurbCurve();
        //    DA.GetData(0, ref nurbcurve);
        //    DA.SetData(0, nurbcurve.ToSVG());
        //}

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>

        public override Guid ComponentGuid
        {
            get { return new Guid("4399a3c3-5b76-47a2-8114-9bfe412240a8"); }
        }
    }
}
