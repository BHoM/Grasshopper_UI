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
    public class CreateTensionRing : BHoMBaseComponent<BHE.TensionRing>
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public CreateTensionRing()
          : base("TensionRingCable", "TRCable",
              "Tension Ring Cable that spans between Tension ring nodes", "Structure", "Elements")
        { }

        public override Guid ComponentGuid

        {
            get
            {
                return new Guid("306CF6D0-5C49-4AFE-BF50-96E4558B39B5");
            }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return CableNetDesign_Alligator.Properties.Resources.TRCable; }
        }

    }

}