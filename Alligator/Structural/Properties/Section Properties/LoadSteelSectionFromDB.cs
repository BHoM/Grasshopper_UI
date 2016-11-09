using Grasshopper.Kernel.Types;
using Grasshopper_Engine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHE = BHoM.Structural.Elements;
using BHP = BHoM.Structural.Properties;

namespace Alligator.Structural.Properties.Section_Properties
{
    public class LoadSteelSectionFromDB : DatabaseComponent
    {
        public LoadSteelSectionFromDB() : base("Load Steel Section", "SteelDB", "Load a steel section from database", "Structure", "Properties")
        {
            this.Initialise(BHoM.Base.Database.SteelSection);
        }

        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_Section_List; }
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("{2860F7AD-A9BB-4886-A46F-C3A2A9437258}");
            }
        }

        protected override void SetData()
        {
            BHoM.Global.Project.ActiveProject.Config.SectionDatabase = TableName;
            BHP.SectionProperty prop = BHP.SectionProperty.LoadFromSteelSectionDB(ObjectName);
            AddVolatileData(new Grasshopper.Kernel.Data.GH_Path(0), 0, new GH_ObjectWrapper(prop));
        }
        protected override void CollectVolatileData_Custom()
        {
            SetData();
        }
    }
}
