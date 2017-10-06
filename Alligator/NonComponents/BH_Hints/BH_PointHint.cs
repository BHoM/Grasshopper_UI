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
    public class BH_PointHint : IGH_TypeHint
    {
        public Guid HintID { get { return new Guid("67b20827-0326-4442-a4bc-42cfc8d69674"); } }
        public string TypeName { get { return typeof(BH.oM.Geometry.Point).ToString(); } }
        public bool Cast(object data, out object target)
        {
            target = data;
            return true;
        }
    }
}
