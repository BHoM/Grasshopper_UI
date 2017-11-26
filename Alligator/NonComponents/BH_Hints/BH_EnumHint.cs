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
using System.Collections;

namespace BH.UI.Alligator.GeometryHints
{
    public class BH_EnumHint : IGH_TypeHint
    {
        public Guid HintID { get { return new Guid("50201E4F-F9F3-4BE5-A927-98AE2EE03530"); } }
        public string TypeName { get { return "Enum"; } }
        public bool Cast(object data, out object target)
        {
            target = data;
            return true;
        }
    }
}
