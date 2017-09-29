using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using GHE = BH.Engine.Grasshopper;
using Grasshopper.Kernel.Types;
using System.Reflection;

namespace BH.UI.Alligator.Base
{
    public class SetProperty : GH_Component
    {
        public SetProperty() : base("SetProperty", "SetProperty", "Set property of a BHoM object", "Alligator", "Base") { }

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
            pManager.AddGenericParameter("BHoM object", "object", "BHoM object to convert", GH_ParamAccess.item);
            pManager.AddTextParameter("key", "key", "Property name", GH_ParamAccess.item);
            pManager.AddGenericParameter("value", "value", "Property value", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("BHoM object", "object", "BHoM object to convert", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            object o = GHE.DataUtils.GetGenericData<object>(DA, 0);
            string key = GHE.DataUtils.GetData<string>(DA, 1);
            object value = GHE.DataUtils.GetGenericData<object>(DA, 2);

            object newObject = o;
            System.Reflection.MethodInfo inst = o.GetType().GetMethod("MemberwiseClone", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (inst != null)
                newObject = inst.Invoke(o, null);

            System.Reflection.PropertyInfo prop = newObject.GetType().GetProperty(key);
            if (prop == null)
            {
                DA.SetData(0, null);
            }
            else if (value == null)
            {
                DA.SetData(0, newObject);
            }
            else
            {
                if (typeof(IGH_Goo).IsAssignableFrom(value.GetType()))
                {
                    value = value.GetType().GetProperty("Value").GetValue(value);               
                }
                prop.SetValue(newObject, value);
                DA.SetData(0, newObject);
            }
                
        }
    }
}
