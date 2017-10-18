using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BH.oM.Structural.Properties;
using BH.UI.Alligator.Base;
using BH.UI.Alligator;

namespace BH.UI.Alligator.Structural.Properties
{
    public class CreateBarRelease : GH_Component
    {
        public CreateBarRelease() : base("Bar Release", "BarRelease", "Creates a release for the end of one bar", "Structure", "Properties") { }
        public override Guid ComponentGuid { get { return new Guid("02D52099-6474-41B1-ACEC-3D335319E9EA"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return Properties.Resources.BHoM_Constraint; } }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "TranslationalX", "TiX", "Translation is x-direction at start node", GH_ParamAccess.item);
            pManager.AddParameter(new BHoMObjectParameter(), "TranslationalY", "TiY", "Translation is y-direction at start node", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "Bar Release", "R", "Release to use on a bar", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            NodeConstraint dofI= new NodeConstraint();
            NodeConstraint dofJ = new NodeConstraint();

            dofI = DA.BH_GetData(0, dofI);
            dofJ = DA.BH_GetData(0, dofJ);

            BarRelease release = new BarRelease();
            release.StartConstraint = dofI;
            release.EndConstraint = dofJ;

            DA.BH_SetData(0, release);
        }
    }
}
