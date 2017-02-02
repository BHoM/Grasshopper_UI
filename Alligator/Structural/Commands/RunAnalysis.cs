using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using System.Linq;
using GHE = Grasshopper_Engine;
using BHI = BHoM.Structural.Interface;
using BHL = BHoM.Structural.Loads;
using GH_IO.Serialization;
using BHoM.Base;

namespace Alligator.Structural.Commands
{
    public class RunAnalysis : GH_Component
    {

        public RunAnalysis() : base("Run Analysis", "Run", "Run structural analysis", "Structure", "Commands")
        {

        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("bf6176ed-2b89-4521-96fa-b204c3e1ea1b");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "Application", "Application to run", GH_ParamAccess.item);
            pManager.AddTextParameter("Cases", "Cases", "Load cases or combinations to analyse", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Activate", "Go", "Activate", GH_ParamAccess.item);

            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Success", "Success", "Returns wether the analysis was successfully run or not", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool success = false;

            if (GHE.DataUtils.Run(DA, 2))
            {
                BHI.ICommandAdapter adapter = GHE.DataUtils.GetData<BHI.ICommandAdapter>(DA, 0);
                List<string> cases = GHE.DataUtils.GetDataList<string>(DA, 1);

                success = adapter.Analyse(cases);
            }

            DA.SetData(0, success);
        }
    }
}
