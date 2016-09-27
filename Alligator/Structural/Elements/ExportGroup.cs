using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using GHE = Grasshopper_Engine;
using BHE = BHoM.Structural.Elements;
using BHI = BHoM.Structural.Interface;
using BHB = BHoM.Base;


namespace Alligator.Structural.Elements
{
    public class ExportGroup : GH_Component
    {
        public ExportGroup() : base("Export Group", "SetGroup", "Creates or Replaces groups", "Structure", "Elements") { }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "App", "Application to export groups to", GH_ParamAccess.item);
            pManager.AddGenericParameter("Groups", "G", "BHoM groups to export", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Activate", "Go", "Generate groups", GH_ParamAccess.item);

            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Ids", "Ids", "Bar Numbers", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (GHE.DataUtils.Run(DA, 2))
            {
                BHI.IElementAdapter app = GHE.DataUtils.GetGenericData<BHI.IElementAdapter>(DA, 0);
                if (app != null)
                {
                    List<BHB.IGroup> groups = GHE.DataUtils.GetGenericDataList<BHB.IGroup>(DA, 1);
                    List<string> ids = null;
                    app.SetGroups(groups, out ids);

                    DA.SetDataList(0, ids);
                }
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("E99F6CA7-A8F0-41DB-B3C5-E7C499B7A7C1"); }
        }


    }
}