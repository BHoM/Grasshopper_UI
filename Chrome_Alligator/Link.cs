using System;
using System.Collections.Generic;
using System.Linq;
using BH.UI.Alligator.Templates;
using Grasshopper.Kernel;
using BH.oM.Chrome.Links;

namespace BH.UI.Alligator.Chrome
{
    public class Link : CreateObjectTemplate
    {
        public Link() : base("ChromeLink", "Link", "Creates a link between data in Chrome", "Alligator", "Chrome")
        {
            m_MenuMaxDepth = 0;
        }
        public override Guid ComponentGuid { get { return new Guid("008A13CC-0ED9-4DD0-BFF7-C2C611173B5B"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }
        public override GH_Exposure Exposure { get { return GH_Exposure.primary; } }

        /*************************************/

        protected override IEnumerable<Type> GetRelevantTypes()
        {
            Type viewType = typeof(ILink);
            return BH.Engine.Reflection.Query.GetBHoMTypeList().Where(x => viewType.IsAssignableFrom(x)).OrderBy(x => x.Name);
        }
    }
}