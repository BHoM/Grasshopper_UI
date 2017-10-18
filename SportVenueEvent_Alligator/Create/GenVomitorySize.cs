using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using BH.oM.SportVenueEvent;
using BH.Engine.SportVenueEvent;
using BH.UI.Alligator.Base;
using BH.UI.Alligator;

namespace BH.UI.Grasshopper.SportVenueEvent
{
    public class GenVomitorySize : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MinVomWidth class.
        /// </summary>
        public GenVomitorySize()
          : base("Vomitory size", "VomSize",
              "",
              "SportVenueEvent", "Generative")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "Tier", "Tier", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("Flow", "Flow", "Flow rate in [people/ (min * m)]", GH_ParamAccess.item);
            pManager.AddNumberParameter("Time", "Time", "", GH_ParamAccess.item, 8);
            pManager[2].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Width", "Width", "", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Tier tier = new Tier();
            double flowRate = 66;
            double time = 8;
            tier = DA.BH_GetData(0, tier);
            flowRate = DA.BH_GetData(1, flowRate);
            time = DA.BH_GetData(2, time);
            DA.BH_SetDataList(0, tier.GenVomitorySize(flowRate, time));
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
            get { return new Guid("c3006a87-a618-4a0f-83bb-fd4011783d88"); }
        }
    }
}