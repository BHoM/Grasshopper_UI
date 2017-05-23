using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using GHE = Grasshopper_Engine;

namespace Alligator.Base
{
    public class DuplicateBhomObject : GH_Component
    {
        public DuplicateBhomObject() : base("DuplicateBhomObject", "DuplicateBhomObject", "Duplicates a BHoM object", "Alligator", "Base") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("B9D06721-4EBF-4415-933F-3E56EF2376A6");
            }
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("BHoM object", "object", "BHoM object to duplicate", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Update GUID", "GUID", "give the duplicated BHoM opbject a new GUID", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("BHoM object", "object", "The duplicated object", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHoM.Base.BHoMObject o = GHE.DataUtils.GetGenericData<BHoM.Base.BHoMObject>(DA, 0);
            bool newGuid = GHE.DataUtils.GetData<bool>(DA, 1);

            if (o == null)
                return;

            DA.SetData(0, o.ShallowClone(newGuid));
        }
    }
}
