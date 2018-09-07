using Grasshopper.Kernel.Types;
using System;

namespace BH.Engine.Alligator.Objects
{
    public class GH_Enum : GH_BHoMGoo<Enum>
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override string TypeName { get; } = "Enum";

        public override string TypeDescription { get; } = "Defines an enum";

        public override bool IsValid { get { return Value != null; } }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public GH_Enum() : base() { }

        /***************************************************/

        public GH_Enum(Enum val) : base(val) { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override IGH_Goo Duplicate()
        {
            return new GH_Enum { Value = Value };
        }

        /*******************************************/

        public override string ToString()
        {
            Enum val = Value;
            if (val == null)
                return "null";
            else
                return val.ToString();
        }

        /*******************************************/
    }
}
