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

    public class CreateRafterBeam : BHoMBaseComponent<BHE.RafterBeam>
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public CreateRafterBeam()
          : base("RafterBram", "Rafter",
              "Beam element for rafter, corresponding to Revit family", "Structure", "Cable net")
        { }

        public override Guid ComponentGuid

        {
            get
            {
                return new Guid("0BAA6876-D9FC-4740-8F3F-FB9C6355CEAC");
            }
        }
       
        
        ///// <summary> Icon (24x24 pixels)</summary>
        //protected override System.Drawing.Bitmap Internal_Icon_24x24
        //{
        //    get { return CableNetDesign_Alligator.Properties.Resources.CRBeam; }
        //}
    }
}