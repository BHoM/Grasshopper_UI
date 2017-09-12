using System;
using BH.oM.Structural;
using Grasshopper.Kernel;
using System.Collections.Generic;
using Grasshopper_Engine.Components;

namespace BH.UI.Grasshopper.SportVenueEvent
{
    public class PrecastSeatingPlank : BHoMBaseComponent<BH.oM.Structural.Elements.PrecastSeatingPlank>
    {
        public PrecastSeatingPlank() : base("Precast Seating Plank", "PrecastSeatingPlank", "Create a Precast seating plank", "SportVenueEvent", "Structural") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("5FE0E2C8-5E50-410F-BBC7-C255FD1BD2B7");
            }
        }

    }
}
