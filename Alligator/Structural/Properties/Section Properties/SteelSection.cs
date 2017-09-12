using BHoM.Structural.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;

namespace Alligator.Structural.Properties.Section_Properties
{
    public class CreateSteelSection : CreateSectionProperty<SteelSection>
    {
        public CreateSteelSection() : base("Create Steel Section", "CreateSteelSection", "Create a BH Section property object", "Structure", "Properties")
        {

        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            base.RegisterInputParams(pManager);

            AppendEnumOptions("Fabrication", typeof(Fabrication));
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("95916851-2b86-46ab-b4e3-d839b817dbb4");
            }
        }
    }
}
