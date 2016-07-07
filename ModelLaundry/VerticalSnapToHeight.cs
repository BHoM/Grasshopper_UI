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
    public class VerticalSnapToHeight : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the VerticalPointSnaping class.
        /// </summary>
        public VerticalSnapToHeight() : base("VerticalSnapToHeight", "VEndSnap", "Description", "Alligator", "ModelLaundry") { }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("GeometryToSnap", "GeomToSnap", "Input an BHoM polyline", GH_ParamAccess.item);
            pManager.AddNumberParameter("HeightToSnapTo", "HeightToSnapTo", "Input a set of heights to snap to", GH_ParamAccess.list);
            pManager.AddNumberParameter("Tolerance", "Tol", "Set a tolerance for the snapping", GH_ParamAccess.item, 0.2);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SnapedGeometry", "SnapedGeom", "New BHoM Polyline", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            object element = Utils.GetGenericData<object>(DA, 0);
            List<double> refHeights = Utils.GetDataList<double>(DA, 1);
            double tol = Utils.GetData<double>(DA, 2);

            // Get the geometry of the element
            BH.GeometryBase geometry = null;
            if (element is BHoM.Global.BHoMObject)
                geometry = ((BHoM.Global.BHoMObject)element).GetGeometry();
            else if (element is BH.GeometryBase)
                geometry = element as BH.GeometryBase;

            // Do the actal snapping
            BH.GeometryBase output = null;
            if (geometry is BH.Line)
            {
                output = Snapping.VerticalEndSnap((BH.Line)geometry, refHeights, tol);
            }
            else if (geometry is BH.Curve)
            {
                output = Snapping.VerticalEndSnap((BH.Curve)geometry, refHeights, tol);
            }
            else if (geometry is BH.Group<BH.Curve>)
            {
                output = Snapping.VerticalEndSnap((BH.Group<BH.Curve>)geometry, refHeights, tol);
            }

            // Prepare the result
            object result = element;
            if (element is BHoM.Global.BHoMObject)
            {
                result = (BHoM.Global.BHoMObject)((BHoM.Global.BHoMObject)element).ShallowClone();
                ((BHoM.Global.BHoMObject)result).SetGeometry(output);
            }
            else if (element is BH.GeometryBase)
            {
                result = output;
            }

            // Return the outputs
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
                return base.Icon;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("d1634fe1-9e9a-47c0-9728-6566e76e524d"); }
        }
    }
}