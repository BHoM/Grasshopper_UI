using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using BHG = BH.oM.Geometry;
using BH.oM.Acoustic;
using BH.Engine.Acoustic;
using BH.UI.Alligator.Base;

namespace BH.UI.Alligator.Acoustic
{
    public class DirectSoundSolver : GH_Component
    {
        public DirectSoundSolver() : base("DirectSound", "DS", "Solve Direct Sound calculation", "Alligator", "Acoustics") { }
        protected override System.Drawing.Bitmap Icon { get { return null; } }
        public override Guid ComponentGuid { get { return new Guid("{cb8d1c29-7b65-4d54-9a55-e3851b5feba5}"); } }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "Speaker", "Spk", "BHoM Acoustic Speaker", GH_ParamAccess.list);
            pManager.AddParameter(new BHoMObjectParameter(), "Receiver", "Rec", "BHoM Acoustic Receiver", GH_ParamAccess.list);
            pManager.AddParameter(new BHoMObjectParameter(), "Panels", "Pan", "BHoM Acoustic Panel", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Parallel", "Par", "Choose computation method: [0] Serial, [1] CPU Threaded, [2] GPU Threaded", GH_ParamAccess.item);

            pManager[2].Optional = true;
            pManager[3].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "Rays", "Rays", "BHoM Acoustic Rays", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List <Speaker> spk = new List<Speaker>();
            List<Receiver> rec = new List<Receiver>();
            List<Panel> pan = new List<Panel>();
            int par = 0;

            DA.GetDataList(0, spk);
            DA.GetDataList(1, rec);
            DA.GetDataList(2, pan);
            DA.GetData(3, ref par);
            
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
    }
}
