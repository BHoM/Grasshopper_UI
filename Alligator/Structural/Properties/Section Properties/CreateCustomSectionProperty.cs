using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using GHE = Grasshopper_Engine;
using BHP = BHoM.Structural.Properties;
using BHG = BHoM.Geometry;
using R = Rhino.Geometry;
using Grasshopper_Engine.Components;
using System.Reflection;

namespace Alligator.Structural.Properties
{
    public class CreateCustomSectionProperty : BHoMBaseComponent<BHP.SteelSection>
    {
        public CreateCustomSectionProperty() : base("Custom Section Property", "SecProp", "Creates a custom section property from curves", "Structure", "Properties") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("23A9A2D6-4601-46B9-AAD5-B8273605ABCD");
            }
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "N", "Name Custom Section", GH_ParamAccess.item);
            pManager.AddCurveParameter("Edges", "E", "Custom section property constructed from edge curves", GH_ParamAccess.list);
            pManager.AddGenericParameter("Material", "M", "Material of Custom Section", GH_ParamAccess.item);

            AppendEnumOptions("Fabrication", typeof(BHP.Fabrication));
            AppendEnumOptions("Shape", typeof(BHP.ShapeType));
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SectionProperty", "Sec", "The created section property", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<R.Curve> crvs = new List<R.Curve>();

            if(!DA.GetDataList(1, crvs)) { return; }

            BHG.Group<BHG.Curve> edges = new BHG.Group<BHG.Curve>();
            string name = GHE.DataUtils.GetData<string>(DA, 0);
            BHoM.Materials.Material m = GHE.DataUtils.GetGenericData<BHoM.Materials.Material>(DA, 2); GHE.DataUtils.GetGenericData<BHoM.Materials.Material>(DA, 2); 
            foreach (R.Curve crv in crvs)
            {
                edges.Add(GHE.GeometryUtils.Convert(crv));
            }

            BHP.SteelSection section = BHP.SectionProperty.CreateCustomSection(m.Type, edges) as BHP.SteelSection;
            section.Name = name;
            section.Material = m;

            for (int i = 0; m_Options != null && i < m_Options.Count; i++)
            {
                PropertyInfo prop = typeof(BHP.SteelSection).GetProperty(m_Options[i].Name);
                prop.SetValue(section, m_SelectedOption[i]);
            }

            DA.SetData(0, section);
        }
    }
}
