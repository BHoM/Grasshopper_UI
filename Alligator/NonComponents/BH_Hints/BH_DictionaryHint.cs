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
    public class BH_DictionaryHint : IGH_TypeHint
    {
        public Guid HintID { get { return new Guid("1574563B-80AC-486D-B175-A4F2E5EC76D5"); } }
        public string TypeName { get { return "Dictionary"; } }
        public bool Cast(object data, out object target)
        {
            target = data;
            return true;
        }
    }
}
