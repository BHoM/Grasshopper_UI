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
        
        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_GetNodeDisplacement; }
        }

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
        
        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_GetNodeReaction; }
        }

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

    public class GetNodeCoordinates : ResultBaseComponent<NodeCoordinates>
    {
        public GetNodeCoordinates() : base("GetNodeCoordinates", "NodeCoordinates", "Gets the node coordinates from the selected result server", "Structure", "Results")
        {
            m_Options = null;
            m_EnvelopeOption = null;
            Message = "";
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_Get_Node_Coordinates; }
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
                return new Guid("{E3FA6887-0BA4-4661-A777-8A7D1948BEFA}");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Result Server", "ResultServer", "Application or Result server to extract results from", GH_ParamAccess.item);
            pManager.AddTextParameter("Ids", "Id", "List of object ids to get results for", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Activate", "Activate", "Run the component", GH_ParamAccess.item);
            Params.Input[1].Optional = true;
            //  Params.Input[1].
            //  Params.Input[2].AddVolatileData(new GH_Path(0), 0, null);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (GHE.DataUtils.Run(DA, 2))
            {
                BHI.IResultAdapter app = GHE.DataUtils.GetGenericData<BHI.IResultAdapter>(DA, 0);
                if (app != null)
                {
                    List<string> ids = GHE.DataUtils.GetDataList<string>(DA, 1);
                    Dictionary<string, BHR.IResultSet> results = new Dictionary<string, BHR.IResultSet>();
                    app.GetNodeCoordinates(ids, out results);

                    SetResults<NodeCoordinates>(DA, results);
                }
            }
        }
    }
}
