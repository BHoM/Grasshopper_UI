using Alligator.Components;
using BHoM.Structural.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHE = Grasshopper_Engine;
using BHE = BHoM.Structural.Elements;
using BHI = BHoM.Structural.Interface;
using BHR = BHoM.Base.Results;
using Grasshopper.Kernel;
using Grasshopper_Engine.Components;

namespace Alligator.Structural.Results
{
    public class GetNodeDisplacement : ResultBaseComponent<NodeDisplacement>
    {
        public GetNodeDisplacement() : base("GetNodeDisplacement", "NodeDisplacement", "Gets the node displacements from the selected result server", "Structure", "Results") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("{E3FA6887-1CA4-4661-A736-8A7D1948BEFA}");
            }
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (GHE.DataUtils.Run(DA, 3))
            {
                BHI.IResultAdapter app = GHE.DataUtils.GetGenericData<BHI.IResultAdapter>(DA, 0);
                if (app != null)
                {
                    List<string> ids = GHE.DataUtils.GetDataList<string>(DA, 1);
                    List<string> cases = GHE.DataUtils.GetDataList<string>(DA, 2);
                    Dictionary<string, BHR.IResultSet> results = new Dictionary<string, BHR.IResultSet>();
                    app.GetNodeDisplacements(ids, cases, m_ResultOrder, out results);

                    SetResults<NodeDisplacement>(DA, results);
                }
            }
        }
    }


    public class GetNodeReaction : ResultBaseComponent<NodeReaction>
    {
        public GetNodeReaction() : base("GetNodeReaction", "NodeReaction", "Gets the node reactions from the selected result server", "Structure", "Results") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("{E3FA6887-0BA4-4661-A736-8A7D1948BEFA}");
            }
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (GHE.DataUtils.Run(DA, 3))
            {
                BHI.IResultAdapter app = GHE.DataUtils.GetGenericData<BHI.IResultAdapter>(DA, 0);
                if (app != null)
                {
                    List<string> ids = GHE.DataUtils.GetDataList<string>(DA, 1);
                    List<string> cases = GHE.DataUtils.GetDataList<string>(DA, 2);
                    Dictionary<string, BHR.IResultSet> results = new Dictionary<string, BHR.IResultSet>();
                    app.GetNodeReactions(ids, cases, m_ResultOrder, out results);

                    SetResults<NodeReaction>(DA, results);
                }
            }
        }
    }
}
