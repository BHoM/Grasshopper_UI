using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using GHE = Grasshopper_Engine;

namespace Alligator.Base
{
    public class GetProperty : GH_Component
    {
        public GetProperty() : base("GetProperty", "GetProperty", "Get property of a BH.oM object from the property name", "Alligator", "Base") { }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BH.oM_GetProperty; }
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("E14EF77D-4F09-4CFB-AB75-F9B723212D00");
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
            pManager.AddGenericParameter("BH.oM object", "object", "BH.oM object to convert", GH_ParamAccess.item);
            pManager.AddTextParameter("key", "key", "Property name", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Property value", "value", "Value of the property", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            object o = GHE.DataUtils.GetGenericData<object>(DA, 0);
            string key = GHE.DataUtils.GetData<string>(DA, 1);

            System.Reflection.PropertyInfo prop = o.GetType().GetProperty(key);
            if (prop == null)
                DA.SetData(0, null);
            else
                DA.SetData(0, prop.GetValue(o));
        }
    }
}
