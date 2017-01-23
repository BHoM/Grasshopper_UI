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
    public class SaveFile : GH_Component
    {
        private bool m_success;
        public SaveFile() : base("Save file", "Save", "Save file", "Structure", "Commands")
        {
            m_success = false;
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("f4bc3b2f-ad1c-47ba-9e2d-2bbd72503795");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "Application", "Application to run", GH_ParamAccess.item);
            pManager.AddTextParameter("Filename", "F", "GSA Filename", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Activate", "Go", "Activate", GH_ParamAccess.item);

            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Success", "Success", "Returns wether the analysis was successfully run or not", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (GHE.DataUtils.Run(DA, 2))
            {
                BHI.ICommandAdapter adapter = GHE.DataUtils.GetGenericData<BHI.ICommandAdapter>(DA, 0);
                string fileName = GHE.DataUtils.GetData<string>(DA, 1);

                m_success = adapter.Save(fileName);
            }

            DA.SetData(0, m_success);
        }
    }
}