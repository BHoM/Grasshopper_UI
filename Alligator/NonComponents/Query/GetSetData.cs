//using BH.oM.Base;
//using BH.oM.Geometry;
//using BH.UI.Alligator.Base;
//using Grasshopper.Kernel;
//using Grasshopper.Kernel.Types;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BH.UI.Alligator
//{
//    public static partial class Query
//    {
//        public static T GetData<T>(this IGH_DataAccess DA, int index, T destination)
//        {
//            if (typeof(IBHoMGeometry).IsAssignableFrom(typeof(T)))
//            {
//                GH_IBHoMGeometry bhg = new GH_IBHoMGeometry();
//                if (!DA.GetData(index, ref bhg)) { return default(T); }
//                return (T)bhg.Value;
//            }
//            else if (typeof(IObject).IsAssignableFrom(typeof(T)))
//            {
//                GH_BHoMObject bho = new GH_BHoMObject();
//                if (!DA.GetData(index, ref bho)) { return default(T); }
//                return (T)(bho.Value);
//            }
//            else
//            {
//                if (!DA.GetData(index, ref destination)) { return default(T); }
//                return destination;
//            }
//        }

//        public static List<T> GetDataList<T>(this IGH_DataAccess DA, int index, List<T> destination)
//        {
//            if (typeof(IBHoMGeometry).IsAssignableFrom(typeof(T)))
//            {
//                List<GH_IBHoMGeometry> bhg = new List<GH_IBHoMGeometry>();
//                if (!DA.GetDataList(index, bhg)) { return null; }
//                destination.Clear();
//                for (int i = 0; i < bhg.Count; i++)
//                {
//                    destination.Add((T)(bhg[i].Value));
//                }
//                return destination;
//            }
//            else if (typeof(IObject).IsAssignableFrom(typeof(T)))
//            {
//                List<GH_BHoMObject> bho = new List<GH_BHoMObject>();
//                if (!DA.GetDataList(index, bho)) { return null; }
//                destination.Clear();
//                for (int i = 0; i < bho.Count; i++)
//                {
//                    destination.Add((T)(bho[i].Value));
//                }
//                return destination;
//            }
//            else
//            {
//                DA.GetDataList(index, destination);
//                return destination;
//            }
//        }

//        public static bool SetData<T>(this IGH_DataAccess DA, int index, T source)
//        {
//            if (typeof(IBHoMGeometry).IsAssignableFrom(source.GetType()))
//            {
//                GH_IBHoMGeometry bhg = new GH_IBHoMGeometry(source as IBHoMGeometry);
//                return DA.SetData(index, bhg);
//            }
//            if (typeof(IObject).IsAssignableFrom(source.GetType()))
//            {
//                GH_BHoMObject bho = new GH_BHoMObject(source as IObject);
//                return DA.SetData(index, bho);
//            }
//            else { return DA.SetData(index, source); }
//        }

//        public static bool SetDataList<T>(this IGH_DataAccess DA, int index, List<T> source)
//        {
//            if (typeof(IBHoMGeometry).IsAssignableFrom(typeof(T)))
//            {
//                List<GH_IBHoMGeometry> bhg = new List<GH_IBHoMGeometry>();
//                for (int i = 0; i < source.Count(); i++)
//                {
//                    bhg.Add(new GH_IBHoMGeometry(source[i] as IBHoMGeometry));
//                }
//                return DA.SetDataList(index, bhg);
//            }
//            if (typeof(IObject).IsAssignableFrom(typeof(T)))
//            {
//                List<GH_BHoMObject> bho = new List<GH_BHoMObject>();
//                for (int i = 0; i < source.Count; i++)
//                {
//                    bho.Add(new GH_BHoMObject(source[i] as IObject));
//                }
//                return DA.SetDataList(index, bho);
//            }
//            else { return DA.SetDataList(index, source); }
//        }
//    }
//}
