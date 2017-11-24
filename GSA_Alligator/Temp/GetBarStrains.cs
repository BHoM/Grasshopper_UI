using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using GHE = Grasshopper_Engine;
using BHR = BHoM.Base.Results;


namespace GSA_Alligator.Temp
{
    public class GetBarStrains : GHE.Components.ResultBaseComponent<BHoM.Structural.Results.BarStrains>
    {
        public GetBarStrains() : base("GetBarStrains", "GetBarStrains", "Gets the bar strains from the selected result server", "Structure", "Results") { }

        /// <summary> Icon (24x24 pixels)</summary>

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("B21AFA61-D76B-4906-839C-2C8C42A092F9");
            }
        }



        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Result Server", "ResultServer", "Application or Result server to extract results from", GH_ParamAccess.item);
            pManager.AddTextParameter("Ids", "Id", "List of object ids to get results", GH_ParamAccess.list);
            pManager.AddTextParameter("Loadcase", "Loadcase", "List of Loadcases to get results for", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Divisions", "Divisions", "Number of divisions along bar", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Activate", "Activate", "Run the component", GH_ParamAccess.item);
            Params.Input[1].Optional = true;
            Params.Input[2].Optional = true;
            Params.Input[3].Optional = true;
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (GHE.DataUtils.Run(DA, 4))
            {
                GSA_Adapter.Structural.Interface.GSAAdapter app = GHE.DataUtils.GetGenericData<GSA_Adapter.Structural.Interface.GSAAdapter>(DA, 0);
                if (app != null)
                {
                    List<string> ids = GHE.DataUtils.GetDataList<string>(DA, 1);
                    List<string> cases = GHE.DataUtils.GetDataList<string>(DA, 2);
                    int divisions = GHE.DataUtils.GetData<int>(DA, 3);
                    if (divisions == 0) divisions = 3;
                    Dictionary<string, BHR.IResultSet> results = new Dictionary<string, BHR.IResultSet>();
                    app.GetBarStrains(ids, cases, m_ResultOrder, out results);

                    SetResults<BHoM.Structural.Results.BarStrains>(DA, results);
                }
            }
        }
    }

}

