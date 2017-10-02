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
    public class GenerateSpanSupportedNodes : BHoMBaseComponent<BHE.Span>
    {
        public GenerateSpanSupportedNodes() : base("GenerateSpans", "GenSpan", "Generate spans on a design element by setting which node indecies that is supported", "Structure", "Design") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("A66FAE46-89DA-459D-BB75-C815C350EC70");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("DesignElement", "DesElem", "Design element to add spans to", GH_ParamAccess.item);
            pManager.AddIntegerParameter("SUpported Nodes", "SupNode", "Supported positions on the design element as the supported node indecies along the element", GH_ParamAccess.list);

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
            List<int> supNode = DataUtils.GetDataList<int>(DA, 1);

            List<BHE.Span> spans = elem.GenerateSpans(supNode, (BHE.SpanDirection)m_SelectedOption[0]);

            DA.SetData(0, elem);
            DA.SetDataList(1, spans);
        }

    }
}