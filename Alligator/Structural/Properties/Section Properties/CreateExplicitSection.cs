using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Structural.Properties;
using Grasshopper_Engine.Components;

namespace Alligator.Structural.Properties.Section_Properties
{
    public class CreateExplicitSection : BHoMBaseComponent<ExplicitSectionProperty>
    {

        public CreateExplicitSection() : base("Create Explicit Section", "CreateExplicitSection", "Create a BH Section property object", "Structure", "Properties")
        {

        }


        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("95933851-2b86-46ab-b4e3-d839b817dbb4");
            }
        }
    }
}
