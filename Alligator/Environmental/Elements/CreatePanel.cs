using BHoM.Environmental;
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
    public class CreatePanel : BHoMBaseComponent<BHE.Panel>
    {
        public CreatePanel() : base("Create Panel", "CreatePanel", "Create a BH Panel object", "Alligator", "Environmental") { }

        public override Guid ComponentGuid
        {
            get { return new Guid("{df8f20ab-72e1-4247-ac0b-fdcff94d7124}"); }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_Panel; }
        }
    }

}