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
    public class BH_BHoMGeometryHint : IGH_TypeHint
    {
        public Guid HintID { get { return new Guid("CC64E37E-C6B8-44F4-9C85-05B19849F4D6"); } }
        public string TypeName { get { return "Geometry"; } }
        public bool Cast(object data, out object target)
        {
            target = data;
            return true;
        }
    }
}
