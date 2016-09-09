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
    public enum PanelClassType
    {
        Constant,
        Waffle,
        Ribbed
    }

    public class CreatePanelProperty : BHoMBaseComponent<BHP.PanelProperty>
    {

        public CreatePanelProperty() : base("Create Panel Property", "CreatePanelProperty", "Create a BH panel property object", "Structure", "Properties")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Name of the section Property", GH_ParamAccess.item);
            pManager.AddGenericParameter("CustomData", "CustomData", "CustomData", GH_ParamAccess.item);
            Params.Input[1].Optional = true;

            AppendEnumOptions("PropertyType", typeof(PanelClassType));
            AppendEnumOptions("PanelType", typeof(BHP.PanelType));
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string propertyName = "";
            BHP.PanelProperty panelProperty = null;

            DA.GetData<string>(0, ref propertyName);

            if (panelProperty == null)
            {
                double[] dimensions = new double[8];
                for (int i = 2; i < Params.Input.Count; i++)
                {
                    if (Params.Input[i].SourceCount > 0)
                    {
                        DA.GetData<double>(i, ref dimensions[i - 2]);
                    }
                }

                BHP.PanelType section = (BHP.PanelType)m_SelectedOption[1];
                PanelClassType classType = (PanelClassType)m_SelectedOption[0];
                switch (classType)
                {
                    case PanelClassType.Constant:
                        panelProperty = new BHP.ConstantThickness(propertyName, dimensions[0], section);
                        break;
                    case PanelClassType.Ribbed:
                        BHP.PanelDirection dir = (BHP.PanelDirection)m_SelectedOption[1];
                        panelProperty = new BHP.Ribbed(propertyName, dimensions[0], dimensions[1], dimensions[2], dimensions[3], dir);
                        break;
                    case PanelClassType.Waffle:
                        panelProperty = new BHP.Waffle(propertyName, dimensions[0], dimensions[1], dimensions[2], dimensions[3], dimensions[4], dimensions[5], dimensions[6]);
                        break;
                }
            }

            DA.SetData(0, panelProperty);
        }

        protected override void UpdateInput(object enumSelection)
        {
            if (m_Options.Count > 2) m_Options.RemoveAt(2);
            if (m_OptionType.Count > 2) m_OptionType.RemoveAt(2);
            if (m_SelectedOption.Count > 2) m_SelectedOption.RemoveAt(2);
            if (enumSelection.GetType() == typeof(PanelClassType))
            {
                switch ((PanelClassType)enumSelection)
                {
                    case PanelClassType.Constant:
                        CreateParam("Thickness", "Thickness", "Total Thickness (m)", GH_ParamAccess.item, 2);
                        UnregisterParameterFrom(3);
                        break;
                    case PanelClassType.Ribbed:
                        CreateParam("Thickness", "Thickness", "Total Thickness (m)", GH_ParamAccess.item, 2);
                        CreateParam("Total Depth", "Total Depth", "Total Depth of Ribs (m)", GH_ParamAccess.item, 3);
                        CreateParam("Stem Width", "Stem Width", "Width of stem (m)", GH_ParamAccess.item, 4);
                        CreateParam("Spacing", "Spacing", "Spacing (m)", GH_ParamAccess.item, 5);
                        UnregisterParameterFrom(6);
                        UpdateInput(typeof(BHP.PanelDirection));
                        break;
                    case PanelClassType.Waffle:
                        CreateParam("Thickness", "Thickness", "Total Thickness (m)", GH_ParamAccess.item, 2);
                        CreateParam("Total Depth X", "Total Depth X", "Total Depth of Ribs in X direction (m)", GH_ParamAccess.item, 3);
                        CreateParam("Total Depth Y", "Total Depth Y", "Total Depth of Ribs in Y direction (m)", GH_ParamAccess.item, 4);
                        CreateParam("Stem Width X", "Stem Width X", "Width of stem in X direction (m)", GH_ParamAccess.item, 5);
                        CreateParam("Stem Width Y", "Stem Width Y", "Width of stem in Y direction (m)", GH_ParamAccess.item, 6);
                        CreateParam("Spacing X", "Spacing X", "Spacing X (m)", GH_ParamAccess.item, 7);
                        CreateParam("Spacing Y", "Spacing Y", "Spacing Y (m)", GH_ParamAccess.item, 8);
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
                return new Guid("95916851-223a-46ab-b4e3-d839b817dbb4");
            }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_SectionProperty; }
        }
    }
}

