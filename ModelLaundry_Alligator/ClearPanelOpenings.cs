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
    public class ClearPanelOpenings : GH_Component
    {
        public ClearPanelOpenings() : base("ClearOpenings", "ClearPanelOpenings", "Remove panel openings", "Alligator", "ModelLaundry") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("4E1197EE-5824-4C11-8F12-9CEB4506ABE7");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("panel", "panel", "panel to process", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("panel", "panel", "resulting panel", GH_ParamAccess.item);
            pManager.AddGenericParameter("openings", "openings", "removed openings", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHE.Panel panel = GHE.DataUtils.GetData<BHE.Panel>(DA, 0).ShallowClone() as BHE.Panel;
            List<BHG.Curve> openings = panel.Internal_Contours.ToList();
            panel.Internal_Contours = new BHG.Group<BHG.Curve>();

            DA.SetData(0, panel);
            DA.SetDataList(1, openings);
        }
    }
}
