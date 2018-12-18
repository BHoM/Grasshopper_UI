using System;
using Grasshopper.Kernel.Parameters;

namespace BH.UI.Grasshopper.GeometryHints
{
    public class BH_PointHint : IGH_TypeHint
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public Guid HintID { get; } = new Guid("67b20827-0326-4442-a4bc-42cfc8d69674");

        public string TypeName { get; } = typeof(BH.oM.Geometry.Point).ToString();


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
