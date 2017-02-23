using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grasshopper.Kernel;
using Rhino.Geometry;


using BHP = BHoM.Structural.Properties;
using BHG = BHoM.Geometry;

using Grasshopper_Engine;

using StructuralDesign_Toolkit;
using StructuralDesign_Toolkit.Steel;
using StructuralDesign_Toolkit.Steel.Eurocode1993;

namespace Design_Alligator.Structural
{
    public class GetCapacityCurves : GH_Component
    {

        public GetCapacityCurves() : base("Get CapacityCurves", "GetCrvs", "Gets axial force - bending moment capacity curves", "Structure", "Design") { }
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("82CCB187-473C-48BD-9031-FB8CDF132A90");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Section property", "SecProp", "Section property", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Major axis curves", "MajComp", "Capacity curves for the major axis", GH_ParamAccess.item);
            pManager.AddCurveParameter("Minor axis curves", "MinComp", "Capacity curves for the minor axis", GH_ParamAccess.item);
            pManager.AddCurveParameter("Major axis curves", "MajAx", "Capacity curves for the major axis", GH_ParamAccess.item);
            pManager.AddCurveParameter("Minor axis curves", "MinAx", "Capacity curves for the minor axis", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHP.SectionProperty prop = DataUtils.GetGenericData<BHP.SectionProperty>(DA, 0);

            Curve  majorCrv = null;
            Curve minorCrv = null;
            Curve majorCrvAx = null;
            Curve minorCrvAx = null;

            if (prop is BHP.SteelSection)
            {
                EUSteelSection major = new EUSteelSection(prop as BHP.SteelSection, Axis.Major);
                EUSteelSection minor = new EUSteelSection(prop as BHP.SteelSection, Axis.Minor);

                majorCrv=GeometryUtils.Convert(major.GetMomentCompressionCurve());
                minorCrv=GeometryUtils.Convert(minor.GetMomentCompressionCurve());
                majorCrvAx = GeometryUtils.Convert(major.GetMomentAxialCurve());
                minorCrvAx = GeometryUtils.Convert(minor.GetMomentAxialCurve());

            }

            DA.SetData(0, majorCrv);
            DA.SetData(1, minorCrv);
            DA.SetData(2, majorCrvAx);
            DA.SetData(3, minorCrvAx);

        }
    }
}
