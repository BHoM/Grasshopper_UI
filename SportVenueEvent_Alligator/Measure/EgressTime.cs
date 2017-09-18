using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using BH.oM.SportVenueEvent;
using BH.Engine.SportVenueEvent;
using BH.UI.Alligator.Base;

namespace SportVenueEvent_Alligator.Measure
{
    public class EgressTime : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the EgressTime class.
        /// </summary>
        public EgressTime()
          : base("EgressTime", "EgressTime",
              "",
              "SportVenueEvent", "Measure")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "Tier", "Tier", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("Flow", "Flow", "Flow rate in [people/ (min * m)]", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Egress time", "Time", "", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Tier tier = null;
            double flowRate = 66;
            DA.BH_GetData(0, tier);
            DA.GetData(1, ref flowRate);
            DA.SetDataList(0, tier.EgressTime(flowRate));
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
            get { return new Guid("81cf34b9-78b2-4e30-8c5b-23027930ca46"); }
        }
    }
}