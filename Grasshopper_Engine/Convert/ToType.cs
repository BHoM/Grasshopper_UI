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

            if (typeString.Contains("[[") && typeString.Contains("]]"))
            {
                int outerSplit = typeString.IndexOf("[[");
                int innerLength = typeString.LastIndexOf("]]") - outerSplit - 2;

                Type outer = typeString.Substring(0, outerSplit).ToType();
                Type inner = typeString.Substring(outerSplit + 2, innerLength).ToType();

                return outer.MakeGenericType(new Type[] { inner });
            }

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
