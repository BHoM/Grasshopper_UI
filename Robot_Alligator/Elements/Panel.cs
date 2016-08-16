using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using GHE = Grasshopper_Engine;
using BHI = BHoM.Structural.Interface;
using BHE = BHoM.Structural.Elements;


namespace Alligator.Robot.Elements
{
    public class SetPanel : GH_Component
    {
        public SetPanel() : base("Set Panel", "ExPanel", "Create a panel", "Robot", "Elements") { }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "Application", "Application to import panels from", GH_ParamAccess.item);
            pManager.AddGenericParameter("Panel", "Panels", "BHoM Panel", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Activate", "Activate", "Set Geometry", GH_ParamAccess.item);
            pManager[2].AddVolatileData(new Grasshopper.Kernel.Data.GH_Path(0), 0, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Ids", "Ids", "Panel Id", GH_ParamAccess.list); ;
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (GHE.DataUtils.Run(DA, 2))
            {
                BHI.IElementAdapter app = GHE.DataUtils.GetGenericData<BHI.IElementAdapter>(DA, 0);
                if (app != null)
                {
                    List<BHE.Panel> panels = GHE.DataUtils.GetGenericDataList<BHE.Panel>(DA, 1);
                    List<string> ids = null;
                    app.SetPanels(panels, out ids);

                    DA.SetDataList(0, ids);
                }
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("61B66CED-C4D5-45F1-8EB4-A6AA3EDD6C88"); }
        }

        
    }
}
