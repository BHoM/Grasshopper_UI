using System;
using Grasshopper.Kernel;
using Alligator.Components;
using System.Collections.Generic;
using BHE = BHoM.Structural.Elements;
using BHP = BHoM.Structural.Properties;
using Grasshopper_Engine.Components;
using System.Windows.Forms;
using Grasshopper.Kernel.Parameters;

namespace Alligator.Structural.Properties
{
    public class CreateSectionProperty : BHoMBaseComponent<BHP.SectionProperty>
    {

        public CreateSectionProperty() : base("Create Section Property", "CreateSectionProperty", "Create a BH Section property object", "Structure", "Properties")
        { 
            
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Name of the section Property", GH_ParamAccess.item);
            pManager.AddGenericParameter("CustomData", "CustomData", "CustomData", GH_ParamAccess.item);
            Params.Input[1].Optional = true;

            AppendEnumOptions("ShapeType", typeof(BHP.ShapeType));
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string propertyName = "";
            BHP.SectionProperty barProperty = null;
            
            DA.GetData<string>(0, ref propertyName);

            barProperty = BHP.SectionProperty.LoadFromDB(propertyName);

            if (barProperty == null)
            {
                double[] dimensions = new double[12];
                for (int i = 2; i < Params.Input.Count; i++)
                {
                    DA.GetData<double>(i, ref dimensions[i - 2]);                   
                }
             
                switch ((BHP.ShapeType)m_SelectedOption[0])
                {
                    case BHP.ShapeType.ISection:
                        barProperty = BHP.SectionProperty.CreateISection(dimensions[1], dimensions[2], dimensions[0], dimensions[3], dimensions[4], dimensions[5], dimensions[6], dimensions[7]);
                        double test = barProperty.Ix;
                        break;
                    case BHP.ShapeType.Box:
                        barProperty = BHP.SectionProperty.CreateBoxSection(dimensions[1], dimensions[0], dimensions[2], dimensions[3], dimensions[4], dimensions[5]);
                        break;
                    case BHP.ShapeType.Rectangle:
                        barProperty = BHP.SectionProperty.CreateRectangularSection(dimensions[1], dimensions[0], dimensions[2]);
                        break;
                    case BHP.ShapeType.Tee:
                        barProperty = BHP.SectionProperty.CreateTee(dimensions[1], dimensions[0], dimensions[2], dimensions[3], dimensions[4], dimensions[5]);
                        break;
                    case BHP.ShapeType.Circle:
                        barProperty = BHP.SectionProperty.CreateCircularSection(dimensions[0]);
                        break;
                    case BHP.ShapeType.Tube:
                        barProperty = BHP.SectionProperty.CreateTubeSection(dimensions[0], dimensions[1]);
                        break;

                }
                barProperty.Name = propertyName;
            }
            barProperty.CalculateSection();
            DA.SetData(0, barProperty);
            SetGeometry(barProperty, DA);
        }

        protected override void UpdateInput(object enumSelection)
        {
            if (enumSelection.GetType() == typeof(BHP.ShapeType))
            {
                switch ((BHP.ShapeType)enumSelection)
                {
                    case BHP.ShapeType.ISection:
                        CreateParam("Depth", "Depth", "Total Depth (m)", GH_ParamAccess.item, 2);
                        CreateParam("Top Flange Width", "B1", "Width of Top flange (m)", GH_ParamAccess.item, 3);
                        CreateParam("Bottom Flange Width", "B2", "Width of Bottom Flange (m)", GH_ParamAccess.item, 4);
                        CreateParam("Top Flange thickness", "Tf", "Thickness of flange (m)", GH_ParamAccess.item, 5);
                        CreateParam("Bottom Flange thickness", "Tb", "Thickness of flange (m)", GH_ParamAccess.item, 6);
                        CreateParam("Web thickness", "Tw", "Thickness of Web (m)", GH_ParamAccess.item, 7);
                        CreateParam("Inner Fillet radius", "Ri", "Inner Fillet Radius (m)", GH_ParamAccess.item, 8);
                        CreateParam("Outter Fillet radius", "Ro", "Outer Fillet Radius (m)", GH_ParamAccess.item, 9);
                        CreateParam("Rotation", "A", "Axis rotation", GH_ParamAccess.item, 10);
                        break;
                    case BHP.ShapeType.Box:
                    case BHoM.Structural.Properties.ShapeType.Tee:
                        CreateParam("Width", "Width", "Total Width (m)", GH_ParamAccess.item, 2);
                        CreateParam("Depth", "Depth", "Total Depth (m)", GH_ParamAccess.item, 3);
                        CreateParam("Flange thickness", "Tf", "Thickness of flange (m)", GH_ParamAccess.item, 4);
                        CreateParam("Web thickness", "Tw", "Thickness of Web (m)", GH_ParamAccess.item, 5);
                        CreateParam("Inner Fillet radius", "Ri", "Inner Fillet Radius (m)", GH_ParamAccess.item, 6);
                        CreateParam("Outter Fillet radius", "Ro", "Outer Fillet Radius (m)", GH_ParamAccess.item, 7);
                        CreateParam("Rotation", "A", "Axis rotation", GH_ParamAccess.item, 8);
                        UnregisterParameterFrom(9);
                        break;
                    case BHP.ShapeType.Rectangle:
                        CreateParam("Width", "Width", "Total Width (m)", GH_ParamAccess.item, 2);
                        CreateParam("Depth", "Depth", "Total Depth (m)", GH_ParamAccess.item, 3);
                        CreateParam("Edge Radius", "D", "Total Depth (m)", GH_ParamAccess.item, 4);
                        UnregisterParameterFrom(5);
                        break;
                    case BHP.ShapeType.Circle:
                        CreateParam("Diameter", "Diameter", "Total Diameter (m)", GH_ParamAccess.item, 2);
                        UnregisterParameterFrom(3);
                        break;
                    case BHP.ShapeType.Tube:
                        CreateParam("Outer Diameter", "Diameter", "Total Diameter (m)", GH_ParamAccess.item, 2);
                        CreateParam("Thickness", "Thickness", "Thickness (m)", GH_ParamAccess.item, 3);
                        UnregisterParameterFrom(4);
                        break;
                    default:
                        CreateParam("Width", "Width", "Total Width (m)", GH_ParamAccess.item, 2);
                        CreateParam("Depth", "Depth", "Total Depth (m)", GH_ParamAccess.item, 3);
                        UnregisterParameterFrom(4);
                        break;
                }
            }
        }

        private void CreateParam(string name, string nickname, string description, GH_ParamAccess access, int index)
        {
            if (Params.Input.Count <= index)
            {
                Params.RegisterInputParam(new Param_Number(), index);
            }
            Params.Input[index].Optional = true;
            Params.Input[index].Name = name;
            Params.Input[index].NickName = nickname;
            Params.Input[index].Description = description;
            Params.Input[index].Access = access;
        }

        private void UnregisterParameterFrom(int index)
        {
            for (int i = index; i < Params.Input.Count; i++)
            {
                IGH_Param p = Params.Input[i--];
                Params.UnregisterParameter(p);
            }
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("95916851-2b86-46ab-b4e3-d839b817dbb4");
            }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_SectionProperty; }
        }
    }

 
}
