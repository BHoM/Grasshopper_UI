using BH.oM.Structural.Properties;
using Grasshopper_Engine.Components;
using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alligator.Structural.Properties.Section_Properties
{
    public class CreateCompositeSection: BHoMBaseComponent<CompositeSection>
    {
        public CreateCompositeSection() : base("Create Composite Section", "CreateCompositeSection", "Create a BH Composite Section property object", "Structure", "Properties")
        {

        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("95916999-2b86-46ab-b4e3-d839b817dbb4");
            }
        }
        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BH.oM_Composite_Section; }
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }
    }
}

