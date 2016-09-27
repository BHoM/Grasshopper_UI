using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BHSP = BHoM.Structural.Properties;
using BHSE = BHoM.Structural.Elements;
using GHE = Grasshopper_Engine;


namespace Alligator.Structural.Properties
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateBarAttributes : GH_Component
    {

        public CreateBarAttributes() : base("Bar Attributes", "BarAtt", "Create attributes to be used on a bar", "Structure", "Properties") { }
        
        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_Bar_Attributes; }
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("49E1FC57-285C-403D-8762-4F24C6510CEA");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            int defaultFeIndex = 0;
            int defaultSuIndex = 0;

            pManager.AddGenericParameter("Start Release", "RA", "Release at the start of the beam", GH_ParamAccess.item);
            pManager.AddGenericParameter("End Release", "RA", "Release at the end of the beam", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Bar FEA Type", "FET", GetFeTypeDescription(defaultFeIndex), GH_ParamAccess.item, defaultFeIndex);
            pManager.AddIntegerParameter("Structural usage", "SU", GetStructuralUsageTypeDesciption(defaultSuIndex), GH_ParamAccess.item, defaultSuIndex);

        }


        private static string GetFeTypeDescription(int deafaultIndex)
        {
            Type enumType = typeof(BHoM.Structural.Elements.BarFEAType);

            string desc = "Type of element that will be used in structural analysis softwares.Default set to ";

            desc += GetStringValeEqualsIndexFromEnum(enumType, deafaultIndex);

            return desc;

        }

        private static string GetStructuralUsageTypeDesciption(int deafaultIndex)
        {

            Type enumType = typeof(BHoM.Structural.Elements.BarStructuralUsage);

            string desc = "Sets what the usage will be for the element. Used for post processing. Default set to ";

            desc += GetStringValeEqualsIndexFromEnum(enumType, deafaultIndex);

            return desc;

        }

        private static string GetStringValeEqualsIndexFromEnum(Type enumType, int deafaultIndex)
        {
            string[] names = Enum.GetNames(enumType);
            var indecies = Enum.GetValues(enumType);

            string str = string.Format("{0}",indecies.GetValue(deafaultIndex));

            for (int i = 0; i < names.Length; i++)
            {
                str += String.Format("\n{0} = {1}", names[i], (int)indecies.GetValue(i));
            }

            return str;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Bar Attributes", "A", "Bar attributes", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHSP.NodeConstraint stRel = GHE.DataUtils.GetGenericData<BHSP.NodeConstraint>(DA, 0);
            BHSP.NodeConstraint enRel = GHE.DataUtils.GetGenericData<BHSP.NodeConstraint>(DA, 1);

            int feTypeInd = 0;
            int strUseInd = 0;

            if (!DA.GetData(2, ref feTypeInd)) { return; }
            if (!DA.GetData(3, ref strUseInd)) { return; }

            BHSE.BarFEAType feType = (BHSE.BarFEAType)feTypeInd;
            BHSE.BarStructuralUsage stUse = (BHSE.BarStructuralUsage)strUseInd;

            BHSP.BarRelease rel = new BHSP.BarRelease(stRel, enRel);

            BarAttributesContainer att = new BarAttributesContainer();

            att.BarReleases = rel;
            att.FEAType = feType;
            att.StructuralUsage = stUse;

            DA.SetData(0, att);

        }
    }
}
