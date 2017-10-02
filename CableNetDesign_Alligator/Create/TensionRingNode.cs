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
    public class CreateTensionRingNode : BHoMBaseComponent<BHE.TensionRingNode>
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public CreateTensionRingNode()
          : base("TensionRingNode", "TRNode",
              "A node between tension ring, radials and struts in a cable net roof", "Structure", "Cable net")
        { }

        public override Guid ComponentGuid

        {
            get
            {
                return new Guid("54489184-DDF4-4495-8F3C-C2AD3F1DB859");
            }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return CableNetDesign_Alligator.Properties.Resources.TRNode; }
        }

    }
 
}