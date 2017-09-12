using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using GHE = Grasshopper_Engine;
using BHE = BH.oM.Structural.Elements;
using BHI = BH.oM.Structural.Interface;
using BH.oM.Structural;
using Rhino.Geometry;
using Grasshopper;
using Grasshopper_Engine.Components;

namespace Alligator.Structural.Elements
{
    public class CreateTensionRing : BH.oMBaseComponent<BHE.TensionRing>
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public CreateTensionRing()
          : base("TensionRingCable", "TRCable",
              "Tension Ring Cable that spans between Tension ring nodes", "Structure", "Cable net")
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