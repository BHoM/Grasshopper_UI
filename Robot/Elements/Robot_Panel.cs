using BHoM.Structural;
using Grasshopper.Kernel;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alligator.Robot.Elements
{
    public class SetPanel : GH_Component
    {
        public SetPanel() : base("Set Panel", "ExPanel", "Create a panel", "Robot", "Elements") { }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "App", "Application to import panels from", GH_ParamAccess.item);
            pManager.AddGenericParameter("Panel", "N", "BHoM Panel", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Execute", "R", "Set Geometry", GH_ParamAccess.item);
            pManager[2].AddVolatileData(new Grasshopper.Kernel.Data.GH_Path(0), 0, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Id", "Id", "Panel Id", GH_ParamAccess.list); ;
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (Utils.Run(DA, 2))
            {
                IStructuralAdapter app = Utils.GetGenericData<IStructuralAdapter>(DA, 0);
                if (app != null)
                {
                    List<Panel> panels = Utils.GetGenericDataList<Panel>(DA, 1);
                    List<string> ids = null;
                    app.SetPanels(panels, out ids);

                    DA.SetDataList(0, ids);
                }
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("61B66CED-C4D5-45F1-8EB4-A6AA3EDD6C88"); }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Robot.Properties.Resources.panel; }
        }
    }
}
