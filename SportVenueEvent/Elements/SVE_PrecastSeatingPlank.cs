using System;
using BHoM.Structural;
using Grasshopper.Kernel;
using System.Collections.Generic;

namespace Alligator.SportVenueEvent.Structural
{
    public class PrecastSeatingPlank : BHoMBaseComponent<SportVenueEventToolkit.Elements.ConcreteRakerBeam>
    {
        public PrecastSeatingPlank() : base("Precast Seating Plank", "PrecastSeatingPlank", "Create a Precast seating plank", "SportVenueEvent", "Structural") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("5FE0E2C8-5E50-410F-BBC7-C255FD1BD2B7");
            }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return SportVenueEvent.Properties.Resources.ConcreteRaker; }
        }
    }
}
