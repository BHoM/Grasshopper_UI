using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using GHE = Grasshopper_Engine;
using BHA = BHoM.Acoustic;
using BHG = BHoM.Geometry;

namespace Acoustic_Alligator
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
            pManager.AddGenericParameter("Ray", "Ray", "BHoM Acoustic Ray", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Path", "P", "Ray polyline", GH_ParamAccess.list);
            pManager.AddTextParameter("Source", "S", "ID of the ray source", GH_ParamAccess.list);
            pManager.AddTextParameter("Receiver", "R", "ID of the ray target", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<BHA.Ray> rays = new List<BHoM.Acoustic.Ray>();
            List<BHG.Polyline> path = new List<BHoM.Geometry.Polyline>();
            List<string> sID = new List<string>();
            List<string> rID = new List<string>();

            if (!DA.GetDataList(0, rays)) { return; }

            for (int i = 0; i<rays.Count;i++)
            {
                path.Add(rays[i].Path);
                sID.Add(rays[i].Source);
                rID.Add(rays[i].Target);
            }

            DA.SetDataList(0, path);
            DA.SetDataList(1, sID);
            DA.SetDataList(2, rID);
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