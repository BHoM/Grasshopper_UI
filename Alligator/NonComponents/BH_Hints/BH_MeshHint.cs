using System;
using Grasshopper.Kernel.Parameters;

namespace BH.UI.Alligator.GeometryHints
{
    public class BH_MeshHint : IGH_TypeHint
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public Guid HintID { get; } = new Guid("66457d29-e0ab-410a-9159-a51015d2923c");

        public string TypeName { get; } = typeof(BH.oM.Geometry.Mesh).ToString();


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
