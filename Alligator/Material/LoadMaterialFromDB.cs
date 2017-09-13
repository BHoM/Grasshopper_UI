//using System;
//using System.Linq;
//using System.Text.RegularExpressions;
//using Grasshopper.Kernel;
//using BH.UI.Alligator.Components;
//using System.Collections.Generic;
//using BHE = BH.oM.Structural.Elements;
//using BHP = BH.oM.Structural.Properties;
//using BH.Engine.Grasshopper.Components;
//using System.Windows.Forms;
//using Grasshopper.Kernel.Parameters;
//using GH_IO.Serialization;
//using Grasshopper.Kernel.Types;
//using BH.oM.Structural.Databases;

//namespace BH.UI.Alligator.Material
//{
//    public class LoadMaterialFromDB : DatabaseComponent<MaterialRow>  //TODO: Needs to be redone to comply with BHoM 2.0
//    {
//        public LoadMaterialFromDB() : base("Load Material", "MaterialDB", "Load a material from database", "Structure", "Properties")
//        {
//            this.Initialise(BH.oM.Base.Data.Database.Material);
//        }

//        protected override System.Drawing.Bitmap Internal_Icon_24x24
//        {
//            get { return Alligator.Properties.Resources.BHoM_Material_List; }
//        }

//        public override Guid ComponentGuid
//        {
//            get
//            {
//                return new Guid("{7860F7AD-A9BB-4886-A46F-C3A2A9437255}");
//            }
//        }

//        public override GH_Exposure Exposure
//        {
//            get
//            {
//                return GH_Exposure.quarternary;
//            }
//        }

//        protected override void SetData()
//        {
//            BHoM.Global.Project.ActiveProject.Config.MaterialDatabase = TableName;
//            BHoM.Materials.Material prop = BHoM.Materials.Material.LoadFromDB(ObjectName);
//            AddVolatileData(new GH.Kernel.Data.GH_Path(0), 0, new GH_ObjectWrapper(prop));
//        }
//        protected override void CollectVolatileData_Custom()
//        {
//            SetData();
//        }
//    }
//}
