using System;
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
    public class FilterSmallCurves : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the HorizontalExtend class.
        /// </summary>
        public FilterSmallCurves()
          : base("FilterSmallCurves", "FilterSCurves",
              "Remove all curves with a length below maxLength",
              "Alligator", "ModelLaundry")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("BHoM Elements", "bhElements", "Element owning the curves to be filtered", GH_ParamAccess.item);
            pManager.AddNumberParameter("max Length", "maxL", "max length of the curve", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("cleared object", "remaining", "Element whose curves have been filtered", GH_ParamAccess.list);
            pManager.AddGenericParameter("removed", "removed", "curves removed from teh element", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Getting the inputs from GH
            object element = Utils.GetGenericData<object>(DA, 0);
            double dist = Utils.GetData<double>(DA, 1);

            Group<BH.Curve> removed = new Group<BH.Curve>();
            object remaining = Util.RemoveSmallContours(element, dist, out removed);

            // Setting the GH outputs
            DA.SetData(0, remaining);
            DA.SetData(1, removed);
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
            get { return new Guid("8F6BB61E-F8D1-4F12-953C-1C9D394D435C"); }
        }
    }
}