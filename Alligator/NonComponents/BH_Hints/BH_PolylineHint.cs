using System;
using Grasshopper.Kernel.Parameters;

namespace BH.UI.Alligator.GeometryHints
{
    public class BH_PolylineHint : IGH_TypeHint
    {
        public Guid HintID { get { return new Guid("bee5c313-57ad-44d0-bd36-30f390261b9f"); } }
        public string TypeName { get { return typeof(BH.oM.Geometry.Polyline).ToString(); } }
        public bool Cast(object data, out object target)
        {
            target = data;
            return true;
        }
    }
}
