using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BHP = BHoM.Structural.Properties;

namespace Alligator.Structural.Properties
{
    public class CreateCableSectionFromDiameter : GH_Component
    {
        public CreateCableSectionFromDiameter() : base("Custom Cable Property", "SecProp", "Creates a cable section property from database based on cable diameter", "Structure", "Properties") { }
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_Custom_Cable_Section; }
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("759140DC-5909-4B80-B49F-DD7B54F80E62");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Diameter", "D", "The diameter of the cable in [mm]", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Number", "N", "The number of cables", GH_ParamAccess.item, 1);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Prop", "CP", "The created cable property", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double d = 0;
            int nb = 1;

            if(!DA.GetData(0, ref d)) { return; }
            if(!DA.GetData(1, ref nb)) { return; }

            BHP.SectionProperty prop = BHP.SectionProperty.LoadFromCableSectionDBDiameter(d / 1000, nb);

            if (prop == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "The diameter proved do not exist in the database");
                return;
            }

            DA.SetData(0, prop);
        }
    }
}
