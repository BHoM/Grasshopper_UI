using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using ModelLaundry_Engine;
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
          : base("FilterByBoundingBox", "FiltByBBox", "Filter the elements inside a set of boundingboxes", "Alligator", "ModelLaundry") {}

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("GeometrytoFilter", "GeomtoFilt", "Insert a set of geometry to be filtred", GH_ParamAccess.list);
            pManager.AddBoxParameter("BoundingboxtoFilterBy", "BBox", "Insert a set of boundingboxes to filter geometry", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Insiders", "Inside", "Geometrys inside the boundingbox", GH_ParamAccess.list);
            pManager.AddGenericParameter("Outsiders", "Outside", "Geometrys outside the boundingbox", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Get GH inputs
            List<object> elements = Utils.GetGenericDataList<object>(DA, 0);
            List<Box> boxes = new List<Box>();
            if (!DA.GetDataList(1, boxes)) return;

            // Convert boxes to BHoM bounding boxes
            List<BH.BoundingBox> bhomBoxes = new List<BHoM.Geometry.BoundingBox>();
            for (int i = 0; i < boxes.Count; i++)
            {
                R.Point3d[] cornerPt = boxes[i].GetCorners();
                List<BH.Point> bhomBoxCornerPt = new List<BH.Point>();
                for (int j = 0; j < 8; j++)
                {
                    bhomBoxCornerPt.Add(new BH.Point(cornerPt[j].X, cornerPt[j].Y, cornerPt[j].Z));
                }
                bhomBoxes.Add(new BH.BoundingBox(bhomBoxCornerPt));
            }

            // Separate the objects between insiders and outsiders
            List<object> insiders = new List<object>();
            List<object> outsiders = new List<object>();
            foreach (object element in elements)
            {
                if (element is BHoM.Global.BHoMObject && Util.IsInside(((BHoM.Global.BHoMObject)element).GetGeometry(), bhomBoxes))
                    insiders.Add(element);
                else if (element is BH.GeometryBase && Util.IsInside((BH.GeometryBase)element, bhomBoxes))
                    insiders.Add(element);
                else
                    outsiders.Add(element);
            }

            /*List<BH.GeometryBase> geometries = new List<BH.GeometryBase>();
            for (int i = 0; i < elements.Count; i++)
            {
                BH.GeometryBase geometry = elements[i].GetGeometry();
                if (geometry is BH.Curve)       // TODO - This case is only here to go around a bug in the bounding  box for polycurve
                {
                    BH.Curve tempCrv = geometry as BH.Curve;
                    Group<BH.Curve> crvs = new Group<BH.Curve>();
                    crvs = new Group<BHoM.Geometry.Curve>(tempCrv.Explode());
                    geometries.Add(crvs);
                }

                else
                {
                    geometries.Add(geometry);
                }

            }

            List<BHoM.Global.BHoMObject> outsiders = new List<BHoM.Global.BHoMObject>();
            List<BH.GeometryBase> insiders = Util.FilterByBoundingBox(geometries, bhomBoxes, out outsiders);*/

            // Set GH outputs
            DA.SetDataList(0, insiders);
            DA.SetDataList(1, outsiders);



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