using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Grasshopper_Engine.Components;
using Grasshopper_Engine;
using BHE = BHoM.Structural.Elements;
using BHP = BHoM.Structural.Properties;

namespace Alligator.Structural.Elements.Design
{
    public class SetSectionPropertyToDesignElem : GH_Component
    {
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("267985B4-7048-4FF8-B4E3-80E433AF9CF7");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("DesignElement", "DesElem", "Design element update section property on", GH_ParamAccess.item);
            pManager.AddGenericParameter("SectionProperty", "SecProp", "The section property to set", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("DesignElement", "DesElem", "The updated design element", GH_ParamAccess.item);

        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHE.DesignElement elem = (BHE.DesignElement)DataUtils.GetData<BHE.DesignElement>(DA, 0).ShallowClone();
            BHP.SectionProperty prop = DataUtils.GetData<BHP.SectionProperty>(DA, 1);

            elem.SetSectionProperty(prop);

            DA.SetData(0, elem);
        }
    }
}
