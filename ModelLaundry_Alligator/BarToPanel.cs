using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using MLE = ModelLaundry_Engine;
using GHE = Grasshopper_Engine;
using BHE = BH.oM.Structural.Elements;


namespace ModelLaundry_Alligator
{
    public class BarToPanel : GH_Component
    {
        public BarToPanel() : base("BarToPanel", "BarToPanel", "Convert bars into panels", "Alligator", "ModelLaundry") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("42ACF2BC-60F9-4166-AA25-888DB36E3A8B");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("bars", "bars", "bars to be converted", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("panels", "panels", "resulting panels", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<BHE.Bar> bars = GHE.DataUtils.GetDataList<BHE.Bar>(DA, 0);
            List<BHE.Panel> panels = MLE.Structure.BarsToPanels(bars);
            DA.SetDataList(0, panels);
        }
    }
}
