using System;
using Grasshopper.Kernel.Parameters;

namespace BH.UI.Grasshopper.GeometryHints
{
    public class BH_VectorHint : IGH_TypeHint
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public Guid HintID { get; } = new Guid("5c0e7f26-adba-4665-85fa-81e57a5b6a69");

        public string TypeName { get; } = typeof(BH.oM.Geometry.Vector).ToString();


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
