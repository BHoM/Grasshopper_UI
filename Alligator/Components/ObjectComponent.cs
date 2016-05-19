using BHoM.Global;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alligator
{
    public static class Utils
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

        internal static bool IsGeometric(Type pType)
        {
            return pType.BaseType == typeof(BHoM.Geometry.GeometryBase);
        }

        public static bool IsEnumerable(this Type type)
        {
            return type != typeof(string) && typeof(IEnumerable<>).IsAssignableFrom(type);
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

        internal static T GetData<T>(Grasshopper.Kernel.IGH_DataAccess DA, int index)
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

        internal static BHoM.Geometry.GeometryBase GetDataGeom(Grasshopper.Kernel.IGH_DataAccess DA, int index)
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

        internal static List<T> GetDataList<T>(Grasshopper.Kernel.IGH_DataAccess DA, int index)
        {
            List<T> data = new List<T>();
            DA.GetDataList<T>(index, data);
            return data;
        }

        internal static T GetGenericData<T>(Grasshopper.Kernel.IGH_DataAccess DA, int index)
        {
            object obj = null;
            DA.GetData<object>(index, ref obj);
                      
            T app = default(T);
            if (obj != null)  (obj as GH_ObjectWrapper).CastTo<T>(out app);
            return app;
        }

        internal static List<T> GetGenericDataList<T>(Grasshopper.Kernel.IGH_DataAccess DA, int index)
        {
            List<object> obj = new List<object>();
            DA.GetDataList<object>(index, obj);
            List<T> result = new List<T>();

            for (int i = 0; i < obj.Count; i++)
            {
                T data = default(T);
                (obj[i] as GH_ObjectWrapper).CastTo<T>(out data);
                result.Add(data);
            }

            return result;
        }

    }

    public class CustomData : GH_Component
    {
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("{AD22A083-3B0C-4E89-9D4E-FECCEDA95099}");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Data Name", "K", "Custom data name/key", GH_ParamAccess.list);
            pManager.AddGenericParameter("Data value", "V", "Custom data value", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Custom Data", "CD", "Custom data", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<string> keys = Utils.GetDataList<string>(DA, 0);
            List<object> data = Utils.GetGenericDataList<object>(DA, 1);

            if (keys.Count != data.Count)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                for (int i = 0; i < keys.Count; i++)
                {
                    dictionary.Add(keys[i], data[i]);
                }
                DA.SetData(0, dictionary);
            }
            else
            {
                throw (new Exception("Data Name and data value list must be of the same length"));
            }
        }
    }

    public abstract class BHoMBaseComponent<T> : GH_Component where T : BHoMObject
    {
        private ComboBox m_Options;
        private Type m_OptionType;
        private object m_SelectedOption;

        public BHoMBaseComponent() { }

        protected BHoMBaseComponent(string name, string nickname, string description, string category, string subCat) : base(name, nickname, description, category, subCat)
        {
           
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("{5FE0E2C4-5E50-410F-BBC7-C255FD0BD2B3}");
            }
        }

        private void InitialiseOptions(Type enumType)
        {
            m_Options = new ComboBox();
            m_Options.DropDownStyle = ComboBoxStyle.DropDownList;
            m_Options.SelectedValueChanged += OptionChanged;
            m_OptionType = enumType;
            string[] OptionNames = Enum.GetNames(enumType);

            for (int i = 0; i < OptionNames.Length; i++)
            {
                m_Options.Items.Add(OptionNames[i]);
            }
            m_Options.Items.Add("From Input");
            m_Options.SelectedIndex = 0;
            this.Message = m_SelectedOption.ToString();
        }

        private void OptionChanged(object sender, EventArgs e)
        {
            m_SelectedOption = Enum.Parse(m_OptionType, m_Options.SelectedItem.ToString());
            this.Message = m_Options.SelectedItem.ToString();
            this.ExpirePreview(true);
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            PropertyInfo[] propInfo = typeof(T).GetProperties();
            for (int i = 0; i < propInfo.Length; i++)
            {
                Type pType = propInfo[i].PropertyType;
                if (propInfo[i].GetSetMethod() != null)
                {
                    string name = Utils.GetName(propInfo[i]);
                    string nickName = name[0].ToString();
                    string description = Utils.GetDescription(propInfo[i]);
                    GH_ParamAccess access = Utils.IsEnumerable(pType) ? GH_ParamAccess.list : GH_ParamAccess.item;
                    if (pType == typeof(string))
                    {
                        pManager.AddTextParameter(name, nickName, description, access);
                    }
                    else if (pType.IsEnum)
                    {
                        pManager.AddTextParameter(name, nickName, description, access);
                        InitialiseOptions(pType);
                    }
                    else if (Utils.IsNumeric(pType))
                    {
                        if (Utils.IsInteger(pType)) pManager.AddIntegerParameter(name, nickName, description, access);
                        else pManager.AddNumberParameter(name, nickName, description, access);

                    }
                    else if (Utils.IsGeometric(pType))
                    {
                        if (pType == typeof(BHoM.Geometry.Point))
                        {
                            pManager.AddPointParameter(name, nickName, description, access);
                        }
                        else if (pType == typeof(BHoM.Geometry.Curve))
                        {
                            pManager.AddCurveParameter(name, nickName, description, access);
                        }
                        else if (pType == typeof(BHoM.Geometry.Surface))
                        {
                            pManager.AddBrepParameter(name, nickName, description, access);
                        }
                    }
                    else
                    {
                        pManager.AddGenericParameter(name, nickName, description, access);
                    }
                    if (Utils.HasDefault(propInfo[i]))
                    {
                        Params.Input[Params.Input.Count - 1].Optional = true;
                        Params.Input[Params.Input.Count - 1].AddVolatileData(new Grasshopper.Kernel.Data.GH_Path(0), 0, Utils.GetDefault(propInfo[i]));
                    }
                }
            }
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter(typeof(T).Name, typeof(T).Name[0].ToString(), "", GH_ParamAccess.item);
            PropertyInfo[] propInfo = typeof(T).GetProperties();
            for (int i = 0; i < propInfo.Length; i++)
            {
                Type pType = propInfo[i].PropertyType;
                if (propInfo[i].GetGetMethod() != null)
                {
                    if (typeof(BHoM.Geometry.GeometryBase).IsAssignableFrom(pType))//.BaseType == typeof(BHoM.Geometry.GeometryBase))
                    {
                        string name = Utils.GetName(propInfo[i]);
                        string nickName = name[0].ToString();
                        string description = Utils.GetDescription(propInfo[i]);
                        pManager.AddGeometryParameter(name, nickName, description, GH_ParamAccess.item);
                    }
                }
            }
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHoMObject obj = BHoMObject.CreateInstance(typeof(T));
            PropertyInfo[] propInfo = typeof(T).GetProperties();
            int propIndex = 0;
            for (int i = 0; i < Params.Input.Count; i++)
            {
                while (propInfo[propIndex].GetSetMethod() == null) propIndex++;
                PropertyInfo prop = propInfo[propIndex];
                Type pType = prop.PropertyType;
                propIndex++;

                if (Params.Input[i].Access == GH_ParamAccess.item)
                {
                    object value = null;
                    if (pType.BaseType == typeof(BHoMObject))
                    {
                        value = Utils.GetGenericData<BHoMObject>(DA, i);
                    }
                    else if (pType.BaseType == typeof(BHoM.Geometry.GeometryBase))
                    {
                        value = Utils.GetDataGeom(DA, i);
                        
                    }
                    else if (pType.IsEnum)
                    {
                        value = m_SelectedOption;
                        i--;
                    }
                    else
                    {
                        value = Utils.GetData<object>(DA, i);
                    }
                    prop.SetValue(obj, value);
                }
                else
                {
                    object list = null;
                    if (pType.BaseType == typeof(BHoMObject))
                    {
                        list = Utils.GetGenericDataList<BHoMObject>(DA, i);
                    }
                    else
                    {
                        list =  Utils.GetDataList<object>(DA, i);
                    }
                    prop.SetValue(obj, list);
                }
            }
            DA.SetData(0, obj);
            int geomIndex = 1;
            for (int i = 0; i < propInfo.Length; i++)
            {
                Type pType = propInfo[i].PropertyType;
                if (propInfo[i].GetGetMethod() != null && typeof(BHoM.Geometry.GeometryBase).IsAssignableFrom(pType))
                {
                    DA.SetData(geomIndex++, GeometryUtils.Convert(propInfo[i].GetValue(obj) as BHoM.Geometry.GeometryBase));
                }
            }
        }
        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            if (m_Options != null)
            {
                writer.SetString("EnumOption", m_Options.Text);
            }
            return base.Write(writer);
        }

        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            if (m_Options != null)
            {
                string selection = "";
                reader.TryGetString("EnumOption", ref selection);
                m_Options.SelectedItem = selection;
            }
            return base.Read(reader);
        }

        public override bool AppendMenuItems(ToolStripDropDown menu)
        {
            if (m_Options != null)
            {
                Menu_AppendObjectName(menu);// (menu, "Section");
                Menu_AppendEnableItem(menu);
                Menu_AppendBakeItem(menu);
                Menu_AppendSeparator(menu);
                Menu_AppendCustomItem(menu, m_Options);
                Menu_AppendSeparator(menu);
                // Menu
                Menu_AppendSeparator(menu);
                Menu_AppendObjectHelp(menu);
                return true;
            }
            else
            {
                return base.AppendMenuItems(menu);
            }
        }
    }
}
