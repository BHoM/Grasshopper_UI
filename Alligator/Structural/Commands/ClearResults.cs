using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using System.Linq;
using GHE = Grasshopper_Engine;
using BHI = BH.oM.Structural.Interface;
using BHL = BH.oM.Structural.Loads;
using GH_IO.Serialization;
using BH.oM.Base;

namespace Alligator.Structural.Commands
{
    public class ClearResults : GH_Component
    {
        public ClearResults() : base("Clear Results", "Clear Results", "Clear Results", "Structure", "Commands")
        {
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("9104bed5-7079-4d88-81bc-fcff184a9af0");
            }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BH.oM_App_Clean; }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "Application", "Application to run", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Activate", "Go", "Activate", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Success", "Success", "Returns wether the analysis was successfully run or not", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool success = false;
            if (GHE.DataUtils.Run(DA, 1))
            {
                BHI.ICommandAdapter adapter = GHE.DataUtils.GetGenericData<BHI.ICommandAdapter>(DA, 0);

                success = adapter.ClearResults();
            }

            DA.SetData(0, success);
        }
    }
}
