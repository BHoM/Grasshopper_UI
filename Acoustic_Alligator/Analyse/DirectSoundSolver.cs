using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using BHG = BH.oM.Geometry;
using BH.oM.Acoustic;
using BH.Engine.Acoustic;
using BH.UI.Alligator.Base;
using BH.UI.Alligator.Query;

namespace BH.UI.Alligator.Acoustic
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
            pManager.AddParameter(new BHoMObjectParameter(), "Speaker", "Spk", "BHoM Acoustic Speaker", GH_ParamAccess.list);
            pManager.AddParameter(new BHoMObjectParameter(), "Receiver", "Rec", "BHoM Acoustic Receiver", GH_ParamAccess.list);
            pManager.AddParameter(new BHoMObjectParameter(), "Panels", "Pan", "BHoM Acoustic Panel", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Parallel", "Par", "Choose computation method: [0] Serial, [1] CPU Threaded, [2] GPU Threaded", GH_ParamAccess.item);

            pManager[2].Optional = true;
            pManager[3].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "Rays", "Rays", "BHoM Acoustic Rays", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List <Speaker> spk = new List<Speaker>();
            List<Receiver> rec = new List<Receiver>();
            List<Panel> pan = new List<Panel>();
            int par = 0;

            DA.BH_GetDataList(0, spk);
            DA.BH_GetDataList(1, rec);
            DA.BH_GetDataList(2, pan);
            DA.BH_GetData(3, par);
            
            if (par == 0)
                DA.SetDataList(0, Analyse.DirectSound(spk, rec, pan));
            else if (par == 1)
                DA.SetDataList(0, Analyse.DirectSoundCPU(spk, rec, pan)); 
            else if (par == 2)
                DA.SetDataList(0, Analyse.DirectSoundGPU(spk.ToArray(), rec.ToArray()));
            else if (par == 3)
                DA.SetDataList(0, Analyse.DirectSoundCuda(spk, rec, pan));
            else
                throw new Exception("Parallel parameter cannot be left blank or be higher than 3. Please specify calculation method: [0] Serial, [1] CPU Threaded, [2] CUDA accelerated. WIP: GPU not working, [3] OpenCL accelerated. WIP: Not Working");
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