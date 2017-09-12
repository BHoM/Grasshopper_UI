using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Alligator.Components;
using GHE = Grasshopper_Engine;
using BHL = BH.oM.Structural.Loads;
using BHI = BH.oM.Structural.Interface;
using Grasshopper_Engine.Components;

namespace Alligator.Structural.Loads
{
    public class CreateLoadcase : BHoMBaseComponent<BHL.Loadcase>
    {
        public CreateLoadcase() : base("Create Loadcase", "CreateLoadcase", "Create a BH Loadcase object", "Structure", "Loads") { }

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
            get { return Alligator.Properties.Resources.BH.oM_LoadCase; }
        }
    }

    public class ExportLoadCase : GH_Component
    {
        public ExportLoadCase() : base("Export Loadcase", "ExLoadcase", "Creates or Replaces loadcase", "Structure", "Loads") { }
        
        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BH.oM_LoadCase_Export; }
        }

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
            pManager.AddGenericParameter("Loadcase", "L", "BH.oM loadcase to export", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Execute", "R", "Generate loadcases", GH_ParamAccess.item);

            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Success", "Success", "Return wheather the operation was succuessful or not", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool success = false;

            if (GHE.DataUtils.Run(DA, 2))
            {
                BHI.IElementAdapter app = GHE.DataUtils.GetGenericData<BHI.IElementAdapter>(DA, 0);
                if (app != null)
                {
                    List<BHL.ICase> loadcases = GHE.DataUtils.GetGenericDataList<BHL.ICase>(DA, 1);
                    app.SetLoadcases(loadcases);

                    success = true;
                }
            }
            DA.SetData(0, success);
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("824e4efc-c4ae-4ac0-a4c7-983547b42365"); }
        }
    }
}
