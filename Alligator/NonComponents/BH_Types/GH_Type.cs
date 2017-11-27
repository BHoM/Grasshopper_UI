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
    public class GH_Type : GH_TemplateType<Type>
    {
        public GH_Type() : base() { }

        /***************************************************/

        public GH_Type(Type val) : base(val) { }

        /***************************************************/

        public override string TypeName
        {
            get { return ("Type"); }
        }

        /***************************************************/

        public override string TypeDescription
        {
            get { return ("Defines an object Type"); }
        }

        /***************************************************/

        public override IGH_Goo Duplicate()
        {
            return new GH_Type { Value = Value };
        }

        /***************************************************/

        public override bool CastFrom(object source)
        {
            if (source == null) { return false; }
            else if (source is string)
                this.Value = BH.Engine.Reflection.Create.Type(source as string);
            else if (source is GH_String)
                this.Value = BH.Engine.Reflection.Create.Type(((GH_String)source).Value);
            else if (source.GetType() == typeof(GH_Goo<Type>))
                this.Value = (Type)source;
            else
                this.Value = (Type)source;
            return true;
        }

    }
}
