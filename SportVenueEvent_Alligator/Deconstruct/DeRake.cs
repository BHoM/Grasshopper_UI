using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using BH.oM.SportVenueEvent;
using BH.UI.Alligator.Base;
using BH.UI.Alligator.Query;

namespace BH.UI.Grasshopper.SportVenueEvent
{
    public class DeRake : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DeRake1 class.
        /// </summary>
        public DeRake()
          : base("Deconstruct Rake", "DeRake",
              "",
              "SportVenueEvent", "Deconstuct")
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }


        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "Rake", "Rake", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "Rows", "Rows", "", GH_ParamAccess.list);
            pManager.AddParameter(new BHoMObjectParameter(), "Gangways", "Gangways", "", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Rake rake = new Rake();
            rake = DA.BH_GetData(0, rake);

            DA.BH_SetDataList(0, rake.Rows);
            DA.BH_SetDataList(1, rake.Gangways);
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
            get { return new Guid("0e548465-ba36-4fef-96c3-ecc77ea7a938"); }
        }
    }
}