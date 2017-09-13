//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Grasshopper.Kernel;
//using BH.Engine.Grasshopper.Components;
//using BH.Engine.Grasshopper;
//using BHD = BH.oM.Structural.Design;

//namespace BH.UI.Alligator.Structural.Elements.Design  //TODO: Requires the corresponding methods in engine 2.0
//{
//    public class SetEffectiveLength : BHoMBaseComponent<BHD.Span>
//    {
//        public SetEffectiveLength() : base("Set Effective Length", "SetEffLength", "Set effective length fo a design element", "Structure", "Design") { }

//        public override Guid ComponentGuid
//        {
//            get
//            {
//                return new Guid("16B63874-25C2-419E-81D8-F7CA5D1A9A28");
//            }
//        }

//        protected override void RegisterInputParams(GH_InputParamManager pManager)
//        {
//            pManager.AddGenericParameter("DesignElement", "DesElem", "Design element to add spans to", GH_ParamAccess.item);
//            pManager.AddNumberParameter("Effective Length", "EffLen", "The effective length", GH_ParamAccess.item);
      
//            AppendEnumOptions("Span dir", typeof(BHD.SpanDirection));
//        }

//        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
//        {
//            pManager.AddGenericParameter("DesignElement", "DesElem", "The designelement with generated spans", GH_ParamAccess.item);
//            pManager.AddGenericParameter("Spans", "Spans", "The generated spans", GH_ParamAccess.list);
//        }

//        protected override void SolveInstance(IGH_DataAccess DA)
//        {
//            BHD.StructuralLayout elem = (BHD.StructuralLayout)DataUtils.GetData<BHD.StructuralLayout>(DA, 0).GetShallowClone();
//            double length = DataUtils.GetData<double>(DA, 1);

//            List<BHD.Span> spans = elem.SetEffectiveLength(length, (BHD.SpanDirection)m_SelectedOption[0]);

//            DA.SetData(0, elem);
//            DA.SetDataList(1, spans);
//        }

//    }
//}