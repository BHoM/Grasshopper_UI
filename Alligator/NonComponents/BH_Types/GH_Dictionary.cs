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
using System.Collections;

namespace BH.UI.Alligator
{
    public class GH_Dictionary : GH_TemplateType<IDictionary>
    {
        public GH_Dictionary() : base() { }

        /***************************************************/

        public GH_Dictionary(IDictionary val) : base(val) { }

        /***************************************************/

        public override string TypeName
        {
            get { return ("Dictionary"); }
        }

        /***************************************************/

        public override string TypeDescription
        {
            get { return ("Defines an Dictionary"); }
        }

        /***************************************************/

        public override IGH_Goo Duplicate()
        {
            return new GH_Dictionary { Value = Value };
        }

        

        
    }
}
