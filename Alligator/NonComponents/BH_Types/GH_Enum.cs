using Grasshopper.Kernel.Types;
using System;

namespace BH.UI.Alligator
{
    public class GH_Enum : GH_TemplateType<Enum>
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override string TypeName { get; } = "Enum";

        public override string TypeDescription { get; } = "Defines an enum";


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
    }
}
