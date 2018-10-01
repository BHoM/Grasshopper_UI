using BH.Engine.Alligator.Objects;
using BH.oM.Base;
using BH.oM.Geometry;
using Grasshopper.Kernel.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Grasshopper
{
    public static partial class Convert
    {
        /*******************************************/
        /**** Interface Methods                 ****/
        /*******************************************/

        public static IGH_Goo IToGoo(this object obj)
        {
            if (obj == null)
                return null;
            else
                return ToGoo(obj as dynamic);
        }

        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public static IGH_Goo ToGoo(this Enum obj)
        {
            return new GH_Enum(obj as Enum);
        }

        /*************************************/

        public static IGH_Goo ToGoo(this IGeometry obj)
        {
            return new GH_IBHoMGeometry(obj);
        }

        /*************************************/

        public static IGH_Goo ToGoo(this BHoMObject obj)
        {
            return new GH_BHoMObject(obj);
        }

        /*************************************/

        public static IGH_Goo ToGoo(this Type obj)
        {
            return new GH_Type(obj);
        }

        /*************************************/

        public static IGH_Goo ToGoo(this IDictionary obj)
        {
            return new GH_Dictionary(obj);
        }

        /*************************************/
    }
}
