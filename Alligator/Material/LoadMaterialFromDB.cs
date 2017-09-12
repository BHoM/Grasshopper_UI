using System;
using System.Linq;
using System.Text.RegularExpressions;
using Grasshopper.Kernel;
using Alligator.Components;
using System.Collections.Generic;
using BHE = BHoM.Structural.Elements;
using BHP = BHoM.Structural.Properties;
using Grasshopper_Engine.Components;
using System.Windows.Forms;
using Grasshopper.Kernel.Parameters;
using GH_IO.Serialization;
using Grasshopper.Kernel.Types;
using BHoM.Structural.Databases;

namespace Alligator.Material
{
    public class LoadMaterialFromDB : DatabaseComponent<MaterialRow>
    {
        public LoadMaterialFromDB() : base("Load Material", "MaterialDB", "Load a material from database", "Structure", "Properties")
        {
            this.Initialise(BHoM.Base.Data.Database.Material);
        }

        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_Material_List; }
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("{7860F7AD-A9BB-4886-A46F-C3A2A9437255}");
            }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.quarternary;
            }
        }

        protected override void SetData()
        {
            BHoM.Global.Project.ActiveProject.Config.MaterialDatabase = TableName;
            BHoM.Materials.Material prop = BHoM.Materials.Material.LoadFromDB(ObjectName);
            AddVolatileData(new Grasshopper.Kernel.Data.GH_Path(0), 0, new GH_ObjectWrapper(prop));
        }
        protected override void CollectVolatileData_Custom()
        {
            SetData();
        }
    }
}
