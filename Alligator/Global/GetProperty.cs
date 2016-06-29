using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;

namespace Alligator.Global
{
    public class GetProperty : GH_Component
    {
        public GetProperty() : base("GetProperty", "GetProperty", "Get property of a BHoM object from the property name", "Alligator", "Global") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("E14EF77D-4F09-4CFB-AB75-F9B723212D00");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("BHoM object", "object", "BHoM object to convert", GH_ParamAccess.item);
            pManager.AddTextParameter("key", "key", "Property name", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Property value", "value", "Value of the property", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            object o = Utils.GetGenericData<object>(DA, 0);
            string key = Utils.GetData<string>(DA, 1);

            System.Reflection.PropertyInfo prop = o.GetType().GetProperty(key);
            if (prop == null)
                DA.SetData(0, null);
            else
                DA.SetData(0, prop.GetValue(o));
        }
    }
}
