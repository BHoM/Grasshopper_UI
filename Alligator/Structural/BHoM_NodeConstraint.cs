using BHoM.Structural;
using System;

namespace Alligator.Structural.Elements
{
    public class CreateNodeConstraint : BHoMBaseComponent<NodeConstraint>
    {
        public CreateNodeConstraint() : base("Create Constraint", "CreateConstraint", "Create a BH Constraint object", "Alligator", "Structural") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("5FE0E2C4-5E10-410F-BBC7-C255FE1BD2B3");
            }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.constraint; }
        }
    }
}
