using GH = Grasshopper;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Reflection;
using BHG = BH.oM.Geometry;
using BHB = BH.oM.Base;
using Grasshopper.Kernel;

namespace BH.Engine.Grasshopper // TODO Sort Out placement for DataUtils
{
    public static partial class Query
    {
        public static string GetDescription(PropertyInfo info)
        {
            object[] attri = info.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attri.Length > 0)
            {
                var description = (DescriptionAttribute)attri[0];
                return description.Description;
            }
            return "";
        }

        public static bool HasDefault(PropertyInfo info)
        {
            object[] attri = info.GetCustomAttributes(typeof(DefaultValueAttribute), true);
            if (attri.Length > 0)
            {
                return true;
            }
            return false;
        }

        public static object GetDefault(PropertyInfo info)
        {
            object[] attri = info.GetCustomAttributes(typeof(DefaultValueAttribute), true);
            if (attri.Length > 0)
            {
                var description = (DefaultValueAttribute)attri[0];
                return description.Value;
            }
            return "";
        }

        public static string GetName(PropertyInfo info)
        {
            object[] attri = info.GetCustomAttributes(typeof(DisplayNameAttribute), true);
            if (attri.Length > 0)
            {
                var description = (DisplayNameAttribute)attri[0];
                return description.DisplayName;
            }
            return info.Name;
        }

        public static T GetData<T>(GH.Kernel.IGH_DataAccess DA, int index)
        {
            T data = default(T);
            DA.GetData<T>(index, ref data);

            if (data != null)
            {
                if (typeof(IGH_Goo).IsAssignableFrom(data.GetType()))
                {
                    T result = default(T);
                    if (((IGH_Goo)data).CastTo<T>(out result))
                    {
                        return result;
                    }
                }
            }

            return data;
        }

        public static BHG.IBHoMGeometry GetDataGeom(GH.Kernel.IGH_DataAccess DA, int index)
        {
            object data = null;
            DA.GetData<object>(index, ref data);

            if (data is GH_Point)
            {
                return GeometryUtils.Convert((data as GH_Point).Value);
            }
            else if (data is GH_Curve)
            {
                //( data as GH_Curve).Value.
                // return GeometryUtils.Convert()
            }

            return null;
        }

        public static List<T> GetDataList<T>(GH.Kernel.IGH_DataAccess DA, int index)
        {
            List<T> data = new List<T>();
            DA.GetDataList<T>(index, data);
            return data;
        }

        public static T GetGenericData<T>(GH.Kernel.IGH_DataAccess DA, int index)
        {
            object obj = null;
            DA.GetData<object>(index, ref obj);
            int test = 0;
            
            T app = default(T);
            if (obj is GH.Kernel.Types.GH_ObjectWrapper)
            {
                test = test + 1;
                (obj as GH.Kernel.Types.GH_ObjectWrapper).CastTo<T>(out app);
            }
            else
                return (T)obj;
            return app;
        }

        public static List<T> GetGenericDataList<T>(GH.Kernel.IGH_DataAccess DA, int index)
        {
            List<object> obj = new List<object>();
            DA.GetDataList<object>(index, obj);
            List<T> result = new List<T>();

            for (int i = 0; i < obj.Count; i++)
            {
                T data = default(T);
                if (obj[i] == null) continue;
                else if (obj[i] is GH_ObjectWrapper)
                {
                    (obj[i] as GH_ObjectWrapper).CastTo<T>(out data);
                    result.Add(data);
                }
                else if (typeof(IGH_Goo).IsAssignableFrom(obj[i].GetType()))
                {
                    ((IGH_Goo)obj[i]).CastTo<T>(out data);
                    result.Add(data);
                }
                else result.Add((T)(object)obj[i]);
            }

            return result;
        }

        public static bool Run(GH.Kernel.IGH_DataAccess DA, int index)
        {
            bool run = false;
            DA.GetData<bool>(index, ref run);
            return run;
        }

        public static BHB.BHoMGroup GetGenericDataGroup(IGH_DataAccess DA, int index) 
        {
            BHB.BHoMGroup group = GetGenericData<BHB.BHoMGroup>(DA, index);

            if (group != null)
                return group;

            List<BHB.BHoMObject> list = GetGenericDataList<BHB.BHoMObject>(DA, index);

            if (list != null)
                return new BHB.BHoMGroup(list);

            return null;
        }

        //public static List<BH.oM.Structural.Elements.DesignElement> GetDesignElements(IGH_DataAccess DA, int index)
        //{

        //    List<BH.oM.Structural.Elements.DesignElement> elems = GetDataList<BH.oM.Structural.Elements.DesignElement>(DA, index);

        //    if (elems != null)
        //        return elems;

        //    List<BH.oM.Structural.Elements.Bar> bars = GetDataList<BH.oM.Structural.Elements.Bar>(DA, index);

        //    if (bars != null)
        //    {
        //        //elems = new List<BH.oM.Structural.Elements.DesignElement>();
        //        return bars.Select(x => new BH.oM.Structural.Elements.DesignElement(x)).ToList();
        //    }

        //    return null;
        //}


        public static bool PointOrNodeToNode(object n, out BH.oM.Structural.Elements.Node node)
        {

            if (typeof(GH_Point).IsAssignableFrom(n.GetType()))
            {
                node = new BH.oM.Structural.Elements.Node(GeometryUtils.Convert(((GH_Point)n).Value));
                return true;
            }

            //Gets node
            if (typeof(BH.oM.Structural.Elements.Node).IsAssignableFrom(n.GetType()))
            {
                node = (BH.oM.Structural.Elements.Node)n;
                return true;
            }

            //Gets node from Rhino point
            if (typeof(Rhino.Geometry.Point3d).IsAssignableFrom(n.GetType()))
            {
                node = new BH.oM.Structural.Elements.Node(GeometryUtils.Convert((Rhino.Geometry.Point3d)n));
                return true;
            }

            //Gets node from Rhino point
            if (typeof(Rhino.Geometry.Point).IsAssignableFrom(n.GetType()))
            {
                node = new BH.oM.Structural.Elements.Node(GeometryUtils.Convert((Rhino.Geometry.Point3d)n));
                return true;
            }

            //Gets node from BHoM point
            if (typeof(BHG.Point).IsAssignableFrom(n.GetType()))
            {
                node = new BH.oM.Structural.Elements.Node((BHG.Point)n);
                return true;
            }

            node = null;
            return false;
        }

        public static bool GetNodeFromPointOrNode(IGH_DataAccess DA, int DAindex, out BH.oM.Structural.Elements.Node node)
        {
            GH_ObjectWrapper n = null;

            //Grab input data
            if (!DA.GetData(DAindex, ref n))
            {
                node = null;
                return false;
            }

            return PointOrNodeToNode(n.Value, out node);

        }

        public static bool GetNodeListFromPointOrNodes(IGH_DataAccess DA, int DAindex, out List<BH.oM.Structural.Elements.Node> nodes)
        {


            nodes = new List<BH.oM.Structural.Elements.Node>();
            List<GH_ObjectWrapper> objs = new List<GH_ObjectWrapper>();

            if (!DA.GetDataList(DAindex, objs)) { return false; }

            for (int i = 0; i < objs.Count; i++)
            {
                BH.oM.Structural.Elements.Node node;
                if (PointOrNodeToNode(objs[i].Value, out node))
                    nodes.Add(node);
                else
                    return false;
            }

            return true;
        }
    }
}
