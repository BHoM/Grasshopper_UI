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
