using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using BH.Engine.Base;
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
