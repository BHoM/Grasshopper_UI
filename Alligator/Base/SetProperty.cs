using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using BH.oM.Base;
using BH.UI.Alligator.Query;
using BH.Engine.Reflection;

namespace BH.UI.Alligator.Base
{
    public class SetProperty : GH_Component
    {
        public SetProperty() : base("SetProperty", "SetProperty", "Returns copy of object with a set property", "Alligator", "Base") { }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_SetProperty; }
        }
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("E3C42F6C-15AC-4FBA-8BCC-F3E773B1C1D8");
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
            pManager.AddParameter(new BHoMObjectParameter(), "BHoM object", "object", "BHoM object to convert", GH_ParamAccess.item);
            pManager.AddTextParameter("key", "key", "Property name", GH_ParamAccess.item);
            pManager.AddGenericParameter("value", "value", "Property value", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "BHoM object", "object", "BHoM object to convert", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHoMObject obj = new BHoMObject();
            string key = "";
            object value = default(object);

            obj = DA.BH_GetData(0, obj);
            key = DA.BH_GetData(1, key);
            value = DA.BH_GetData(2, value);

            BHoMObject newObject = obj.GetShallowClone();
            newObject.SetPropertyValue(key, value);

            DA.BH_SetData(0, newObject);
        }
    }
}
