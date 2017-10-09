using BHoM.Structural.Results;
using Grasshopper.Kernel;
using StructuralDesign_Toolkit.Composite;
using StructuralDesign_Toolkit.Composite.Eurocode1994_2004;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Design_Alligator.Structural
{
    public class CompositeDesign : ElementDesignBase<CompositeElementDesign, CompositeUtilisation>
    {
        public CompositeDesign() : base("Composite Design", "CompositeDesign", "Design a list of bars based on input forces", "Structure", "Design") { }
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("{29380B55-B24D-4145-A838-82FBAA4734A0}");
            }
        }
    }
}
