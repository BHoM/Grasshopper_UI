using System;
using Grasshopper.Kernel;
using Alligator.Components;
using System.Collections.Generic;
using BHE = BHoM.Structural.Elements;
using BHP = BHoM.Structural.Properties;
using Grasshopper_Engine.Components;
using System.Windows.Forms;
using Grasshopper.Kernel.Parameters;
using System.Reflection;
using Grasshopper_Engine;

namespace Alligator.Structural.Properties
{
    public abstract class CreateSectionProperty<T> : BHoMBaseComponent<T> where T : BHP.SectionProperty
    {
        public CreateSectionProperty(string name, string nickname, string description, string category, string subCat) : base(name, nickname, description, category, subCat)
        {

        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Name of the section Property", GH_ParamAccess.item);
            pManager.AddGenericParameter("CustomData", "CustomData", "CustomData", GH_ParamAccess.item);
            pManager.AddGenericParameter("Material", "Material", "The material of the cross section", GH_ParamAccess.item);
            Params.Input[0].Optional = true;
            Params.Input[1].Optional = true;
            Params.Input[2].Optional = true;

            AppendEnumOptions("Shape", typeof(BHP.ShapeType));
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string propertyName = null;
            BHP.SectionProperty barProperty = null;
            DA.GetData<string>(0, ref propertyName);
            BHoM.Materials.MaterialType matType = BHoM.Materials.MaterialType.Steel;
            if (typeof(T) == typeof(BHP.ConcreteSection))
            {
                matType = BHoM.Materials.MaterialType.Concrete;
            }
            else if (typeof(T) == typeof(BHP.SteelSection))
            {
                matType = BHoM.Materials.MaterialType.Steel;
            }

            if (barProperty == null)
            {
                double[] dimensions = new double[12];
                int start = typeof(T) == typeof(BHP.ConcreteSection) ?  5 : 3;
                for (int i = start; i < Params.Input.Count; i++)
                {
                    DA.GetData<double>(i, ref dimensions[i - start]);                   
                }
             
                switch ((BHP.ShapeType)m_SelectedOption[0])
                {
                    case BHP.ShapeType.ISection:
                        barProperty = BHP.SectionProperty.CreateISection(matType, dimensions[1], dimensions[2], dimensions[0], dimensions[3], dimensions[4], dimensions[5], dimensions[6], dimensions[7]);
                        double test = barProperty.Iy;
                        break;
                    case BHP.ShapeType.Box:
                        barProperty = BHP.SectionProperty.CreateBoxSection(matType, dimensions[0], dimensions[1],  dimensions[2], dimensions[3], dimensions[4], dimensions[5]);
                        break;
                    case BHP.ShapeType.Rectangle:
                        barProperty = BHP.SectionProperty.CreateRectangularSection(matType, dimensions[0], dimensions[1], dimensions[2]);
                        break;
                    case BHP.ShapeType.Tee:
                        barProperty = BHP.SectionProperty.CreateTeeSection(matType, dimensions[0], dimensions[1],  dimensions[2], dimensions[3], dimensions[4], dimensions[5]);
                        break;
                    case BHP.ShapeType.Circle:
                        barProperty = BHP.SectionProperty.CreateCircularSection(matType, dimensions[0]);
                        break;
                    case BHP.ShapeType.Tube:
                        barProperty = BHP.SectionProperty.CreateTubeSection(matType, dimensions[0], dimensions[1]);
                        break;

                }
                if(propertyName != null)
                    barProperty.Name = propertyName;
            }

            if (barProperty == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Failed to create bar section property. Check the input data");
                return;
            }

            BHoM.Materials.Material mat = Grasshopper_Engine.DataUtils.GetGenericData<BHoM.Materials.Material>(DA, 2);
            Dictionary<string, object> customData = Grasshopper_Engine.DataUtils.GetGenericData<Dictionary<string, object>>(DA, 1);

            if (mat != null)
                barProperty.Material = mat;
            if (customData != null)
                barProperty.CustomData = customData;

            for (int i = 0; m_Options != null && i < m_Options.Count; i++)
            {
                PropertyInfo prop = typeof(T).GetProperty(m_Options[i].Name);
                if (prop!=null) prop.SetValue(barProperty, m_SelectedOption[i]);
            }

            if (typeof(T) == typeof(BHP.ConcreteSection))
            {
                (barProperty as BHP.ConcreteSection).Reinforcement = DataUtils.GetGenericDataList<BHP.Reinforcement>(DA, 3);
                (barProperty as BHP.ConcreteSection).MinimumCover = DataUtils.GetData<double>(DA, 4);
            }

            DA.SetData(0, barProperty);
            SetGeometry(barProperty, DA);
        }

        protected override void UpdateInput(object enumSelection)
        {
            int firstParamIndex = 3;

            if (typeof(T) == typeof(BHP.ConcreteSection)) firstParamIndex += 2;

            if (enumSelection.GetType() == typeof(BHP.ShapeType))
            {
                switch ((BHP.ShapeType)enumSelection)
                {
                    case BHP.ShapeType.ISection:
                        CreateParam("Depth", "Depth", "Total Depth (m)", GH_ParamAccess.item, firstParamIndex, false);
                        CreateParam("Top Flange Width", "B1", "Width of Top flange (m)", GH_ParamAccess.item, firstParamIndex+1, false);
                        CreateParam("Bottom Flange Width", "B2", "Width of Bottom Flange (m)", GH_ParamAccess.item, firstParamIndex+2);
                        CreateParam("Top Flange thickness", "Tf", "Thickness of flange (m)", GH_ParamAccess.item, firstParamIndex+3, false);
                        CreateParam("Bottom Flange thickness", "Tb", "Thickness of flange (m)", GH_ParamAccess.item, firstParamIndex +4);
                        CreateParam("Web thickness", "Tw", "Thickness of Web (m)", GH_ParamAccess.item, firstParamIndex +5, false);
                        if (typeof(T) == typeof(BHP.SteelSection))
                        {
                            CreateParam("Inner Fillet radius", "Ri", "Inner Fillet Radius (m)", GH_ParamAccess.item, firstParamIndex + 6);
                            CreateParam("Outter Fillet radius", "Ro", "Outer Fillet Radius (m)", GH_ParamAccess.item, firstParamIndex + 7);
                            CreateParam("Rotation", "A", "Axis rotation", GH_ParamAccess.item, firstParamIndex + 8);
                        }
                        else if (typeof(T) == typeof(BHP.ConcreteSection))
                        {
                            UnregisterParameterFrom(11);
                        }
                        break;
                    case BHP.ShapeType.Box:
                    case BHoM.Structural.Properties.ShapeType.Tee:
                        CreateParam("Depth", "Depth", "Total Depth (m)", GH_ParamAccess.item, firstParamIndex, false);
                        CreateParam("Width", "Width", "Total Width (m)", GH_ParamAccess.item, firstParamIndex + 1, false);
                        CreateParam("Flange thickness", "Tf", "Thickness of flange (m)", GH_ParamAccess.item, firstParamIndex +2);
                        CreateParam("Web thickness", "Tw", "Thickness of Web (m)", GH_ParamAccess.item, firstParamIndex +3);
                        if (typeof(T) == typeof(BHP.SteelSection))
                        {
                            CreateParam("Inner Fillet radius", "Ri", "Inner Fillet Radius (m)", GH_ParamAccess.item, firstParamIndex + 4);
                            CreateParam("Outter Fillet radius", "Ro", "Outer Fillet Radius (m)", GH_ParamAccess.item, firstParamIndex + 5);
                            CreateParam("Rotation", "A", "Axis rotation", GH_ParamAccess.item, firstParamIndex + 6);
                            UnregisterParameterFrom(10);
                        }
                        else if (typeof(T) == typeof(BHP.ConcreteSection))
                        {
                            CreateParam("Rotation", "A", "Axis rotation", GH_ParamAccess.item, firstParamIndex + 4);
                            UnregisterParameterFrom(10);
                        }
                        
                        break;
                    case BHP.ShapeType.Rectangle:
                        CreateParam("Depth", "Depth", "Total Depth (m)", GH_ParamAccess.item, firstParamIndex, false);
                        CreateParam("Width", "Width", "Total Width (m)", GH_ParamAccess.item, firstParamIndex + 1, false);
                        if (typeof(T) == typeof(BHP.SteelSection))
                        {
                            CreateParam("Edge Radius", "Radius", "Edge Radius (m)", GH_ParamAccess.item, firstParamIndex + 2);
                            UnregisterParameterFrom(6);
                        }
                        else
                        {
                            UnregisterParameterFrom(7);
                        }
                        break;
                    case BHP.ShapeType.Circle:
                        CreateParam("Diameter", "Diameter", "Total Diameter (m)", GH_ParamAccess.item, firstParamIndex, false);
                        if (typeof(T) == typeof(BHP.SteelSection))
                        {
                            UnregisterParameterFrom(4);
                        }
                        else if (typeof(T) == typeof(BHP.ConcreteSection))
                        {
                            UnregisterParameterFrom(6);
                        }
                        break;
                    case BHP.ShapeType.Tube:
                        CreateParam("Outer Diameter", "Diameter", "Total Diameter (m)", GH_ParamAccess.item, firstParamIndex, false);
                        CreateParam("Thickness", "Thickness", "Thickness (m)", GH_ParamAccess.item, firstParamIndex +1, false);
                        UnregisterParameterFrom(5);
                        break;
                    default:
                        CreateParam("Depth", "Depth", "Total Depth (m)", GH_ParamAccess.item, firstParamIndex, false);
                        CreateParam("Width", "Width", "Total Width (m)", GH_ParamAccess.item, firstParamIndex + 1, false);
                        UnregisterParameterFrom(5);
                        break;
                }
            }

            OnAttributesChanged();
        }

        private void CreateParam(string name, string nickname, string description, GH_ParamAccess access, int index, bool optional = true)
        {
            if (Params.Input.Count <= index)
            {
                Params.RegisterInputParam(new Param_Number(), index);
            }
            Params.Input[index].Optional = optional;
            Params.Input[index].Name = name;
            Params.Input[index].NickName = nickname;
            Params.Input[index].Description = description;
            Params.Input[index].Access = access;
            Params.Input[index].ExpirePreview(true);
        }

        private void UnregisterParameterFrom(int index)
        {
            for (int i = index; i < Params.Input.Count; i++)
            {
                IGH_Param p = Params.Input[i--];
                Params.UnregisterParameter(p);
            }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_SectionProperty; }
        }
    }

 
}
