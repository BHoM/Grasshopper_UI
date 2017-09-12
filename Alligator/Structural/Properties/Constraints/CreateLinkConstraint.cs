using System;
using Grasshopper.Kernel;
using Alligator.Components;
using System.Collections.Generic;
using BHE = BH.oM.Structural.Elements;
using BHP = BH.oM.Structural.Properties;
using Grasshopper_Engine;
using Grasshopper_Engine.Components;
using System.Windows.Forms;
using Grasshopper.Kernel.Parameters;
using System.Reflection;
using Grasshopper_Engine;


namespace Alligator.Structural.Properties.Constraints
{
    public class CreateLinkConstraint : BHoMBaseComponent<BHP.LinkConstraint>
    {

        public CreateLinkConstraint() : base("Link Constraint", "LinkConstriant", "Creates a restraint for a rigid link", "Structure", "Properties") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("A5BF39B1-A333-461E-BF36-E36707383740");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Name of the restraint", GH_ParamAccess.item);
            pManager[0].Optional = true;

            AppendEnumOptions("Linkage Type", typeof(BHP.LinkageType));
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Link constraint", "LnkCnst", "Created link constraint to use with rigid link", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            BHP.LinkConstraint linkConst = null;

            switch ((BHP.LinkageType)m_SelectedOption[0])
            {
                case BHP.LinkageType.All:
                    linkConst = BHP.LinkConstraint.Fixed;
                    break;
                case BHP.LinkageType.AllPinned:
                    linkConst = BHP.LinkConstraint.Pinned;
                    break;
                case BHP.LinkageType.XYPlane:
                    linkConst = BHP.LinkConstraint.XYPlane;
                    break;
                case BHP.LinkageType.YZPlane:
                    linkConst = BHP.LinkConstraint.YZPlane;
                    break;
                case BHP.LinkageType.ZXPlane:
                    linkConst = BHP.LinkConstraint.ZXPlane;
                    break;
                case BHP.LinkageType.XYPlanePin:
                    linkConst = BHP.LinkConstraint.XYPlanePin;
                    break;
                case BHP.LinkageType.YZPlanePin:
                    linkConst = BHP.LinkConstraint.YZPlanePin;
                    break;
                case BHP.LinkageType.ZXPlanePin:
                    linkConst = BHP.LinkConstraint.ZXPlanePin;
                    break;
                case BHP.LinkageType.XPlate:
                    linkConst = BHP.LinkConstraint.XPlate;
                    break;
                case BHP.LinkageType.YPlate:
                    linkConst = BHP.LinkConstraint.YPlate;
                    break;
                case BHP.LinkageType.ZPlate:
                    linkConst = BHP.LinkConstraint.ZPlate;
                    break;
                case BHP.LinkageType.XPlatePin:
                    linkConst = BHP.LinkConstraint.XPlatePin;
                    break;
                case BHP.LinkageType.YPlatePin:
                    linkConst = BHP.LinkConstraint.YPlatePin;
                    break;
                case BHP.LinkageType.ZPlatePin:
                    linkConst = BHP.LinkConstraint.ZPlatePin;
                    break;
                case BHP.LinkageType.Custom:
                    bool[] fixities = new bool[12];
                    for (int i = 1; i < 13; i++)
                    {
                        bool data = false;
                        if (DA.GetData(i, ref data))
                        {
                            fixities[i - 1] = data;
                        }
                    }

                    linkConst = new BHP.LinkConstraint(fixities);
                    break;
            }

            string name = "";

            if(DA.GetData(0, ref name))
                linkConst.Name = name;

            DA.SetData(0, linkConst);

        }

        protected override void UpdateInput(object enumSelection)
        {
            int firstParamIndex = 1;

            if (enumSelection.GetType() == typeof(BHP.LinkageType))
            {
                switch ((BHP.LinkageType)enumSelection)
                {
                    case BHP.LinkageType.Custom:
                        CreateParam("X to X", "XtoX", "Link x-translation to x-translation", GH_ParamAccess.item, firstParamIndex);
                        CreateParam("X to YY", "XtoYY", "Link x-translation to y-rotation", GH_ParamAccess.item, firstParamIndex+1);
                        CreateParam("X to ZZ", "XtoZZ", "Link x-translation to z-rotation", GH_ParamAccess.item, firstParamIndex+2);
                        CreateParam("Y to Y", "YtoY", "Link y-translation to y-translation", GH_ParamAccess.item, firstParamIndex+3);
                        CreateParam("Y to XX", "YtoXX", "Link y-translation to x-rotation", GH_ParamAccess.item, firstParamIndex+4);
                        CreateParam("Y to ZZ", "YtoZZ", "Link y-translation to z-rotation", GH_ParamAccess.item, firstParamIndex+5);
                        CreateParam("Z to Z", "ZtoZ", "Link z-translation to z-translation", GH_ParamAccess.item, firstParamIndex+6);
                        CreateParam("Z to XX", "ZtoXX", "Link z-translation to x-rotation", GH_ParamAccess.item, firstParamIndex+7);
                        CreateParam("Z to YY", "ZtoYY", "Link z-translation to y-rotation", GH_ParamAccess.item, firstParamIndex+8);
                        CreateParam("XX to XX", "XXtoXX", "Link x-rotation to x-rotation", GH_ParamAccess.item, firstParamIndex+9);
                        CreateParam("YY to YY", "YYtoYY", "Link y-rotation to y-rotation", GH_ParamAccess.item, firstParamIndex+10);
                        CreateParam("ZZ to ZZ", "ZZtoZZ", "Link z-rotation to z-rotation", GH_ParamAccess.item, firstParamIndex+11);
                        break;
                    case BHP.LinkageType.All:
                    case BHP.LinkageType.AllPinned:
                    default:
                        UnregisterParameterFrom(firstParamIndex);
                        break;
                }
            }
            OnAttributesChanged();
        }

        private void CreateParam(string name, string nickname, string description, GH_ParamAccess access, int index, bool optional = true)
        {
            if (Params.Input.Count <= index)
            {
                Params.RegisterInputParam(new Param_Boolean(), index);
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

    }
}
