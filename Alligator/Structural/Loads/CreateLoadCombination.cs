using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper_Engine.Components;
using BHL = BHoM.Structural.Loads;

namespace Alligator.Structural.Loads
{
    public class CreateLoadCombination : BHoMBaseComponent<BHL.LoadCombination>
    {
        public CreateLoadCombination() : base("Create Loadcombination", "LoadComination", "Create a BH Load combination object", "Structure", "Loads") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("00DCE84D-AA71-4702-8971-A6B7A1305D69");
            }
        }
    }
}
