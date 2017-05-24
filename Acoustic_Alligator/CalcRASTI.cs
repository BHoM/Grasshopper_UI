using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rhino.Geometry;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Parameters;

using BHG = BHoM.Geometry;
using BHA = BHoM.Acoustic;
using AcousticSPI_Engine;
using GHE = Grasshopper_Engine;

namespace Acoustic_Alligator
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
            pManager.AddGenericParameter("BHoM Zone", "Zone", "BHoM Zone", GH_ParamAccess.item);
            pManager.AddGenericParameter("BHoM Speakers", "Speakers", "BHoM Speakers", GH_ParamAccess.list);
            pManager.AddGenericParameter("Signal", "Signal", "Signal to measure", GH_ParamAccess.list);             
            pManager.AddGenericParameter("Ambient Noise", "Noise", "Ambient Noise", GH_ParamAccess.list);           
            pManager.AddGenericParameter("Reverberation Time", "RT", "Reverberation Time", GH_ParamAccess.list);    

            pManager[2].Optional = true;
            pManager[3].Optional = true;
            pManager[4].Optional = true;

            // Indexing defaulting parameters
            Param_GenericObject param2 = (Param_GenericObject)pManager[2];
            Param_GenericObject param3 = (Param_GenericObject)pManager[3];
            Param_GenericObject param4 = (Param_GenericObject)pManager[4];

            // Assigning default values to indexed parameters
            param2.PersistentData.Append(new GH_ObjectWrapper(85));
            param3.PersistentData.Append(new GH_ObjectWrapper(53.5));
            param4.PersistentData.Append(new GH_ObjectWrapper(0.001));
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Calculated RASTI", "RASTI", "Calculated Result of RASTI", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHA.Zone zone = GHE.DataUtils.GetGenericData<BHA.Zone>(DA, 0);
            List<BHA.Speaker> speakers = GHE.DataUtils.GetGenericDataList<BHA.Speaker>(DA, 1);
            List<double> signal = GHE.DataUtils.GetGenericDataList<double>(DA, 2);
            List<double> noise = GHE.DataUtils.GetGenericDataList<double>(DA, 3);
            List<double> rt = GHE.DataUtils.GetGenericDataList<double>(DA, 4);

            BHA.AcousticRASTIParameters param = new BHA.AcousticRASTIParameters();

            DA.SetDataList(0, STICalculator.Solve(param, signal, noise, rt, speakers, zone));
        }
    }
}