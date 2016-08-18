using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using GHE = Grasshopper_Engine;
using BHG = BHoM.Geometry;
using BHE = BHoM.Structural.Elements;

namespace ModelLaundry_Alligator
{
    public class SetPanelOpenings : GH_Component
    {
        public SetPanelOpenings() : base("SetOpenings", "SetPanelOpenings", "Set panel openings", "Alligator", "ModelLaundry") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("561D7AD3-8750-4A62-B827-F7D8E7D3AE9F");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("panel", "panel", "panel to process", GH_ParamAccess.item);
            pManager.AddGenericParameter("openings", "openings", "removed openings", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("panel", "panel", "resulting panel", GH_ParamAccess.item);  
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHE.Panel panel = GHE.DataUtils.GetData<BHE.Panel>(DA, 0);
            List<BHG.Curve> openings = GHE.DataUtils.GetDataList<BHG.Curve>(DA, 1);
            panel.Internal_Contours = new BHG.Group<BHG.Curve>(openings);

            DA.SetData(0, panel);
        }
    }
}
