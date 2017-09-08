using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using SportVenueEvent.oM;
using SportVenueEvent.En;
using GHE = Grasshopper_Engine;

namespace SportVenueEvent_Alligator
{
    public class ParseStadium : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ParseStadium class.
        /// </summary>
        public ParseStadium()
          : base("ParseStadium", "ParseStadium",
              "Characterize an undefined stadium geometry and returns them as as BHoM Stadium parts",
              "SportVenueEvent", "Stadium")
        {
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.tertiary;
            }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Geometry", "G", "Undifferenciated Stadium Geometry", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Stadium", "Stadium", "Newly characterised BHoM Stadium", GH_ParamAccess.item);
            pManager.AddGenericParameter("Bleachers", "Bleachers", "BHoM Stadium Bleachers", GH_ParamAccess.list);
            pManager.AddGenericParameter("Pitch", "Pitch", "BHoM Stadium Pitch", GH_ParamAccess.item);
            pManager.AddGenericParameter("Roof", "Roof", "BHoM Stadium Roof", GH_ParamAccess.list);
            pManager.AddMeshParameter("UnclassifiedMesh", "Others", "Unclassified meshes", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            /*
            Stadium stadium = new Stadium();
            if (!DA.GetData(0, ref stadium)) { return; }

            stadium.Characterise();
            DA.SetData(0, stadium);
            DA.SetDataList(1, stadium.Bleachers);
            DA.SetData(2, stadium.Pitch);
            DA.SetData(3, stadium.Roof);
            DA.SetData(4, stadium.Geometry);
            */
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
            get { return new Guid("{cb4ef743-3993-407d-9ff8-edb3d9c59992}"); }
        }
    }
}