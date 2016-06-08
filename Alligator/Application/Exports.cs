using BHoM.Structural;
using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alligator.Application
{
    public class SetNode : GH_Component
    {
        public SetNode() : base("Set Node", "ExNode", "Create a node", "Alligator", "Geometry") { }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "App", "Application to import nodes from", GH_ParamAccess.item);
            pManager.AddGenericParameter("Node", "N", "BHoM Node", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Execute", "R", "Set Geometry", GH_ParamAccess.item);
            pManager[2].AddVolatileData(new Grasshopper.Kernel.Data.GH_Path(0), 0, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Id", "Id", "Node Id", GH_ParamAccess.list); ;
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (Utils.Run(DA, 2))
            {
                IStructuralAdapter app = Utils.GetGenericData<IStructuralAdapter>(DA, 0);
                if (app != null)
                {
                    List<Node> nodes = Utils.GetGenericDataList<Node>(DA, 1);
                    List<string> ids = null;
                    app.SetNodes(nodes, out ids);

                    DA.SetDataList(0, ids);
                }
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("322adc1f-86b4-491a-93fa-1495515ca5aa"); }
        }
    }

    public class SetBar : GH_Component
    {
        public SetBar() : base("Set Bar", "ExBar", "Creates or Replaces the geometry of a Bar", "Alligator", "Geometry") { }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "App", "Application to import nodes from", GH_ParamAccess.item);
            pManager.AddGenericParameter("Bars", "B", "BHoM bars to export", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Execute", "R", "Generate Bars", GH_ParamAccess.item);

            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("Ids", "Ids", "Bar Numbers", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (Utils.Run(DA, 2))
            {
                IStructuralAdapter app = Utils.GetGenericData<IStructuralAdapter>(DA, 0);
                if (app != null)
                {
                    List<Bar> bars = Utils.GetGenericDataList<Bar>(DA, 1);
                    List<string> ids = null;
                    app.SetBars(bars, out ids);

                    DA.SetDataList(0, ids);
                }
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("2220dc1b-87b3-491a-93fa-1495315ca5a2"); }
        }
    }
}
