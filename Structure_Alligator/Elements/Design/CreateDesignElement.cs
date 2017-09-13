using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BH.Engine.Grasshopper;
using BHD = BH.oM.Structural.Design;

namespace BH.UI.Alligator.Structural.Elements.Design
{
    public class CreateDesignElement : BH.Engine.Grasshopper.Components.BHoMBaseComponent<BHD.StructuralLayout> //TODO: CreateDesignElement is using StructuralLayout class. It needs clarification
    {
        public CreateDesignElement() : base("Create Design Element", "CreateDesElem", "Create a BH Design element object", "Structure", "Elements") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("B0F19E28-4082-4D71-BA94-3CF0FE17F988");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            base.RegisterInputParams(pManager);
            for (int i = 1; i < 5; i++)
            {
                pManager[i].Optional = true;
            }
        }

    }
}
