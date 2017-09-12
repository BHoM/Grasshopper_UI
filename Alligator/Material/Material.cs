using BH.oM.Structural;
using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using Alligator.Components;
using BHM = BH.oM.Materials;
using Grasshopper_Engine.Components;

namespace Alligator.Materials
{
    public class CreateMaterial : BHoMBaseComponent<BHM.Material>
    {
        public CreateMaterial() : base("Create Material", "CreateMaterial", "Create a BH Material object", "Structure", "Properties") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("c62239cc-3bf7-42cc-9027-866f46e1afd2");
            }
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.quarternary;
            }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BH.oM_Material; }
        }
    }


}
