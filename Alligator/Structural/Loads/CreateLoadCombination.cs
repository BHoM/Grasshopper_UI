using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper_Engine.Components;
using BHL = BH.oM.Structural.Loads;

namespace Alligator.Structural.Loads
{
    public class CreateLoadCombination : BHoMBaseComponent<BHL.LoadCombination>
    {
        public CreateLoadCombination() : base("Create Loadcombination", "LoadComination", "Create a BH Load combination object", "Structure", "Loads") { }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BH.oM_LoadCase_Combination; }
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("00DCE84D-AA71-4702-8971-A6B7A1305D69");
            }
        }
    }
}
