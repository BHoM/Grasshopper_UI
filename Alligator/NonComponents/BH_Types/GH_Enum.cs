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
    public class GH_Enum : GH_Goo<Enum>
    {
        public GH_Enum()
        {
            this.Value = null;
        }
        public GH_Enum(Enum value)
        {
            this.Value = value;
        }

        public override bool IsValid
        {
            get
            {
                if (Value == null) { return false; }
                return Value != null;
            }
        }

        public override IGH_Goo Duplicate()
        {
            return new GH_Enum { Value = Value };
        }

        public override string ToString()
        {
            if (Value == null)
                return "Undefined Enum";

            return Value.ToString();
        }
        public override string TypeName
        {
            get { return ("Enum"); }
        }
        public override string TypeDescription
        {
            get { return ("Defines an enum"); }
        }


        public override bool CastFrom(object source)
        {
            if (source == null) { return false; }
            else if (source.GetType() == typeof(GH_Goo<Enum>))
                this.Value = (Enum)source;
            else
                this.Value = (Enum)source;
            return true;
        }
        public override bool CastTo<Q>(ref Q target)
        {
            object ptr = this.Value;
            target = (Q)ptr;
            return true;
        }

        
    }
}
