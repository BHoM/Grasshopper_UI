using System;
using Grasshopper.Kernel.Parameters;

namespace BH.UI.Grasshopper.GeometryHints
{
    public class BH_NurbCurveHint : IGH_TypeHint
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public Guid HintID { get; } = new Guid("cbf9a9aa-471f-4a92-b8c0-f80096b7567b");

        public string TypeName { get; } = typeof(BH.oM.Geometry.NurbsCurve).ToString();


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
