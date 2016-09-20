using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BHSP = BHoM.Structural.Properties;
using BHSE = BHoM.Structural.Elements;


namespace Alligator.Structural.Properties
{
    public class CreateBarRelease : GH_Component
    {
        public CreateBarRelease() : base("Bar Release", "BR", "Creates a release for the end of one bar", "Structure", "Properties") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("02D52099-6474-41B1-ACEC-3D335319E9EA");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Tranlation x", "x", "Translation is x-direction", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Tranlation y", "y", "Translation is y-direction", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Tranlation z", "z", "Translation is z-direction", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Rotation x", "xx", "Rotation around the x-axis", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Rotation y", "yy", "Rotation around the y-axis", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Rotation z", "zz", "Rotation around the z-axis", GH_ParamAccess.item);
            pManager.AddTextParameter("Name", "N", "Name of the release", GH_ParamAccess.item, "Release");

        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Bar Release", "R", "Release to use on a bar", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //bool x, y, z, xx, yy, zz;
            //x = y = z = xx = yy = zz = false;

            bool[] fixities = new bool[6];

            for (int i = 0; i < 6; i++)
            {
                bool flag = false;
                if (!DA.GetData(i, ref flag)) { return; }

                fixities[i] = flag;
            }

            string name = "";

            if(!DA.GetData(6, ref name)) { return; }

            BHSP.NodeConstraint rel = new BHoM.Structural.Properties.NodeConstraint(name, fixities, new double[] { 0, 0, 0, 0, 0, 0 });


            DA.SetData(0, rel);
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_Constraint; }
        }
    }
}
