using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using System.Linq;
using GHE = Grasshopper_Engine;
using BHE = BHoM.Structural.Elements;
using BHI = BHoM.Structural.Interface;
using GH_IO.Serialization;
using BHoM.Base;

namespace Alligator.Structural.Analysis
{
    public class RunAnalysis : GH_Component
    {
        public RunAnalysis() : base("Run Analysis", "Run", "Run structural analysis", "Structure", "Analysis")
        { }


        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("application", "app", "application to run", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Run", "Run", "Run structural analysis", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {

        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            if (GHE.DataUtils.Run(DA, 1))
            {
                BHI.IElementAdapter app = GHE.DataUtils.GetGenericData<BHI.IElementAdapter>(DA, 0);
                if (app != null)
                {
                    app.Run();
                }
            }

        }

        public override Guid ComponentGuid
        {
            get { return new Guid("AE2C6A3B-5BD8-4EEA-A3DB-D79A3F9395B3"); }
        }


        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_Run_Analasis; }
        }

    }
}