using BHoM.Structural;
using System;
using Grasshopper.Kernel;
using System.Collections.Generic;

namespace Alligator.Structural.Elements
{
    public class CreateBar : BHoMBaseComponent<Bar>
    {
        public CreateBar() : base("Create Bar", "CreateBar", "Create a BH Bar object", "Alligator", "Structural") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("5FE0E2C4-5E50-410F-BBC7-C255FD1BD2B4");
            }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.bar; }
        }
    }

    public class MultiExportBar : GH_Component
    {
        public MultiExportBar() : base("Multi Export Bar", "ExBar", "Creates or Replaces the geometry of a Bar", "Alligator", "Structural") { }

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
            pManager.AddGenericParameter("Bars", "B", "BHoM bars to export", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Execute", "R", "Generate Bars", GH_ParamAccess.item);

            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("Ids", "Ids", "Bar Numbers", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (Utils.Run(DA, 2))
            {
                IStructuralAdapter app = Utils.GetGenericData<IStructuralAdapter>(DA, 0);
                if (app != null)
                {
                    List<Bar> bars = Utils.GetGenericDataList<Bar>(DA, 1);
                    List<string> ids = null;
                    app.SetBars(bars, out ids);

                    DA.SetDataList(0, ids);
                }
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("2420dc1b-87b3-491a-93fa-1495315ca5a2"); }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.bar; }
        }
    }
}
