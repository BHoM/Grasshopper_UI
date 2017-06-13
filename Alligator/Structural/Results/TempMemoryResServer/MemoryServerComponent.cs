using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHE = Grasshopper_Engine;
using BHI = BHoM.Structural.Interface;

namespace Alligator.Structural.Results.TempMemoryResServer
{
    public class MemoryServerComponent : GH_Component
    {
        public MemoryServerComponent() : base("MemoryServerComponent", "MemoryServerComponent", "MemoryServerComponent", "Structure", "Results") { }


        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("2BEB2C9D-187B-4E4B-8919-0C5FD22D8B09");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("ResAdapter", "ResAdapter", "ResAdapter", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ResAdapter", "ResAdapter", "ResAdapter", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHI.IResultAdapter app1 = GHE.DataUtils.GetGenericData<BHI.IResultAdapter>(DA, 0);
            MemoryResultServer memServer = new MemoryResultServer(app1);
            memServer.LoadAllForceData();
            DA.SetData(0, memServer);
        }
    }
}