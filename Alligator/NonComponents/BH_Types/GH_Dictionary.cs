using Grasshopper.Kernel.Types;
using System.Collections;

namespace BH.UI.Alligator
{
    public class GH_Dictionary : GH_TemplateType<IDictionary>
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override string TypeName { get; } = "Dictionary";

        public override string TypeDescription { get; } = "Defines an Dictionary"; 


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
    }
}
