using System;
using Grasshopper.Kernel;
using MLE = ModelLaundry_Engine;
using GHE = Grasshopper_Engine;


namespace Alligator.ModelLaundry
{
    public class HorizontalExtend : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the HorizontalExtend class.
        /// </summary>
        public HorizontalExtend()
          : base("HorizontalExtend", "HExtend",
              "Description",
              "Alligator", "ModelLaundry")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("BH.oM Elements", "bhElements", "BH.oM object or geometry to extend", GH_ParamAccess.item);
            pManager.AddNumberParameter("Distance", "distance", "Extention distance", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Extended Element", "extended", "New object or geometry after extention", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Getting the inputs from GH
            object element = GHE.DataUtils.GetGenericData<object>(DA, 0);
            double dist = GHE.DataUtils.GetData<double>(DA, 1);

            object result = MLE.Util.HorizontalExtend(element, dist);
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
            get { return new Guid("{486fee2f-8167-416d-a4f1-5f9dda0e0570}"); }
        }
    }
}