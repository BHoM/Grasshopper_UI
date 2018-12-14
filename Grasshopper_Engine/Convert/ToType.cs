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

        public static Type ToType(this string typeString)
        {
            // Try to get teh type from assembly name
            Type type = Type.GetType(typeString);
            if (type != null)
                return type;

            // Try get to recover type from full name
            string[] parts = typeString.Split(new char[] { ',' });
            if (parts.Length == 0)
                return null;
            else
                return BH.Engine.Reflection.Create.Type(parts[0]);
        }

        /*******************************************/
    }
}
