using BHoM.Base.Results;
using BHoM.Structural.Elements;
using BHoM.Structural.Interface;
using BHoM.Structural.Results;
using StructuralDesign_Toolkit;
using StructuralDesign_Toolkit.Steel;
using StructuralDesign_Toolkit.Steel.Eurocode1993;
using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Design_Alligator.Structural.Steel
{
    public class SteelDesign : ElementDesignBase<SteelElementDesign, SteelUtilisation>
    {
        public SteelDesign() : base("Steel Design", "SteelDesign", "Design a list of bars based on input forces", "Structure", "Design") { }
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("{29380BC6-B24D-4145-A838-82FBAA4734A0}");
            }
        }
    }
}