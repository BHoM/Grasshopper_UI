using System;
using System.Collections.Generic;
using BHG = BH.oM.Geometry;
using BH.oM.Acoustic;
using Grasshopper.Kernel;
using BH.UI.Alligator.Base;

namespace BH.UI.Alligator.Acoustic
{
    public class CreateReceiver : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateReceiver class.
        /// </summary>
        public CreateReceiver()
          : base("CreateReceiver", "Rec",
              "Creates BHoM Acoustic receiver",
              "Alligator", "Acoustics")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new BHoMGeometryParameter(), "Position", "P", "Receiver central point", GH_ParamAccess.item);
            pManager.AddTextParameter("Category", "T", "Category type of Receiver for directivity calculation", GH_ParamAccess.item);
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "Receiver", "Rec", "BHoM Acoustic Receiver", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Receiver> receivers = new List<Receiver>();
            BHG.Point pos = new BHG.Point();
            string cat = "Omni";
            pos = DA.BH_GetData(0, pos);
            cat = DA.BH_GetData(1, cat);

            Receiver receiver = new Receiver(pos, cat);

            DA.SetDataList(0, receivers);
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
            get { return new Guid("{8e6a26c4-05f9-46db-8fd0-7328059a3003}"); }
        }
    }
}