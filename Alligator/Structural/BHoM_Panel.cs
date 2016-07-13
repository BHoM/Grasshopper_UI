using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHoM.Structural;
using BHoM.Geometry;
using Grasshopper.Kernel;


namespace Alligator.Structural
{
    public class CreatePanel : BHoMBaseComponent<Panel>
    {
        public CreatePanel() : base("Create Panel", "CreatePanel", "Create a BH Panel object", "Alligator", "Structural") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("9E64C671-01BD-4D39-94D3-554BD2F8BA52");
            }
        }
    }

    public class SplitOpenings : GH_Component
    {
        public SplitOpenings() : base("SplitOpenings", "SplitOpenings", "Remove openings from the panel", "Alligator", "Structural") { }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Panel", "Panel", "BHoM Panel", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Panel", "Panel", "BHoM Panel", GH_ParamAccess.item);
            pManager.AddGenericParameter("Openings", "Openings", "BHoM Openings", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Getting the inputs from GH
            Panel panel = Utils.GetGenericData<Panel>(DA, 0);

            // Createing the panel without openings
            Panel newPanel = panel.ShallowClone() as Panel;
            Group<Curve> contour = new Group<Curve>();
            contour.Add(panel.External_Contour);
            newPanel.Edges = contour;

            // Getting the openings
            DA.SetData(0, newPanel);
            DA.SetDataList(1, panel.Internal_Contours);
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("3C7E971B-E08B-4605-A526-E6CB3248FFBC"); }
        }

    }
}
