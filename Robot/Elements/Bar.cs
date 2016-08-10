using BHoM.Structural;
using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using GHE = Grasshopper_Engine;
using BHI = BHoM.Structural.Interface;
using BHE = BHoM.Structural.Elements;


namespace Alligator.Robot.Elements
{
    public class SetBar : GH_Component
    {
        public SetBar() : base("Set Bar", "ExBar", "Creates or Replaces the geometry of a Bar", "Robot", "Elements") { }

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
            if (GHE.DataUtils.Run(DA, 2))
            {
                BHI.IElementAdapter app = GHE.DataUtils.GetGenericData<BHI.IElementAdapter>(DA, 0);
                if (app != null)
                {
                    List<BHE.Bar> bars = GHE.DataUtils.GetGenericDataList<BHE.Bar>(DA, 1);
                    List<string> ids = null;
                    app.SetBars(bars, out ids);

                    DA.SetDataList(0, ids);
                }
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("2220dc1b-87b3-491a-93fa-1495315ca5a2"); }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Robot.Properties.Resources.bar; }
        }
    }
}
