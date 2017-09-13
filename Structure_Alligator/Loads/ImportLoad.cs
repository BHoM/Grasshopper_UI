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
    public class ImportLoad : GH_Component
    {
        public ImportLoad() : base("Import Load", "GetLoad", "Get the load of the input loadcases", "Structure", "Loads")
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
            if (GHE.DataUtils.Run(DA, 2))
            {
                BHI.IElementAdapter app = GHE.DataUtils.GetGenericData<BHI.IElementAdapter>(DA, 0);
                if (app != null)
                {
                    List<Loadcase> loadcases = GHE.DataUtils.GetGenericDataList<BH.oM.Structural.Loads.Loadcase>(DA, 1);
                    List<ILoad> loads = null;

                    app.GetLoads(out loads, loadcases);

                    DA.SetDataList(0, loads);
                }
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "Application", "Application to get elements from", GH_ParamAccess.item);
            pManager.AddGenericParameter("Loadcases", "Loadcases", "Loadcases to get loads", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Activate", "Activate", "Activate", GH_ParamAccess.item);           
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Loads", "Loads", "Load values assigned to objects", GH_ParamAccess.list);
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("1355dc1b-87b3-491a-93fa-1495315aa5a2"); }
        }
                    /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Structural.Properties.Resources.BHoM_Bar_Force_Import; }
        }

    }
}
