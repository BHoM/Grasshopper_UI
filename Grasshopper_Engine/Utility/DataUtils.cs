using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using BHG = BHoM.Geometry;
using BHB = BHoM.Base;

namespace Grasshopper_Engine
{
    public static class DataUtils
    {
        public static bool IsNumeric(this Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                case TypeCode.Object:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        return Nullable.GetUnderlyingType(type).IsNumeric();
                        //return IsNumeric(Nullable.GetUnderlyingType(type));
                    }
                    return false;
                default:
                    return false;
            }
        }

        public static bool IsInteger(this Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return true;
                case TypeCode.Object:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        return Nullable.GetUnderlyingType(type).IsNumeric();
                        //return IsNumeric(Nullable.GetUnderlyingType(type));
                    }
                    return false;
                default:
                    return false;
            }
        }

        public static bool IsGeometric(Type pType)
        {
            return pType.BaseType == typeof(BHoM.Geometry.GeometryBase);
        }

        public static bool IsEnumerable(Type type)
        {
            return type != typeof(string) && !typeof(IDictionary<string, object>).IsAssignableFrom(type) && type.GetInterface("IEnumerable") != null;
        }

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


        public static T GetData<T>(Grasshopper.Kernel.IGH_DataAccess DA, int index)
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

        public static BHoM.Geometry.GeometryBase GetDataGeom(Grasshopper.Kernel.IGH_DataAccess DA, int index)
        {
            object data = null;
            DA.GetData<object>(index, ref data);

            if (data is GH_Point)
            {
                return new BHoM.Geometry.Point(GeometryUtils.Convert((data as GH_Point).Value));
            }
            else if (data is GH_Curve)
            {
                //( data as GH_Curve).Value.
                // return GeometryUtils.Convert()
            }

            return null;
        }

        public static List<T> GetDataList<T>(Grasshopper.Kernel.IGH_DataAccess DA, int index)
        {
            List<T> data = new List<T>();
            DA.GetDataList<T>(index, data);
            return data;
        }

        public static T GetGenericData<T>(Grasshopper.Kernel.IGH_DataAccess DA, int index)
        {
            object obj = null;
            DA.GetData<object>(index, ref obj);

            T app = default(T);
            if (obj is Grasshopper.Kernel.Types.GH_ObjectWrapper)
                (obj as Grasshopper.Kernel.Types.GH_ObjectWrapper).CastTo<T>(out app);
            else
                return (T)obj;
            return app;
        }

        public static List<T> GetGenericDataList<T>(Grasshopper.Kernel.IGH_DataAccess DA, int index)
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

        public static bool Run(Grasshopper.Kernel.IGH_DataAccess DA, int index)
        {
            bool run = false;
            DA.GetData<bool>(index, ref run);
            return run;
        }

        public static BHB.Group<T> GetGenericDataGroup<T>(IGH_DataAccess DA, int index) where T:BHB.BHoMObject
        {
            BHB.Group<T> group = GetGenericData<BHB.Group<T>>(DA, index);

            if (group != null)
                return group;

            List<T> list = GetGenericDataList<T>(DA, index);

            if (list != null)
                return new BHoM.Base.Group<T>(list);

            return null;
        }

        public static bool PointOrNodeToNode(object n, out BHoM.Structural.Elements.Node node)
        {

            if (typeof(GH_Point).IsAssignableFrom(n.GetType()))
            {
                node = new BHoM.Structural.Elements.Node(GeometryUtils.Convert(((GH_Point)n).Value));
                return true;
            }

            //Gets node
            if (typeof(BHoM.Structural.Elements.Node).IsAssignableFrom(n.GetType()))
            {
                node = (BHoM.Structural.Elements.Node)n;
                return true;
            }

            //Gets node from Rhino point
            if (typeof(Rhino.Geometry.Point3d).IsAssignableFrom(n.GetType()))
            {
                node = new BHoM.Structural.Elements.Node(GeometryUtils.Convert((Rhino.Geometry.Point3d)n));
                return true;
            }

            //Gets node from Rhino point
            if (typeof(Rhino.Geometry.Point).IsAssignableFrom(n.GetType()))
            {
                node = new BHoM.Structural.Elements.Node(GeometryUtils.Convert((Rhino.Geometry.Point3d)n));
                return true;
            }

            //Gets node from BHoM point
            if (typeof(BHG.Point).IsAssignableFrom(n.GetType()))
            {
                node = new BHoM.Structural.Elements.Node((BHG.Point)n);
                return true;
            }

            node = null;
            return false;
        }

        public static bool GetNodeFromPointOrNode(IGH_DataAccess DA, int DAindex, out BHoM.Structural.Elements.Node node)
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

        public static bool GetNodeListFromPointOrNodes(IGH_DataAccess DA, int DAindex, out List<BHoM.Structural.Elements.Node> nodes)
        {


            nodes = new List<BHoM.Structural.Elements.Node>();
            List<GH_ObjectWrapper> objs = new List<GH_ObjectWrapper>();

            if(!DA.GetDataList(DAindex, objs)) { return false; }

            for (int i = 0; i < objs.Count; i++)
            {
                BHoM.Structural.Elements.Node node;
                if (PointOrNodeToNode(objs[i].Value, out node))
                    nodes.Add(node);
                else
                    return false;
            }

            return true;
        }
    }
}
