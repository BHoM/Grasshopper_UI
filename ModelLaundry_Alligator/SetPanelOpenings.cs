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
            pManager.AddGenericParameter("panels", "panels", "panels to process", GH_ParamAccess.list);
            pManager.AddGenericParameter("openings", "openings", "openings to inject into panels", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("panel", "panel", "resulting panel", GH_ParamAccess.list);  
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<BHE.Panel> panels = GHE.DataUtils.GetDataList<BHE.Panel>(DA, 0);
            List<BHG.Curve> openings = GHE.DataUtils.GetDataList<BHG.Curve>(DA, 1);

            foreach (BHE.Panel panel in panels)
            {
                BHG.Group<BHG.Curve> matching = new BHG.Group<BHG.Curve>();
                foreach (BHG.Curve external in panel.External_Contours)
                {
                    foreach (BHG.Curve opening in openings)
                    {
                        if (external.ContainsPoints(new List<BHG.Point> { opening.Bounds().Centre }))
                            matching.Add(opening);
                    }
                }
                panel.Internal_Contours = matching;

            }

            DA.SetDataList(0, panels);
        }
    }
}
