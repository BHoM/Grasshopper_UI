using System;
using Grasshopper.Kernel.Parameters;

namespace BH.UI.Alligator.GeometryHints
{
    public class BH_NurbCurveHint : IGH_TypeHint
    {
        public Guid HintID { get { return new Guid("cbf9a9aa-471f-4a92-b8c0-f80096b7567b"); } }
        public string TypeName { get { return typeof(BH.oM.Geometry.NurbCurve).ToString(); } }
        public bool Cast(object data, out object target)
        {
            target = data;
            return true;
        }
    }
}
