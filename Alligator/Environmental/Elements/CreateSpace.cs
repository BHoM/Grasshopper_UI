using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using GHE = Grasshopper_Engine;
using Alligator.Components;
using BHE = BHoM.Environmental.Elements;
using BHG = BHoM.Geometry;
using System.Windows.Forms;
using R = Rhino.Geometry;
using Grasshopper;
using GHKT = Grasshopper.Kernel.Types;
using Grasshopper_Engine.Components;
using ASP = Alligator.Structural.Properties;

namespace Alligator.Environmental.Elements
{
    public class CreateSpace : BHoMBaseComponent<BHE.Space>
    {

        public CreateSpace() : base("Create Space", "CreateSpace", "Create a BH Space object", "Alligator", "Environmental") { }

        public override Guid ComponentGuid
        {
            get { return new Guid("{89be84e2-2e50-48b7-bb28-8d11e9e0ff05}"); }
        }
    }
}