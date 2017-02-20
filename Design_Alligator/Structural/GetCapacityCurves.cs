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
            pManager.AddCurveParameter("Major axis curves", "Maj", "Capacity curves for the major axis", GH_ParamAccess.list);
            pManager.AddCurveParameter("Minor axis curves", "Min", "Capacity curves for the minor axis", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHP.SectionProperty prop = DataUtils.GetGenericData<BHP.SectionProperty>(DA, 0);

            List<Curve> majorCrvs = new List<Curve>();
            List<Curve> minorCrvs = new List<Curve>();

            if (prop is BHP.SteelSection)
            {
                EUSteelSection major = new EUSteelSection(prop as BHP.SteelSection, Axis.Major);
                EUSteelSection minor = new EUSteelSection(prop as BHP.SteelSection, Axis.Minor);

                majorCrvs.Add(GeometryUtils.Convert(major.GetMomentCompressionCurve()));
                minorCrvs.Add(GeometryUtils.Convert(minor.GetMomentCompressionCurve()));
                //for (int i = 0; i < 3; i++)
                //{
                //    BHG.Curve c = major.GetMomentCompressionCurve((DesignType)i);
                //    majorCrvs.Add(GeometryUtils.Convert(c));
                //}

                //for (int i = 0; i < 3; i++)
                //{
                //    BHG.Curve c = minor.GetMomentCompressionCurve((DesignType)i);
                //    minorCrvs.Add(GeometryUtils.Convert(c));
                //}

            }

            DA.SetDataList(0, majorCrvs);
            DA.SetDataList(1, minorCrvs);

        }
    }
}
