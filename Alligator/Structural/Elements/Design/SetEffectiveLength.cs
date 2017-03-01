using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Grasshopper_Engine.Components;
using Grasshopper_Engine;
using BHE = BHoM.Structural.Elements;

namespace Alligator.Structural.Elements.Design
{
    public class SetEffectiveLength : BHoMBaseComponent<BHE.Span>
    {
        public SetEffectiveLength() : base("Set Effective Length", "SetEffLength", "Set effective length fo a design element", "Structure", "Design") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("16B63874-25C2-419E-81D8-F7CA5D1A9A28");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("DesignElement", "DesElem", "Design element to add spans to", GH_ParamAccess.item);
            pManager.AddNumberParameter("Effective Length", "EffLen", "The effective length", GH_ParamAccess.item);

            AppendEnumOptions("Span dir", typeof(BHE.SpanDirection));
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("DesignElement", "DesElem", "The designelement with generated spans", GH_ParamAccess.item);
            pManager.AddGenericParameter("Spans", "Spans", "The generated spans", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHE.DesignElement elem = (BHE.DesignElement)DataUtils.GetData<BHE.DesignElement>(DA, 0).ShallowClone();
            double length = DataUtils.GetData<double>(DA, 1);

            List<BHE.Span> spans = elem.SetEffectiveLength(length, (BHE.SpanDirection)m_SelectedOption[0]);

            DA.SetData(0, elem);
            DA.SetDataList(1, spans);
        }

    }
}