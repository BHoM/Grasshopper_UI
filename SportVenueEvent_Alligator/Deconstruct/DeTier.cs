using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using BHG = BHoM.Geometry;
using BH.oM.SportVenueEvent;
using BH.UI.Alligator.Base;

namespace BH.UI.Grasshopper.SportVenueEvent
{
    public class DeTier : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DeTier class.
        /// </summary>
        public DeTier()
          : base("Deconstruct Tier", "DeTier",
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
            pManager.AddParameter(new BHoMObjectParameter(), "Tier", "Tier", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "Rakes", "Rakes", "", GH_ParamAccess.list);
            pManager.AddParameter(new BHoMObjectParameter(), "Rows", "Rows", "", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Tier tier = new Tier();
            DA.GetData(0, ref tier);
            DA.SetDataList(0, tier.Rakes);
            DA.SetDataList(1, tier.Rows);
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
            get { return new Guid("348f78cd-bb1e-4eb8-a13e-ed115d904fae"); }
        }
    }
}