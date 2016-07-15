using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alligator.Global
{
    public class ToJSON : GH_Component
    {
        public ToJSON() : base("ToJSON", "ToJSON", "Convert a BHoM object to a JSON string", "Alligator", "Global") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("C36278A4-DD93-4645-8F69-2DB73167DD32");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("BHoM object", "object", "BHoM object to convert", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("json", "json", "json representation of the BHoM object", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<BHoM.Global.BHoMObject> objects = Utils.GetGenericDataList<BHoM.Global.BHoMObject>(DA, 0);
            DA.SetData(0, BHoM.Global.BHoMJSON.WritePackage(objects.Cast<BHoM.Global.BHoMObject>().ToList()));
        }
    }
}
