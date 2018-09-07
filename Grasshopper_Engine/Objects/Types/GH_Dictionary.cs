using Grasshopper.Kernel.Types;
using System.Collections;
using System;
using System.Linq;
using BH.Engine.Reflection.Convert;

namespace BH.Engine.Alligator.Objects
{
    public class GH_Dictionary : GH_BHoMGoo<IDictionary>
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override string TypeName { get; } = "Dictionary";

        public override string TypeDescription { get; } = "Defines an Dictionary";

        public override bool IsValid { get { return Value != null; } }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public GH_Dictionary() : base() { }

        /***************************************************/

        public GH_Dictionary(IDictionary val) : base(val) { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override IGH_Goo Duplicate()
        {
            return new GH_Dictionary { Value = Value };
        }

        /*******************************************/

        public override string ToString()
        {
            IDictionary val = Value;
            if (val == null)
                return "null";
            else
                return val.GetType().ToText();
        }

        /*******************************************/
    }
}
