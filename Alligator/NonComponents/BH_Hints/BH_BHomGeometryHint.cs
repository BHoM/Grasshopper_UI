using System;
using Grasshopper.Kernel.Parameters;

namespace BH.UI.Alligator.GeometryHints
{
    public class BH_BHoMGeometryHint : IGH_TypeHint
    {
        public Guid HintID { get { return new Guid("CC64E37E-C6B8-44F4-9C85-05B19849F4D6"); } }
        public string TypeName { get { return "Geometry"; } }
        public bool Cast(object data, out object target)
        {
            GH_IBHoMGeometry geom = new GH_IBHoMGeometry() { Value = null };
            geom.CastFrom(data);
            if (geom.Value == null)
                target = data;
            else
                target = geom.Value;
            return true;
        }
    }
}
