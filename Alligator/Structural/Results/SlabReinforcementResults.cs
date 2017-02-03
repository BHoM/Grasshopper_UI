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
    public class GetSlabReinforcement : ResultBaseComponent<SlabReinforcement>
    {
        public GetSlabReinforcement() : base("GetSlabReinforcement", "SlabReinforcement", "Get the Slab Reinforcement Levels", "Structure", "Results") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("3FF251C9-913A-4C06-A19F-9B0A07CF5A49");
            }
        }
        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_Get_Slabreinforcement; }
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

                    app.GetSlabReinforcement(ids, cases, m_ResultOrder, out results);

                    SetResults<SlabReinforcement>(DA, results);
                }
            }
        }
    }
}
