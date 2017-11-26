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
    public class BH_BHoMObjectHint : IGH_TypeHint
    {
        public Guid HintID { get { return new Guid("0977C35E-92DD-4933-8835-8B2C8A37C8CF"); } }
        public string TypeName { get { return typeof(BH.oM.Base.BHoMObject).ToString(); } }
        public bool Cast(object data, out object target)
        {
            target = data;
            return true;
        }
    }
}
