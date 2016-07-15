using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BHoM.Structural;
using BHoM.Structural.SectionProperties;
using GSAToolkit;
using Interop.gsa_8_7;

namespace Alligator.GSA
{
    public class CreateSectionProperty : GH_Component
    {
        public CreateSectionProperty() : base("Create Section Properties", "CreateProp", "Create section properties in GSA", "GSA", "Elements") { }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "App", "Application to import nodes from", GH_ParamAccess.item);
            pManager.AddGenericParameter("SectionProperty", "Prop", "BHoM section property", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Execute", "R", "Generate Bars", GH_ParamAccess.item);

            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("Names", "Names", "Section Property Names", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (Utils.Run(DA, 2))
            {
                GSAAdapter app = Utils.GetGenericData<GSAAdapter>(DA, 0);
                if (app != null)
                {
                    SectionProperty secProp = Utils.GetData<SectionProperty>(DA, 1);
                    //PropertyIO.SetSectionProperty(app, secProp);

                    DA.SetData(1, secProp.Name);
                }
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("41092770-8613-41b9-938d-e50507ec44b9"); }
        }

        /// <summary> Icon(24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return GSA.Properties.Resources.sectionproperty; }
        }

    }
}
