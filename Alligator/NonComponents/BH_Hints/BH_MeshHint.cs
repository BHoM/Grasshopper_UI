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
    public class BH_MeshHint : IGH_TypeHint
    {
        public Guid HintID { get { return new Guid("66457d29-e0ab-410a-9159-a51015d2923c"); } }
        public string TypeName { get { return typeof(BH.oM.Geometry.Mesh).ToString(); } }
        public bool Cast(object data, out object target)
        {
            target = data;
            return true;
        }
    }
}
