using System;
using System.Collections.Generic;
using System.Linq;
using BH.UI.Alligator.Templates;
using Grasshopper.Kernel;

namespace BH.UI.Alligator.Chrome
{
    public class View : CreateObjectTemplate
    {
        public View() : base("ChromeView", "View", "Creates a specific view for data sent to Chrome", "Alligator", "Chrome")
        {
            m_MenuMaxDepth = 0;
        }
        public override Guid ComponentGuid { get { return new Guid("1D91BF89-3111-40B0-A1F5-19460941493F"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }
        public override GH_Exposure Exposure { get { return GH_Exposure.primary;  } }

        /*************************************/

        protected override IEnumerable<Type> GetRelevantTypes()
        {
            Type viewType = typeof(View);
            return BH.Engine.Reflection.Query.BHoMTypeList().Where(x => viewType.IsAssignableFrom(x)).OrderBy(x => x.Name);
        }
    }
}