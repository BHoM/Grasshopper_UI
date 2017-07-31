using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using BHA = BHoM.Acoustic;

namespace Acoustic_Alligator
{
    public class SumSPL : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the SumSPL class.
        /// </summary>
        public SumSPL()
          : base("SumSPL", "Sum SPL",
              "Sound Pressure Level summation",
              "Alligator", "Acoustics")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Rays", "Rays", "List of BHoM Acoustic Rays", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("SPL", "SPL", "Sound pressure level", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<BHA.Ray> rays = new List<BHA.Ray>();
            if (!DA.GetDataList(0, rays)) { return; }

            DA.SetData(0, AcousticSPI_Engine.SPLCalculator.Solve(rays));
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
            get { return new Guid("{e3664f69-592e-4e9b-8b98-a1fe43fc60f1}"); }
        }
    }
}