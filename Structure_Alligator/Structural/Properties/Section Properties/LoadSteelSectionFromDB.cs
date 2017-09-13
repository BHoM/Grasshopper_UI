//using Grasshopper.Kernel.Types;
//using Grasshopper.Kernel;
//using BH.Engine.Grasshopper.Components;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using BHE = BH.oM.Structural.Elements;
//using BHP = BH.oM.Structural.Properties;
//using BH.oM.Structural.Databases;

//namespace BH.UI.Alligator.Structural.Properties.Section_Properties          //TODO: Needs to be redone to comply with BHoM 2.0
//{
//    public class LoadSteelSectionFromDB : DatabaseComponent<SteelSectionRow>
//    {
//        public LoadSteelSectionFromDB() : base("Load Steel Section", "SteelDB", "Load a steel section from database", "Structure", "Properties")
//        {
//            this.Initialise(BH.oM.Base.Data.Database.SteelSection);
//        }

//        protected override System.Drawing.Bitmap Internal_Icon_24x24
//        {
//            get { return Alligator.Structural.Properties.Resources.BHoM_Section_List; }
//        }

//        public override GH_Exposure Exposure
//        {
//            get
//            {
//                return GH_Exposure.secondary;
//            }
//        }

//        public override Guid ComponentGuid
//        {
//            get
//            {
//                return new Guid("{2860F7AD-A9BB-4886-A46F-C3A2A9437258}");
//            }
//        }

//        protected override void SetData()
//        {
//            BHoM.Global.Project.ActiveProject.Config.SectionDatabase = TableName;
//            BHP.SectionProperty prop = BHP.SectionProperty.LoadFromSteelSectionDB(ObjectName);
//            AddVolatileData(new GH.Kernel.Data.GH_Path(0), 0, new GH_ObjectWrapper(prop));
//        }

//        protected override void CollectVolatileData_Custom()
//        {
//            SetData();
//            base.CollectVolatileData_Custom();
//        }
//    }
//}
