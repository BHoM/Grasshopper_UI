using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using MLE = ModelLaundry_Engine;
using GHE = Grasshopper_Engine;
using BHG = BH.oM.Geometry;
using RG = Rhino.Geometry;


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
            pManager.AddGenericParameter("BH.oM Elements", "bhElements", "BH.oM objects or geometry to be filtered", GH_ParamAccess.list);
            pManager.AddBoxParameter("Boundingbox to Filter by", "bBox", "Insert a set of boundingboxes to filter by", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Insiders", "inside", "Elements inside the boundingbox", GH_ParamAccess.list);
            pManager.AddGenericParameter("Outsiders", "outside", "Elements outside the boundingbox", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Get GH inputs
            List<object> elements = GHE.DataUtils.GetGenericDataList<object>(DA, 0);
            List<RG.Box> boxes = new List<RG.Box>();
            if (!DA.GetDataList(1, boxes)) return;

            // Convert boxes to BH.oM bounding boxes
            List<BHG.BoundingBox> BH.oMBoxes = new List<BHG.BoundingBox>();
            for (int i = 0; i < boxes.Count; i++)
            {
                RG.Point3d[] cornerPt = boxes[i].GetCorners();
                List<BHG.Point> BH.oMBoxCornerPt = new List<BHG.Point>();
                for (int j = 0; j < 8; j++)
                {
                    BH.oMBoxCornerPt.Add(new BHG.Point(cornerPt[j].X, cornerPt[j].Y, cornerPt[j].Z));
                }
                BH.oMBoxes.Add(new BHG.BoundingBox(BH.oMBoxCornerPt));
            }

            List<object> outsiders = new List<object>();
            List<object> insiders = MLE.Util.FilterByBoundingBox(elements, BH.oMBoxes, out outsiders);

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