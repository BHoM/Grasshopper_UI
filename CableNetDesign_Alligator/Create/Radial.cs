using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using GHE = Grasshopper_Engine;
using BHE = BHoM.Structural.Elements;
using BHI = BHoM.Structural.Interface;
using BHoM.Structural;
using Rhino.Geometry;
using Grasshopper;
using Grasshopper_Engine.Components;

namespace Alligator.Structural.Elements
{
    public class CreateRadial : BHoMBaseComponent<BHE.Radial>
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public CreateRadial()
          : base("RadialCable", "Radial",
              "Radial Cable that spans between compressionr ring and Tension ring nodes", "Structure", "Cable net")
        { }

        public override Guid ComponentGuid

        {
            get
            {
                return new Guid("CF12E7D1-545D-4FBC-B96D-E35019BECB47");
            }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return CableNetDesign_Alligator.Properties.Resources.Radial; }
        }

    }

}