using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Structural.Loads;
using Grasshopper.Kernel;
using BH.UI.Alligator.Components;
using GHE = BH.Engine.Grasshopper;
using BHE = BH.oM.Structural.Elements;
using BHI = BH.oM.Structural.Interface;
using BH.Engine.Grasshopper.Components;
using Grasshopper;

namespace BH.UI.Alligator.Structural.Loads
{
    public class ImportLoadcase : GH_Component
    {
        public ImportLoadcase() : base("Import Loadcases", "GetLoadcase", "Get the load of the input loadcases", "Structure", "Loads")
        {

        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.tertiary;
            }
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool success = false;
            if (GHE.DataUtils.Run(DA, 1))
            {
                BHI.IElementAdapter app = GHE.DataUtils.GetGenericData<BHI.IElementAdapter>(DA, 0);
                if (app != null)
                {
                    List<ICase> loadcases = null;

                    DA.SetDataList(0, app.GetLoadcases(out loadcases));

                    DA.SetDataList(1, loadcases);
                }
                success = true;
                DA.SetData(2, success);
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "Application", "Application to get elements from", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Activate", "Activate", "Activate", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Ids", "Id", "Id of loadcases", GH_ParamAccess.list);
            pManager.AddGenericParameter("Loadcases", "Loadcases", "Loadcases", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Success", "Success", "Success", GH_ParamAccess.item);
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("1355dc1b-87b3-491a-93fa-1225315aa5a2"); }
        }

                    /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Structural.Properties.Resources.BHoM_Loadcase_Import; }
        }


    }
}
