using BH.Engine.Rhinoceros;
using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Grasshopper
{
    public static partial class Convert
    {
        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public static object ToRhino(object x)
        {
            try
            {
                if (x == null)
                    return null;
                else if (BH.Engine.Rhinoceros.Query.IsRhinoEquivalent(x.GetType()))
                    return ((IGeometry)x).IToRhino();
                else
                    return x;
            }
            catch (Exception e)
            {
                BH.Engine.Reflection.Compute.RecordError("Object of type " + x.GetType().ToString() + " failed to convert to a Rhino geometry\nError: " + e.Message);
                return null;
            }
        }

        /*******************************************/
    }
}
