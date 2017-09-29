using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Parameters;
using BH.oM.Acoustic;
using BH.Engine.Acoustic;
using BH.UI.Alligator.Base;

namespace BH.UI.Alligator.Acoustic
{
    public class CalcRASTI : GH_Component
    {
        public CalcRASTI() : base("Calculate RASTI", "CalcRASTI", "Calculate RASTI For A Zone And Speakers", "Alligator", "Acoustics") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("42DABE3F-3D6F-40F5-8F4A-AC7BC875F4CE");
            }
        }
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Acoustic_Alligator.Properties.Resources.BHoM_Acoustics_RASTI; }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Signal", "Signal", "Option Signal to measure. Default value 85dB if the parameter is left blank.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Ambient Noise", "Noise", "Optional Ambient Noise. Default value 53.5 dB if the parameter is left blank.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Reverberation Time", "RT", "Reverberation Time. Default value 0.001s if the parameter is left blank.", GH_ParamAccess.list);
            pManager.AddParameter(new BHoMObjectParameter(), "BHoM Speakers", "Speakers", "BHoM Speakers", GH_ParamAccess.list);
            pManager.AddParameter(new BHoMObjectParameter(), "BHoM Zone", "Zone", "BHoM Zone", GH_ParamAccess.item);

            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Calculated RASTI", "RASTI", "Calculated Result of RASTI", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<double> signal = new List<double>();
            List<double> noise = new List<double>();
            List<double> rt = new List<double>();
            List<Speaker> speakers = new List<Speaker>();
            Zone zone = default(Zone);

            signal = DA.BH_GetDataList(0, signal);
            noise = DA.BH_GetDataList(1, noise);
            rt = DA.BH_GetDataList(2, rt);
            speakers = DA.BH_GetDataList(3, speakers);
            zone = DA.BH_GetData(4, zone);

            DA.SetDataList(0, Query.GetSTI(signal, noise, rt, speakers, zone));
        }
    }
}