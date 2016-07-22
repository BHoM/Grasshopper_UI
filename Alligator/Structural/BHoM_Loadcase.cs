using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHoM.Structural.Loads;
using BHoM.Structural;
using Grasshopper.Kernel;

namespace Alligator.Structural.Loads
{
    public class CreateLoadcase : BHoMBaseComponent<Loadcase>
    {
        public CreateLoadcase() : base("Create Loadcase", "CreateLoadcase", "Create a BH Loadcase object", "Alligator", "Structural") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("3aeb1df2-16b0-477e-814a-59743a10062c");
            }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.bar; }
        }
    }

    public class MultiExportLoadCase : GH_Component
    {
        public MultiExportLoadCase() : base("Multi Export Loadcase", "ExLoadcase", "Creates or Replaces loadcase", "Alligator", "Structural") { }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "App", "Application to export loadcases to", GH_ParamAccess.item);
            pManager.AddGenericParameter("Loadcase", "L", "BHoM loadcase to export", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Execute", "R", "Generate loadcases", GH_ParamAccess.item);

            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (Utils.Run(DA, 2))
            {
                IStructuralAdapter app = Utils.GetGenericData<IStructuralAdapter>(DA, 0);
                if (app != null)
                {
                    List<ICase> loadcases = Utils.GetGenericDataList<ICase>(DA, 1);
                    app.SetLoadcases(loadcases);

                }
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("824e4efc-c4ae-4ac0-a4c7-983547b42365"); }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.loadcase; }
        }
    }
}
