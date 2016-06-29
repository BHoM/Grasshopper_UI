using System;
using System.Drawing;
using System.Collections.Generic;
using BHoM_Engine.ModelLaundry;
using BHoM.Geometry;
using Grasshopper.Kernel;
using Rhino.Geometry;
using BH = BHoM.Geometry;
using R = Rhino.Geometry;
namespace Alligator.ModelLaundry
{
    public class FilterByBoundingBox : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the FilterByBoundingBox class.
        /// </summary>
        public FilterByBoundingBox()
          : base("FilterByBoundingBox", "FiltByBBox", "Filter the elements inside a set of boundingboxes", "Alligator", "ModelLaundry"){}

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("GeometrytoFilter", "GeomtoFilt", "Insert a set of geometry to be filtred", GH_ParamAccess.list);
            pManager.AddGenericParameter("BoundingboxtoFilterBy", "BBox", "Insert a set of boundingboxes to filter geometry", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FilteredGeometry", "FilteredGeom", "Geometrys inside the boundingbox", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<object> geomToFilt = new List<object>();
            List<object> geomtoFiltBy = new List<object>();
            List<object> output = new List<object>();
            List<BHoM.Geometry.GeometryBase> insiders = new List<BHoM.Geometry.GeometryBase>();
            List<BHoM.Geometry.GeometryBase> elements = new List<BH.GeometryBase>();
            List<BHoM.Geometry.BoundingBox> boxes = new List<BH.BoundingBox>();

            if (!DA.GetDataList(0, geomToFilt)) return;
            if (!DA.GetDataList(1, geomtoFiltBy)) return;

            foreach (object o in geomToFilt)
            {
                elements.Add((BHoM.Geometry.GeometryBase)o);
            }

            foreach (object o in geomtoFiltBy)
            {
                boxes.Add((BHoM.Geometry.BoundingBox)o);
            }

            insiders = Util.FilterByBoundingBox(elements, boxes);

            foreach (BHoM.Geometry.GeometryBase i in insiders)
            {
                output.Add((object)i);
            }


            DA.SetDataList(0, output);


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
            get { return new Guid("{9f0cc5bf-3afd-41d4-b1f2-e6a1941958c0}"); }
        }
    }
}