using System;
using Grasshopper.Kernel.Parameters;

namespace BH.UI.Alligator.GeometryHints
{
    public class BH_VectorHint : IGH_TypeHint
    {
        public Guid HintID { get { return new Guid("5c0e7f26-adba-4665-85fa-81e57a5b6a69"); } }
        public string TypeName { get { return typeof(BH.oM.Geometry.Vector).ToString(); } }
        public bool Cast(object data, out object target)
        {
            target = data;
            return true;
        }
    }
}
