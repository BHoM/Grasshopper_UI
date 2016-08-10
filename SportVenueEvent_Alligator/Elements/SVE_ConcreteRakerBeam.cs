using System;
using BHoM.Structural;
using Grasshopper.Kernel;
using System.Collections.Generic;

namespace Alligator.SportVenueEvent.Structural
{
    public class ConcreteRakerBeam : BHoMBaseComponent<SportVenueEventToolkit.Elements.ConcreteRakerBeam>
    {
        public ConcreteRakerBeam() : base("Concrete raker beam", "ConcreteRakerBeam", "Create a BH Concrete raker beam object", "SportVenueEvent", "Structural") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("5FE0E2C4-5E50-410F-BBC7-C255FD1BD2B6");
            }
        }
        

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return SportVenueEvent.Properties.Resources.ConcreteRaker; }
        }
    }
}
