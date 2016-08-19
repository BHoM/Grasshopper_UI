using BHoM.Global;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using BHB = BHoM.Base;
using GHE = Grasshopper_Engine;

namespace Alligator.Components
{

    public abstract class BHoMBaseComponent<T> : GH_Component where T : BHB.BHoMObject
    {
        private List<ComboBox> m_Options;
        private List<Type> m_OptionType;
        private List<object> m_SelectedOption;

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

        private void Initialise()
        {
            if (m_Options == null) m_Options = new List<ComboBox>();
            if (m_OptionType == null) m_OptionType = new List<Type>();
            if (m_SelectedOption == null) m_SelectedOption = new List<object>();
        }

        private void AppendEnumOptions(string name, Type enumType)
        {
            Initialise();
            ComboBox box = new ComboBox();
            box.Name = name;
            box.DropDownStyle = ComboBoxStyle.DropDownList;
            box.SelectedValueChanged += OptionChanged;

            m_Options.Add(box);
            m_SelectedOption.Add(0);
            m_OptionType.Add(enumType);

            string[] OptionNames = Enum.GetNames(enumType);

            for (int i = 0; i < OptionNames.Length; i++)
            {
                box.Items.Add(OptionNames[i]);
            }

            box.SelectedIndex = 0;
        }

        private void OptionChanged(object sender, EventArgs e)
        {
            int index = m_Options.IndexOf((ComboBox)sender);
            m_SelectedOption[index] = Enum.Parse(m_OptionType[index], m_Options[index].SelectedItem.ToString());
            this.Message = m_Options[index].SelectedItem.ToString();
            this.ExpirePreview(true);
            this.ExpireSolution(true);
        }


        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            PropertyInfo[] propInfo = typeof(T).GetProperties();
            for (int i = 0; i < propInfo.Length; i++)
            {
                Type pType = propInfo[i].PropertyType;
                if (propInfo[i].GetSetMethod() != null)
                {
                    string name = GHE.DataUtils.GetName(propInfo[i]);
                    string nickName = propInfo[i].Name.ToString();
                    string description = GHE.DataUtils.GetDescription(propInfo[i]);
                    GH_ParamAccess access = GHE.DataUtils.IsEnumerable(pType) ? GH_ParamAccess.list : GH_ParamAccess.item;
                    if (pType == typeof(string))
                    {
                        pManager.AddTextParameter(name, nickName, description, access);
                    }
                    else if (pType.IsEnum)
                    {
                        //pManager.AddTextParameter(name, nickName, description, access);
                        AppendEnumOptions(propInfo[i].Name, pType);
                    }
                    else if (GHE.DataUtils.IsNumeric(pType))
                    {
                        if (GHE.DataUtils.IsInteger(pType)) pManager.AddIntegerParameter(name, nickName, description, access);
                        else pManager.AddNumberParameter(name, nickName, description, access);

                    }
                    else if (GHE.DataUtils.IsGeometric(pType))
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
                        else if (pType == typeof(BHoM.Geometry.Group<BHoM.Geometry.Curve>))
                        {
                            pManager.AddCurveParameter(name, nickName, description, GH_ParamAccess.list);
                        }
                    }
                    else
                    {
                        pManager.AddGenericParameter(name, nickName, description, access);
                    }
                    if (GHE.DataUtils.HasDefault(propInfo[i]))
                    {
                        Params.Input[Params.Input.Count - 1].Optional = true;
                        Params.Input[Params.Input.Count - 1].AddVolatileData(new Grasshopper.Kernel.Data.GH_Path(0), 0, GHE.DataUtils.GetDefault(propInfo[i]));
                    }
                }
            }
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter(typeof(T).Name, typeof(T).Name.ToString(), "", GH_ParamAccess.item);
            PropertyInfo[] propInfo = typeof(T).GetProperties();
            for (int i = 0; i < propInfo.Length; i++)
            {
                Type pType = propInfo[i].PropertyType;
                if (propInfo[i].GetGetMethod() != null)
                {
                    if (typeof(BHoM.Geometry.GeometryBase).IsAssignableFrom(pType))//.BaseType == typeof(BHoM.Geometry.GeometryBase))
                    {
                        string name = GHE.DataUtils.GetName(propInfo[i]);
                        string nickName = name.ToString();
                        string description = GHE.DataUtils.GetDescription(propInfo[i]);
                        pManager.AddGeometryParameter(name, nickName, description, GH_ParamAccess.item);
                    }
                }
            }
        }
       
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHB.BHoMObject obj = BHB.BHoMObject.CreateInstance(typeof(T));
            int propIndex = 0;

            for (int i = 0; m_Options != null && i < m_Options.Count; i++)
            {
                PropertyInfo prop = typeof(T).GetProperty(m_Options[i].Name);
                prop.SetValue(obj, m_SelectedOption[i]);
            }

            for (int i = 0; i < Params.Input.Count; i++)
            {
                PropertyInfo prop = typeof(T).GetProperty(Params.Input[i].NickName);
                Type pType = prop.PropertyType;
                propIndex++;

                if (Params.Input[i].Access == GH_ParamAccess.item)
                {
                    object value = null;
                    if (pType.BaseType == typeof(BHB.BHoMObject))
                    {
                        value = GHE.DataUtils.GetGenericData<BHB.BHoMObject>(DA, i);
                    }
                    else if (pType.BaseType == typeof(BHoM.Geometry.GeometryBase))
                    {
                        value = GHE.DataUtils.GetDataGeom(DA, i);                   
                    }
                    else if (GHE.DataUtils.IsNumeric(pType))
                    {
                        if (GHE.DataUtils.IsInteger(pType)) value = GHE.DataUtils.GetData<int>(DA, i);
                        else value = GHE.DataUtils.GetData<double>(DA, i);
                    }
                    else
                    {
                        value = GHE.DataUtils.GetData<object>(DA, i);
                    }
                    prop.SetValue(obj, value, null);
                }
                else
                {
                    if (pType.IsGenericType)
                    {
                        Type listType = pType.GetGenericArguments()[0];
                        var utils = typeof(GHE.DataUtils);
                        MethodInfo methodInfo = utils.GetMethod("GetGenericDataList", System.Reflection.BindingFlags.Static | BindingFlags.Public);
                        MethodInfo genericMethodInfo = null;

                        if (listType.BaseType == typeof(BHoM.Geometry.GeometryBase))
                        {
                            genericMethodInfo = methodInfo.MakeGenericMethod(new Type[] { GHE.GeometryUtils.GetRhinoType(listType) });
                        }
                        else
                        {
                            genericMethodInfo = methodInfo.MakeGenericMethod(new Type[] { listType });
                        }
                    
                        object list = genericMethodInfo.Invoke(null, new object[] { DA, i });

                        if (listType.BaseType == typeof(BHoM.Geometry.GeometryBase))
                        {
                            utils = typeof(GHE.GeometryUtils);
                            methodInfo = utils.GetMethod("ConvertList", System.Reflection.BindingFlags.Static | BindingFlags.Public);
                            genericMethodInfo = methodInfo.MakeGenericMethod(new Type[] { listType, GHE.GeometryUtils.GetRhinoType(listType) });
                            list = genericMethodInfo.Invoke(null, new object[] { list });
                        }

                        prop.SetValue(obj, list);
                    }
                }
            }

            if (obj.CustomData == null) obj.CustomData = new Dictionary<string, object>();

            DA.SetData(0, obj);

            int geomIndex = 1;
            PropertyInfo[] propInfo = typeof(T).GetProperties();

            for (int i = 0; i < propInfo.Length; i++)
            {
                Type pType = propInfo[i].PropertyType;
                if (propInfo[i].GetGetMethod() != null && typeof(BHoM.Geometry.GeometryBase).IsAssignableFrom(pType))
                {
                    DA.SetData(geomIndex++, GHE.GeometryUtils.Convert(propInfo[i].GetValue(obj, null) as BHoM.Geometry.GeometryBase));
                }
            }
        }
        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            if (m_Options != null && m_Options.Count > 0)
            {
                for (int i = 0; i < m_Options.Count; i++)
                {
                    writer.SetString("EnumOption " + i, m_Options[i].Text);
                }
            }
            return base.Write(writer);
        }

        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            if (m_Options != null)
            {
                string selection = "";
                for (int i = 0; i < m_Options.Count; i++)
                {
                    reader.TryGetString("EnumOption " + i, ref selection);
                    m_Options[i].SelectedItem = selection;
                }
            }
            return base.Read(reader);
        }

        public override bool AppendMenuItems(ToolStripDropDown menu)
        {
            if (m_Options != null && m_Options.Count > 0)
            {
                Menu_AppendObjectName(menu);// (menu, "Section");
                Menu_AppendEnableItem(menu);
                Menu_AppendBakeItem(menu);
                Menu_AppendSeparator(menu);
                for (int i = 0; i < m_Options.Count; i++)
                {
                    Menu_AppendCustomItem(menu, m_Options[i]);
                }
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
