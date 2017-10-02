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

    public class CreateCompressionRingBeam : BHoMBaseComponent<BHE.CompressionRingBeam>
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public CreateCompressionRingBeam()
          : base("CompressionRingBeam", "CRBeam",
              "Beam element for compression ring, corresponding to Revit family", "Structure", "Cable net")
        { }

        public override Guid ComponentGuid

        {
            get
            {
                return new Guid("E71ACF30-ED1C-479C-9BD1-863F2D765277");
            }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return CableNetDesign_Alligator.Properties.Resources.CRBeam; }
        }
    }
}