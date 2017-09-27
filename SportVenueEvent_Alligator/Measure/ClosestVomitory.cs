using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using BH.oM.SportVenueEvent;
using BH.Engine.SportVenueEvent;
using BH.UI.Alligator.Base;

namespace BH.UI.Grasshopper.SportVenueEvent
{
    public class ClosestVomitory : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GetClosestVomitory class.
        /// </summary>
        public ClosestVomitory()
          : base("Closest Vomitory", "ClosestVom", "", "SportVenueEvent", "Measure")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "Tier", "Tier", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("Vomitories", "Vomitories","", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("Index", "Index", "Index of closest vomitory", GH_ParamAccess.list);
            pManager.AddParameter(new BHoMObjectParameter(), "Tier", "Tier", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Tier tier = new Tier();
            List<Vomitory> vomitories = new List<Vomitory>();
            tier = DA.BH_GetData(0, tier);
            vomitories = DA.BH_GetDataList(1, vomitories);

            DA.BH_SetDataList(0, tier.AssignVomitories(vomitories));
            DA.BH_SetData(1, tier);
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
            get { return new Guid("1ced8fe7-9afd-4bab-88c7-7cd93f0d288b"); }
        }
    }
}