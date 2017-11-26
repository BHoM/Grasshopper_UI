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
    public class GH_Dictionary : GH_Goo<IDictionary>
    {
        public GH_Dictionary()
        {
            this.Value = null;
        }
        public GH_Dictionary(IDictionary value)
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
            return new GH_Dictionary { Value = Value };
        }

        public override string ToString()
        {
            if (Value == null)
                return "Undefined Dictionary";

            return Value.ToString();
        }
        public override string TypeName
        {
            get { return ("Dictionary"); }
        }
        public override string TypeDescription
        {
            get { return ("Defines an Dictionary"); }
        }


        public override bool CastFrom(object source)
        {
            if (source == null) { return false; }
            else if (source.GetType() == typeof(GH_Goo<IDictionary>))
                this.Value = (IDictionary)source;
            else
                this.Value = (IDictionary)source;
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
