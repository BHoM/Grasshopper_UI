using System;
using System.Collections.Generic;

using BHG = BHoM.Geometry;
using BHA = BHoM.Acoustic;
using AcousticSPI_Engine;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Acoustic_Alligator
{
    public class DirectSoundSolver : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DirectSoundSolver class.
        /// </summary>
        public DirectSoundSolver()
          : base("DirectSound", "DS",
              "Solve Direct Sound calculation",
              "Alligator", "Acoustics")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Speaker", "Spk", "BHoM Acoustic Speaker", GH_ParamAccess.list);
            pManager.AddGenericParameter("Receiver", "Rec", "BHoM Acoustic Receiver", GH_ParamAccess.list);
            pManager.AddGenericParameter("Panels", "Pan", "BHoM Acoustic Panel", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Parallel", "Par", "Choose computation method: [0] Serial, [1] CPU Threaded, [2] GPU Threaded", GH_ParamAccess.item);

            pManager[2].Optional = true;
            pManager[3].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Rays", "Rays", "BHoM Acoustic Rays", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List <BHA.Speaker> spk = new List<BHA.Speaker>();
            List<BHA.Receiver> rec = new List<BHA.Receiver>();
            List<BHA.Panel> pan = new List<BHA.Panel>();
            int par = 0;

            if (!DA.GetDataList(0, spk)) { return; }
            if (!DA.GetDataList(1, rec)) { return; }
            if (!DA.GetDataList(2, pan)) { pan = null; }
            if (!DA.GetData(3, ref par)) { par = 0; }
          
            DA.SetDataList(0, DirectSound.Solve(spk, rec, pan, par));
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
            get { return new Guid("{cb8d1c29-7b65-4d54-9a55-e3851b5feba5}"); }
        }
    }
}