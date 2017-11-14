using System;
using System.Collections.Generic;
using System.Linq;
using BH.UI.Alligator.Templates;
using Grasshopper.Kernel;
using BH.oM.Chrome.Parts;

namespace BH.UI.Alligator.Chrome
{
    public class Domain : CreateObjectTemplate
    {
        public Domain() : base("ChromeDomain", "Domain", "Creates a specific domain definition to be used by a dimension", "Alligator", "Chrome") { }
        public override Guid ComponentGuid { get { return new Guid("E258EC5C-9D91-4039-9960-BAADD2926B09"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }
        public override GH_Exposure Exposure { get { return GH_Exposure.secondary; } }

        /*************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Domain", "Domain", "Domain", GH_ParamAccess.item);
        }

        /*************************************/

        protected override IEnumerable<Type> GetRelevantTypes()
        {
            Type viewType = typeof(IDomain);
            return BH.Engine.Reflection.Query.GetBHoMTypeList().Where(x => viewType.IsAssignableFrom(x)).OrderBy(x => x.Name);
        }
    }
}