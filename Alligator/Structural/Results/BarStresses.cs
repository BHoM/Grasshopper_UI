using Alligator.Components;
using BHoM.Structural.Results;
using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHE = Grasshopper_Engine;
using BHE = BHoM.Structural.Elements;
using BHI = BHoM.Structural.Interface;
using BHR = BHoM.Base.Results;
using Grasshopper_Engine.Components;
using System.Reflection;

namespace Alligator.Structural.Results
{
    public class GetBarStress : ResultBaseComponent<BarStress>
    {
        public GetBarStress() : base("GetBarStress", "GetBarStress", "Gets the bar stresses from the selected result server", "Structure", "Results") { }

        ///// <summary> Icon (24x24 pixels)</summary>
        //protected override System.Drawing.Bitmap Internal_Icon_24x24
        //{
        //    get { return Alligator.Properties.Resources.BHoM_GetBarForce; }
        //}

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("4D5C57CE-5E34-4E84-98CA-6BC58A84F1CC");

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
                BHI.IResultAdapter app = GHE.DataUtils.GetGenericData<BHI.IResultAdapter>(DA, 0);
                if (app != null)
                {
                    List<string> ids = GHE.DataUtils.GetDataList<string>(DA, 1);
                    List<string> cases = GHE.DataUtils.GetDataList<string>(DA, 2);
                    int divisions = GHE.DataUtils.GetData<int>(DA, 3);
                    if (divisions == 0) divisions = 3;
                    Dictionary<string, BHR.IResultSet> results = new Dictionary<string, BHR.IResultSet>();
                    // app.GetBarForces(ids, cases, divisions, m_ResultOrder, out results);
                    app.GetBarStresses(ids, cases, divisions, m_ResultOrder, out results);

                    SetResults<BarStress>(DA, results);
                }
            }
        }
    }
}