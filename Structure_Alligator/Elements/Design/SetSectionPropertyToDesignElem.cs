//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Grasshopper.Kernel;
//using BH.Engine.Grasshopper.Components;
//using BH.Engine.Grasshopper;
//using BHE = BH.oM.Structural.Elements;
//using BHP = BH.oM.Structural.Properties;
//using BH.oM.Structural.Design;

//namespace BH.UI.Alligator.Structural.Elements.Design
//{
//    public class SetSectionPropertyToDesignElem : GH_Component  //TODO: Requires the corresponding methods in engine 2.0
//    {
//        public SetSectionPropertyToDesignElem() : base("Set section property", "SetSecProp", "Set the section property for a design element and all its analytic bars", "Structure", "Design") { }

//        public override GH_Exposure Exposure
//        {
//            get
//            {
//                return GH_Exposure.secondary;
//            }
//        }

//        public override Guid ComponentGuid
//        {
//            get
//            {
//                return new Guid("267985B4-7048-4FF8-B4E3-80E433AF9CF7");
//            }
//        }

//        protected override void RegisterInputParams(GH_InputParamManager pManager)
//        {
//            pManager.AddGenericParameter("DesignElement", "DesElem", "Design element update section property on", GH_ParamAccess.item);
//            pManager.AddGenericParameter("SectionProperty", "SecProp", "The section property to set", GH_ParamAccess.item);
//        }

//        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
//        {
//            pManager.AddGenericParameter("DesignElement", "DesElem", "The updated design element", GH_ParamAccess.item);

//        }

//        protected override void SolveInstance(IGH_DataAccess DA)
//        {
//            StructuralLayout elem = (StructuralLayout)DataUtils.GetData<StructuralLayout>(DA, 0).GetShallowClone();
//            List<BHE.Bar> bars = elem.AnalyticBars;
//            bars = bars.Select(x => (BHE.Bar)x.GetShallowClone()).ToList();
//            elem.AnalyticBars = bars;

//            BHP.SectionProperty prop = DataUtils.GetData<BHP.SectionProperty>(DA, 1);

//            elem.SetSectionProperty(prop);

//            DA.SetData(0, elem);
//        }
//    }
//}
