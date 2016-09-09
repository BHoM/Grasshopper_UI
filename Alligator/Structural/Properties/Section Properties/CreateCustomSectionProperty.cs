using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using GHE = Grasshopper_Engine;
using BHP = BHoM.Structural.Properties;
using BHG = BHoM.Geometry;
using R = Rhino.Geometry;


namespace Alligator.Structural.Properties
{
    public class CreateCustomSectionProperty : GH_Component
    {
        public CreateCustomSectionProperty() : base("Custom Section Property", "SecProp", "Creates a custom section property from curves", "Structure", "Properties") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("23A9A2D6-4601-46B9-AAD5-B8273605ABCD");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Edges", "E", "Custom section property constructed from edge curves", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SectionProperty", "Sec", "The created section property", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<R.Curve> crvs = new List<R.Curve>();

            if(!DA.GetDataList(0, crvs)) { return; }

            BHG.Group<BHG.Curve> edges = new BHG.Group<BHG.Curve>();

            foreach (R.Curve crv in crvs)
            {
                edges.Add(GHE.GeometryUtils.Convert(crv));
            }

            BHP.SectionProperty prop = new BHP.SectionProperty(edges, BHP.ShapeType.Polygon);

            DA.SetData(0, prop);
        }
    }
}
