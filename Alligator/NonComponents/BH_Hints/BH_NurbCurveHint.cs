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

namespace BH.UI.Alligator
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
