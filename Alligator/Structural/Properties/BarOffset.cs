using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHE = Grasshopper_Engine;
using BHG = BHoM.Geometry;
using BHE = BHoM.Structural.Elements;
using BHI = BHoM.Structural.Interface;
using BHP = BHoM.Structural.Properties;
using Grasshopper_Engine.Components;

namespace Alligator.Structural.Properties
{
    public class BarOffset : BHoMBaseComponent<BHP.Offset>
    {
        public BarOffset() : base("Create Bar Offset", "BarOffset", "Create a BH bar offset object", "Structure", "Properties") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("9E64C678-01BD-4D39-94D3-554BD2F8BA52");
            }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_BarOffsets; }
        }
    }
}
