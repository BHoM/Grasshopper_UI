using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using Grasshopper.Kernel;
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
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Calculated RASTI", "RASTI", "Calculated Result of RASTI", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            BHA.Zone zone = GHE.DataUtils.GetGenericData<BHA.Zone>(DA, 0);
            List<BHA.Speaker> speakers = GHE.DataUtils.GetGenericDataList<BHA.Speaker>(DA, 1);


            BHA.Parameters param = new BHA.AcousticRASTIParameters();
            STICalculator rasti = new STICalculator(param);

            DA.SetDataList(0, rasti.CalculateRASTI(speakers, zone, param.Frequencies, param.Octaves));
        }
    }
}