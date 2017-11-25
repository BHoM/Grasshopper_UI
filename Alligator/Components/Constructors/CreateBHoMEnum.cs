using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Base;
using BH.UI.Alligator.Templates;
using System.IO;
using System.Reflection;

namespace BH.UI.Alligator.Base
{
    public class CreateBHoMEnum : CreateEnumTemplate
    {
        public CreateBHoMEnum() : base("Create BHoM Enum", "BHoMEnum", "Creates a specific type of BHoM Enum", "Alligator", "Base")
        {
        }
        public override Guid ComponentGuid { get { return new Guid("68B29FAE-057B-417A-96BC-32224974CCBE"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }

        public override GH_Exposure Exposure { get { return GH_Exposure.primary; } }


        /*************************************/

        protected override IEnumerable<Type> GetRelevantEnums()
        {
            return BH.Engine.Reflection.Query.GetBHoMEnumList();
        }
  
    }
}