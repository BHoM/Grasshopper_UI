using BH.oM.Base;
using BH.oM.Geometry;
using BH.UI.Alligator.Base;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BH.UI.Alligator
{
    public static partial class Transform
    {
        public static object UnwrapObject(this object obj)
        {
            if (obj is GH_ObjectWrapper)
                return ((GH_ObjectWrapper)obj).Value;
            else if (obj is GH_String)
                return ((GH_String)obj).Value;
            else if (obj is IGH_Goo)
            {
                try
                {
                    System.Reflection.PropertyInfo prop = obj.GetType().GetProperty("Value");
                    return prop.GetValue(obj);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Grasshopper sucks, what can I do?" + e.ToString());
                }
                return obj;

            }
            else
                return obj;
        }
    }
}