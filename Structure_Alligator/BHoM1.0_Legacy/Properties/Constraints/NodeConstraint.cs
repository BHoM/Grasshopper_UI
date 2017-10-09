using BH.oM.Structural;
using System;
using BH.UI.Alligator.Components;
using BHP = BH.oM.Structural.Properties;
using Grasshopper.Kernel;
using BH.Engine.Grasshopper.Components;

namespace BH.UI.Alligator.Structural.Properties
{
    public class CreateNodeConstraint : BHoMBaseComponent<BHP.NodeConstraint>
    {
        public CreateNodeConstraint() : base("Create Constraint", "CreateConstraint", "Create a BH Constraint object", "Structure", "Properties")
        {

        }

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
            get { return Alligator.Structural.Properties.Resources.BHoM_Constraint; }
        }
    }
}
