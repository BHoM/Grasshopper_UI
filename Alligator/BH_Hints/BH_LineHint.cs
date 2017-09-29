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

namespace BH.UI.Alligator.Base
{
    public class BH_LineHint : IGH_TypeHint
    {
        public Guid HintID { get { return new Guid("e630327b-d9b4-41b8-8dc9-c74436e4bca9"); } }
        public string TypeName { get { return typeof(BH.oM.Geometry.Line).ToString(); } }
        public bool Cast(object data, out object target)
        {
            target = data;
            return true;
        }
    }
}
