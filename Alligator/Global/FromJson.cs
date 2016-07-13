using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;

namespace Alligator.Global
{
    public class FromJSON : GH_Component
    {
        public FromJSON() : base("FromJSON", "FromJSON", "Create a BHoM object from a JSON string", "Alligator", "Global") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("45912883-EE6F-49F4-BEBA-4A123EC2370C");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("json", "json", "json representation of the BHoM object", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("BHoM object", "object", "Resulting BHoM object", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string json = ""; // Utils.GetGenericData<string>(DA, 0);
            DA.GetData<string>(0, ref json);
            DA.SetDataList(0, BHoM.Global.BHoMJSON.ReadPackage(json));
        }
    }
}
