using System;
using System.Collections.Generic;
using BHG = BHoM.Geometry;
using BHA = BHoM.Acoustic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Acoustic_Alligator
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
            pManager.AddGenericParameter("Position", "P", "Receiver central point", GH_ParamAccess.list);
            pManager.AddGenericParameter("Category", "T", "Category type of Receiver for directivity calculation", GH_ParamAccess.list);
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Receiver", "Rec", "BHoM Acoustic Receiver", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<BHA.Receiver> receivers = new List<BHA.Receiver>();

            List<BHG.Point> pos = new List<BHG.Point>();
            List<String> cat = new List<string>();

            if (!DA.GetDataList(0,pos)) { return; }
            if (!DA.GetDataList(1, cat)) { return; }

            for (int i = 0; i < pos.Count; i++)
            {
                if (cat.Count < pos.Count)                          // not used yet
                {
                    cat.Add("Omni");
                }
                BHA.Receiver receiver = new BHA.Receiver(pos[i]);
                receivers.Add(receiver);
            }

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