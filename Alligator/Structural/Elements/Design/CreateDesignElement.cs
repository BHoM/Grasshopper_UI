using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Grasshopper_Engine;
using BHE = BH.oM.Structural.Elements;

namespace Alligator.Structural.Elements.Design
{
    public class CreateDesignElement : Grasshopper_Engine.Components.BHoMBaseComponent<BHE.DesignElement>
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
