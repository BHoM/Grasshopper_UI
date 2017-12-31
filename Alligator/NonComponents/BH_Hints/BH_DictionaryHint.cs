using System;
using Grasshopper.Kernel.Parameters;

namespace BH.UI.Alligator.GeometryHints
{
    public class BH_DictionaryHint : IGH_TypeHint
    {
        public Guid HintID { get { return new Guid("1574563B-80AC-486D-B175-A4F2E5EC76D5"); } }
        public string TypeName { get { return "Dictionary"; } }
        public bool Cast(object data, out object target)
        {
            target = data;
            return true;
        }
    }
}
