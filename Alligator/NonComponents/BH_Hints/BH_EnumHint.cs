using System;
using Grasshopper.Kernel.Parameters;

namespace BH.UI.Alligator.GeometryHints
{
    public class BH_EnumHint : IGH_TypeHint
    {
        public Guid HintID { get { return new Guid("50201E4F-F9F3-4BE5-A927-98AE2EE03530"); } }
        public string TypeName { get { return "Enum"; } }
        public bool Cast(object data, out object target)
        {
            target = data;
            return true;
        }
    }
}
