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

namespace Alligator.Structural.Results
{
    public class BarUtilisation : ResultBaseComponent<SteelUtilisation>
    {
        public BarUtilisation() : base("GetBarUtilisation", "GetBarUtilisation", "Gets the bar utilisation from the selected result server", "Structure", "Results") { }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_GetBarForce; }
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("AB3AE2FA-EB57-4C59-ABF8-5F011D413F84");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Result Server", "ResultServer", "Application or Result server to extract results from", GH_ParamAccess.item);
            pManager.AddTextParameter("Ids", "Id", "List of object ids to get results", GH_ParamAccess.list);
            pManager.AddTextParameter("Loadcase", "Loadcase", "List of Loadcases to get results for", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Activate", "Activate", "Run the component", GH_ParamAccess.item);
            Params.Input[1].Optional = true;
            Params.Input[2].Optional = true;
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
                    app.GetBarUtilisation(ids, cases, m_ResultOrder, out results);

                    SetResults<SteelUtilisation>(DA, results);
                }
            }
        }
    }
}