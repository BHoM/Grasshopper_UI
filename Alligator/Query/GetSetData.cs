using BH.oM.Base;
using BH.oM.Geometry;
using BH.UI.Alligator.Base;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.UI.Alligator.Query
{
    public static class RetrieveInput
    {
        public static T BH_GetData<T>(this IGH_DataAccess DA, int index, T destination)
        {
            if (typeof(IBHoMGeometry).IsAssignableFrom(destination.GetType()))
            {
                BH_GeometricGoo bhg = new BH_GeometricGoo();
                if (!DA.GetData(index, ref bhg)) { return default(T); }
                return (T)bhg.Value;
            }
            else if (typeof(IObject).IsAssignableFrom(destination.GetType()))
            {
                BH_Goo bho = new BH_Goo();
                if (!DA.GetData(index, ref bho)) { return default(T); }
                return (T)(bho.Value);
            }
            else
            {
                if (!DA.GetData(index, ref destination)) { return default(T); }
                return destination;
            }
        }

        public static List<T> BH_GetDataList<T>(this IGH_DataAccess DA, int index, List<T> destination)
        {
            if (typeof(IBHoMGeometry).IsAssignableFrom(typeof(T)))
            {
                List<BH_GeometricGoo> bhg = new List<BH_GeometricGoo>();
                if (!DA.GetDataList(index, bhg)) { return null; }
                destination.Clear();
                for (int i = 0; i < bhg.Count; i++)
                {
                    destination.Add((T)(bhg[i].Value));
                }
                return destination;
            }
            else if (typeof(IObject).IsAssignableFrom(typeof(T)))
            {
                List<BH_Goo> bho = new List<BH_Goo>();
                if (!DA.GetDataList(index, bho)) { return null; }
                destination.Clear();
                for (int i = 0; i < bho.Count; i++)
                {
                    destination.Add((T)(bho[i].Value));
                }
                return destination;
            }
            else
            {
                DA.GetDataList(index, destination);
                return destination;
            }
        }

        public static bool BH_SetData<T>(this IGH_DataAccess DA, int index, T source)
        {
            if (typeof(IBHoMGeometry).IsAssignableFrom(source.GetType()))
            {
                BH_GeometricGoo bhg = new BH_GeometricGoo(source as IBHoMGeometry);
                return DA.SetData(index, bhg);
            }
            if (typeof(IObject).IsAssignableFrom(source.GetType()))
            {
                BH_Goo bho = new BH_Goo(source as IObject);
                return DA.SetData(index, bho);
            }
            else { return DA.SetData(index, source); }
        }

        public static bool BH_SetDataList<T>(this IGH_DataAccess DA, int index, List<T> source)
        {
            if (typeof(IBHoMGeometry).IsAssignableFrom(typeof(T)))
            {
                List<BH_GeometricGoo> bhg = new List<BH_GeometricGoo>();
                for (int i = 0; i < source.Count(); i++)
                {
                    bhg.Add(new BH_GeometricGoo(source[i] as IBHoMGeometry));
                }
                return DA.SetDataList(index, bhg);
            }
            if (typeof(IObject).IsAssignableFrom(typeof(T)))
            {
                List<BH_Goo> bho = new List<BH_Goo>();
                for (int i = 0; i < source.Count; i++)
                {
                    bho.Add(new BH_Goo(source[i] as IObject));
                }
                return DA.SetDataList(index, bho);
            }
            else { return DA.SetDataList(index, source); }
        }

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
