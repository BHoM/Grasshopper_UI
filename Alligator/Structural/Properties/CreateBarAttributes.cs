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
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("49E1FC57-285C-403D-8762-4F24C6510CEA");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Start Release", "RA", "Release at the start of the beam", GH_ParamAccess.item);
            pManager.AddGenericParameter("End Release", "RA", "Release at the end of the beam", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Bar FEA Type", "FET", "Type of element that will be used in structural analysis softwares. Default set to beam. Beam = 0, Bar = 1, Tie = 2 Strut = 3", GH_ParamAccess.item, 0);
            pManager.AddIntegerParameter("Structural usage", "ST", "Sets what the usage will be for the element. Used for post processing", GH_ParamAccess.item, 0);

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
