using System;
using Grasshopper.Kernel.Parameters;

namespace BH.UI.Alligator.GeometryHints
{
    public class BH_TypeHint : IGH_TypeHint
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public Guid HintID { get; } = new Guid("8ECF16E7-F71B-4813-AD63-C4AECC246A26");

        public string TypeName { get; } = "Type";


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public bool Cast(object data, out object target)
        {
            target = data;
            return true;
        }

        /*******************************************/
    }
}
