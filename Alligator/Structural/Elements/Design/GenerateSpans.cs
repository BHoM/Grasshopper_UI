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
    public class GenerateSpans : BHoMBaseComponent<BHE.Span>
    {
        public GenerateSpans() : base("Generate Spans", "GenSpan", "Generate spans on a design element", "Structure", "Design") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("F2A62A6A-51D6-4D24-AB4F-C8BB661D5C68");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("DesignElement", "DesElem", "Design element to add spans to", GH_ParamAccess.item);
            pManager.AddNumberParameter("Support Positions", "SupPos", "Supported positions on the design element", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Support as length", "Len", "Support positions as length or fraction of the element between 0 and 1", GH_ParamAccess.item, false);

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
            List<double> supPos = DataUtils.GetDataList<double>(DA, 1);
            bool supAsLength = DataUtils.GetData<bool>(DA, 2);

            List<BHE.Span> spans = elem.GenerateSpans(supPos, (BHE.SpanDirection)m_SelectedOption[0], supAsLength);

            DA.SetData(0, elem);
            DA.SetDataList(1, spans);
        }

    }
}
