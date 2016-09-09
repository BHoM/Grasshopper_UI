﻿using BHoM.Structural;
using System;
using Alligator.Components;
using BHP = BHoM.Structural.Properties;
using Grasshopper.Kernel;
using Grasshopper_Engine.Components;

namespace Alligator.Structural.Properties
{
    public class CreateBarConstraint : BHoMBaseComponent<BHP.BarConstraint>
    {
        public CreateBarConstraint() : base("Create  bar Constraint", "BarConstraint", "Create a BH Constraint object", "Structure", "Properties")
        {

        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("5FE0E2C4-5E10-410F-BBC7-C333FE1BD2B3");
            }
        }


    }
}