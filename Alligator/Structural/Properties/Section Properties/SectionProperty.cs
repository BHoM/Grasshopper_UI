using System;
using Grasshopper.Kernel;
using Alligator.Components;
using System.Collections.Generic;
using BHE = BHoM.Structural.Elements;
using BHP = BHoM.Structural.Properties;
using Grasshopper_Engine.Components;

namespace Alligator.Structural.Properties
{
    public class CreateSectionProperty : BHoMBaseComponent<BHP.SectionProperty>
    {
        public CreateSectionProperty() : base("Create Section Property", "CreateSectionProperty", "Create a BH Section property object", "Structure", "Properties") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("95916851-2b86-46ab-b4e3-d839b817dbb4");
            }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.sectionproperty; }
        }
    }

 
}
