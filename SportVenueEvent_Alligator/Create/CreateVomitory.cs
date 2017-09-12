using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using RHG = Rhino.Geometry;

using BHG = BH.oM.Geometry;
using BH.oM.SportVenueEvent;


namespace BH.UI.Grasshopper.SportVenueEvent
{
    public class CreateVomitory : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreatVomitory class.
        /// </summary>
        public CreateVomitory()
          : base("Create Vomitory", "Vomitory",
              "",
              "SportVenueEvent", "Create")
        {
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.primary;
            }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Tier", "Tier", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("Centre", "Centre", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("Width", "Width", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Vomitory", "Vom", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Vomitory vom = new Vomitory();
            Tier tier = new Tier();
            BHG.Point centre = new BHG.Point();
            double width = 0;

            DA.GetData(0, ref tier);
            DA.GetData(1, ref centre);
            DA.GetData(2, ref width);
            DA.SetData(0, new Vomitory(tier, centre, width));
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
            get { return new Guid("bddd3f85-2e7f-4b2d-9b16-2a12a6154c2a"); }
        }
    }
}