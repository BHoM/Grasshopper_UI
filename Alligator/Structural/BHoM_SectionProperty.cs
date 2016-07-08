using BHoM.Structural;
using BHoM.Structural.SectionProperties;
using System;
using Grasshopper.Kernel;
using System.Collections.Generic;

namespace Alligator.Structural.Elements
{
    public class CreateSectionProperty : BHoMBaseComponent<SectionProperty>
    {
        public CreateSectionProperty() : base("Create Section Property", "CreateSectionProperty", "Create a BH Section property object", "Alligator", "Structural") { }

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
