using BH.UI.Alligator.Components;
using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHE = BH.Engine.Grasshopper;
using BHE = BH.oM.Structural.Elements;
using BHI = BH.oM.Structural.Interface;
using BHR = BH.oM.Structural.Results;
using BH.Engine.Grasshopper.Components;

namespace BH.UI.Alligator.Structural.Results
{
    public class BarUtilisation : ResultBaseComponent<BHR.SteelUtilisation>
    {
        public BarUtilisation() : base("GetBarUtilisation", "GetBarUtilisation", "Gets the bar utilisation from the selected result server", "Structure", "Results") { }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Structural.Properties.Resources.BHoM_Get_BarUtilisation; }
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
                    Dictionary<string, BHI.IResultSet> results = new Dictionary<string, BHI.IResultSet>();
                    app.GetBarUtilisation(ids, cases, m_ResultOrder, out results);

                    SetResults<BHR.SteelUtilisation>(DA, results);
                }
            }
        }
    }
}