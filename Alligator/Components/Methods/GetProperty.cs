using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BH.oM.Base;
using BH.UI.Alligator;
using BH.oM.Geometry;
using BH.Adapter.Rhinoceros;
using System.Collections;

namespace BH.UI.Alligator.Base
{
    public class GetProperty : GH_Component
    {
        public GetProperty() : base("Get Property", "GetProperty", "Get property of a BHoM object from the property name", "Alligator", "Base") { }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return Properties.Resources.BHoM_GetProperty; } }

        public override Guid ComponentGuid { get { return new Guid("E14EF77D-4F09-4CFB-AB75-F9B723212D00"); } }
        public override GH_Exposure Exposure { get { return GH_Exposure.tertiary; } }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "BHoM object", "object", "BHoM object to convert", GH_ParamAccess.item);
            pManager.AddTextParameter("key", "key", "Property name", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Property value", "value", "Value of the property", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHoMObject obj = new BHoMObject();
            string key = "";

            DA.GetData(0, ref obj);
            DA.GetData(1, ref key);

            object result = BH.Engine.Reflection.Query.GetPropertyValue(obj, key);

            if (result is IEnumerable && !(result is string) && !(result is IDictionary))
            {
                if (typeof(IBHoMGeometry).IsAssignableFrom(((IEnumerable)result).GetType().GenericTypeArguments.First()))
                    DA.SetDataList(0, ((IEnumerable)result).Cast<IBHoMGeometry>().Select(x => x.IToRhino()));
                else
                    DA.SetDataList(0, result as IEnumerable);
            }
            else
            {
                if (result is IBHoMGeometry)
                    DA.SetData(0, ((IBHoMGeometry)result).IToRhino());
                else
                    DA.SetData(0, result);
            }
        }
    }
}
