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
                            string name = GHE.DataUtils.GetName(propInfo[i]);
                            string nickName = name[0].ToString();
                            string description = GHE.DataUtils.GetDescription(propInfo[i]);
                            GH_ParamAccess access = GHE.DataUtils.IsEnumerable(pType) ? GH_ParamAccess.list : GH_ParamAccess.item;
                            if (pType == typeof(string))
                            {
                                pManager.AddTextParameter(name, nickName, description, access);
                            }
                            else if (pType.IsEnum)
                            {
                                //pManager.AddTextParameter(name, nickName, description, access);
                                InitialiseOptions(pType);
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
                                    pManager.AddGroupParameter(name, nickName, description, GH_ParamAccess.item);
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
            pManager.AddGenericParameter(typeof(T).Name, typeof(T).Name[0].ToString(), "", GH_ParamAccess.item);
            PropertyInfo[] propInfo = typeof(T).GetProperties();
            for (int i = 0; i < propInfo.Length; i++)
            {
                Type pType = propInfo[i].PropertyType;
                   if (propInfo[i].GetGetMethod() != null)
                    {
                        if (typeof(BHoM.Geometry.GeometryBase).IsAssignableFrom(pType))//.BaseType == typeof(BHoM.Geometry.GeometryBase))
                        {
                            string name = GHE.DataUtils.GetName(propInfo[i]);
                            string nickName = name[0].ToString();
                            string description = GHE.DataUtils.GetDescription(propInfo[i]);
                            pManager.AddGeometryParameter(name, nickName, description, GH_ParamAccess.item);
                        }
                    }
            }
        }
       

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHB.BHoMObject obj = BHB.BHoMObject.CreateInstance(typeof(T));
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
                    if (pType.BaseType == typeof(BHB.BHoMObject))
                    {
                        value = GHE.DataUtils.GetGenericData<BHB.BHoMObject>(DA, i);
                    }
                    else if (pType.BaseType == typeof(BHoM.Geometry.GeometryBase))
                    {
                        value = GHE.DataUtils.GetDataGeom(DA, i);
                        
                    }
                    else if (pType.IsEnum)
                    {
                        value = m_SelectedOption;
                        i--;
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
                        MethodInfo genericMethodInfo = methodInfo.MakeGenericMethod(new Type[] { listType });

                        var newList = typeof(List<>);
                        var listOfType = newList.MakeGenericType(listType);
                        var list = genericMethodInfo.Invoke(null, new object[] { DA, i });

                        prop.SetValue(obj, list);
                    }
                }
            }
            if (obj.CustomData == null) obj.CustomData = new Dictionary<string, object>();
            DA.SetData(0, obj);
            int geomIndex = 1;
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
