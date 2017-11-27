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
using BH.Adapter.Rhinoceros;

namespace BH.UI.Alligator
{
    public class GH_Enum : GH_TemplateType<Enum>
    {
        public GH_Enum() : base() { }

        /***************************************************/

        public GH_Enum(Enum val) : base(val) { }

        /***************************************************/

        public override string TypeName
        {
            get { return ("Enum"); }
        }

        /***************************************************/

        public override string TypeDescription
        {
            get { return ("Defines an enum"); }
        }

        /***************************************************/

        public override IGH_Goo Duplicate()
        {
            return new GH_Enum { Value = Value };
        }

    }
}
