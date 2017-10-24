using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using BH.oM.Acoustic;
using BH.UI.Alligator.Base;

namespace BH.UI.Alligator.Acoustic
{
    public class SumSoundLevels : GH_Component
    {
        public SumSoundLevels() : base("SumSPL", "Sum SPL", "Sound Pressure Level summation", "Alligator", "Acoustics") { }
        protected override System.Drawing.Bitmap Icon { get { return null; } }
        public override Guid ComponentGuid { get { return new Guid("{e3664f69-592e-4e9b-8b98-a1fe43fc60f1}"); } }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "Rays", "Rays", "List of BHoM Acoustic Rays", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("SPL", "SPL", "Sound pressure level", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Ray> rays = new List<Ray>();
            DA.GetDataList(0, rays);

            DA.SetData(0, Engine.Acoustic.Query.GetSoundPressureLevel(rays));
        }
    }
}