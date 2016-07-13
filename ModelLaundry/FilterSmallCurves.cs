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
            pManager.AddGenericParameter("Curves to Filter", "InCurves", "Curves to be filtered", GH_ParamAccess.item);
            pManager.AddNumberParameter("max Lenght", "maxL", "max length of the curve", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("remaining", "remaining", "curves longer than maxLength", GH_ParamAccess.list);
            pManager.AddGenericParameter("removed", "removed", "curves shorter than maxLength", GH_ParamAccess.list);
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

            // Get the geometry of the ref elements
            BH.GeometryBase geom = null;
            if (element is BHoM.Global.BHoMObject)
                geom = ((BHoM.Global.BHoMObject)element).GetGeometry();
            else if (element is BH.GeometryBase)
            geom = element as BH.GeometryBase;

            BH.Group<BH.Curve> group = new BH.Group<BH.Curve>();
            if (geom is BH.Curve)
                group.Add((BH.Curve)geom);
            else if (geom is BH.Group<BH.Curve>)
            {
                group = geom as BH.Group<BH.Curve>;
            }

            // Actually do the filtering
            BH.Group<BH.Curve> removed = new Group<BH.Curve>();
            BH.Group<BH.Curve> remaining = BHoM_Engine.ModelLaundry.Util.RemoveSmallContours(group, dist, out removed);

            // Prepare the result
            object result = element;
            if (element is BHoM.Global.BHoMObject)
            {
                result = (BHoM.Global.BHoMObject)((BHoM.Global.BHoMObject)element).ShallowClone();
                ((BHoM.Global.BHoMObject)result).SetGeometry(remaining);
            }
            else if (element is BH.GeometryBase)
            {
                result = group;
            }

            /*else if(geometry is BH.PolyCurve)
            {
                BH.PolyCurve curve = geometry as BH.PolyCurve;
                List<BH.Curve> tempCrv = curve.Explode();
                List<BH.Point> tempPts = new List<BH.Point>();
                for (int j = 0; j < tempCrv.Count; j++)
                {
                    tempPts.Add(tempCrv[j].StartPoint);
                }

                if (curve.IsClosed())
                {
                    tempPts.Add(tempCrv[tempCrv.Count - 1].EndPoint);
                }

                BH.Polyline newPolyLine = new BH.Polyline(tempPts);
                output = Util.HorizontalExtend(newPolyLine, dist);
            }*/

            // Setting the GH outputs
            DA.SetData(0, result);
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