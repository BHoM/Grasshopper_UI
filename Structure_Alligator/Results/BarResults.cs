using BH.UI.Alligator.Components;
using BH.oM.Structural.Results;
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
using System.Reflection;

namespace BH.UI.Alligator.Structural.Results
{
    public class GetBarForce : ResultBaseComponent<BarForce>
    {
        public GetBarForce() : base("GetBarForce", "GetBarForce", "Gets the bar forces from the selected result server", "Structure", "Results") { }
        
        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Structural.Properties.Resources.BHoM_GetBarForce; }
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("{E3FA67E7-0BA4-4661-A736-8A7D1948BEFA}");
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
                    Dictionary<string, BHI.IResultSet> results = new Dictionary<string, BHI.IResultSet>();
                    app.GetBarForces(ids, cases, divisions, m_ResultOrder, out results);

                    SetResults<BarForce>(DA, results);
                }
            }
        }
    }

    public class GetBarCoordinates : ResultBaseComponent<BarCoordinates>
    {
        public GetBarCoordinates() : base("GetBarCoordinates", "BarCoordinates", "Gets the bar coordinates from the selected result server", "Structure", "Results")
        {
            m_Options = null;
            m_EnvelopeOption = null;
            Message = "";
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Structural.Properties.Resources.BHoM_Get_Bar_Coordinates; }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("80B0B77A-F6CC-4D55-9A52-CFE3B9C83CF1");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Result Server", "ResultServer", "Application or Result server to extract results from", GH_ParamAccess.item);
            pManager.AddTextParameter("Ids", "Id", "List of object ids to get results for", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Activate", "Activate", "Run the component", GH_ParamAccess.item);
            Params.Input[1].Optional = true;
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (GHE.DataUtils.Run(DA, 2))
            {
                BHI.IResultAdapter app = GHE.DataUtils.GetGenericData<BHI.IResultAdapter>(DA, 0);
                if (app != null)
                {
                    List<string> ids = GHE.DataUtils.GetDataList<string>(DA, 1);
                    Dictionary<string, BHI.IResultSet> results = new Dictionary<string, BHI.IResultSet>();
                    app.GetBarCoordinates(ids, out results);

                    SetResults<BarCoordinates>(DA, results);
                }
            }
        }
    }
}
