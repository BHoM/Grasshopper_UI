using Grasshopper.Kernel.Types;
using System;

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
