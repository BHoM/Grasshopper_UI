using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using BH.oM.Acoustic;
using BHG = BH.oM.Geometry;
using BH.UI.Alligator.Base;

namespace BH.UI.Alligator.Acoustic
{
    public class DeconstructRay : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DeconstructRay class.
        /// </summary>
        public DeconstructRay()
          : base("DeconstructRay", "DeRay",
              "Explode BHoM Acoustic Ray into its parts",
              "Alligator", "Acoustics")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "Ray", "Ray", "BHoM Acoustic Ray", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMGeometryParameter(), "Path", "P", "Ray polyline", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Source", "S", "ID of the ray source", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Receiver", "R", "ID of the ray target", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Ray ray = new Ray();
            ray = DA.BH_GetData(0, ray);

            DA.BH_SetData(0, ray.Path);
            DA.BH_SetData(1, ray.SpeakerID);
            DA.BH_SetData(2, ray.ReceiverID);
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
            get { return new Guid("{b11dad6c-0044-4310-9667-b95a7e9f919c}"); }
        }
    }
}