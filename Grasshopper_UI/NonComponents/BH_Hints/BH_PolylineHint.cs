using System;
using Grasshopper.Kernel.Parameters;

namespace BH.UI.Grasshopper.GeometryHints
{
    public class BH_PolylineHint : IGH_TypeHint
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public Guid HintID { get; } = new Guid("bee5c313-57ad-44d0-bd36-30f390261b9f");

        public string TypeName { get; } = typeof(BH.oM.Geometry.Polyline).ToString();


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
