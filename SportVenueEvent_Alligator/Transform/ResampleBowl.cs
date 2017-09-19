using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using BH.Engine.SportVenueEvent;
using BH.oM.SportVenueEvent;
using BH.UI.Alligator.Base;

namespace BH.UI.Grasshopper.SportVenueEvent
{
    public class ResampleBowl : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ResampleBowl class.
        /// </summary>
        public ResampleBowl()
          : base("ResampleBowl", "ReBowl",
              "",
              "SportVenueEvent", "Transform")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "Tier", "Tier", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("Seat width", "Width", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "Tier", "Tier", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Tier> tiers = new List<Tier>();
            double width = 0;
            tiers = DA.BH_GetDataList(0, tiers);
            width = DA.BH_GetData(1, width);

            Bowl bowl = new Bowl(tiers);
            DA.BH_SetDataList(0, bowl.ResampleBowl(width));
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
            get { return new Guid("d038b156-bc7c-4377-8d83-aae19775b444"); }
        }
    }
}