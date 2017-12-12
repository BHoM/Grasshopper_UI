using System;
using System.Collections.Generic;
using System.Linq;
using BH.UI.Alligator.Templates;
using Grasshopper.Kernel;
using BH.oM.Chrome.Parts;
using BH.oM.Chrome.Dimensions;

namespace BH.UI.Alligator.Chrome
{
    public class Dimension : CreateObjectTemplate
    {
        public Dimension() : base("ChromeDimension", "Dimension", "Creates a specific dimension definition to be used in a view", "Alligator", "Chrome")
        {
            m_MenuMaxDepth = 0;
        }
        public override Guid ComponentGuid { get { return new Guid("F6579818-B00B-44F4-B6A9-A10408024B0B"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }
        public override GH_Exposure Exposure { get { return GH_Exposure.secondary; } }


        /*************************************/

        protected override IEnumerable<Type> GetRelevantTypes()
        {
            Type viewType = typeof(IDimension);
            return BH.Engine.Reflection.Query.GetBHoMTypeList().Where(x => viewType.IsAssignableFrom(x)).OrderBy(x => x.Name);
        }
    }
}