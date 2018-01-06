using System;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using BH.oM.Base;
using BH.Engine.Reflection;

namespace BH.UI.Alligator.Base
{
    public class SetProperty : GH_Component
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Properties.Resources.BHoM_SetProperty; 

        public override Guid ComponentGuid { get; } = new Guid("E3C42F6C-15AC-4FBA-8BCC-F3E773B1C1D8"); 

        public override GH_Exposure Exposure { get; } = GH_Exposure.tertiary; 


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public SetProperty() : base("Set Property", "SetProperty", "Returns copy of object with a set property", "Alligator", " Engine") { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "BHoM object", "object", "BHoM object to convert", GH_ParamAccess.item);
            pManager.AddTextParameter("key", "key", "Property name", GH_ParamAccess.item);
            pManager.AddGenericParameter("value", "value", "Property value", GH_ParamAccess.item);
        }

        /*******************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "BHoM object", "object", "BHoM object to convert", GH_ParamAccess.item);
        }

        /*******************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHoMObject obj = new BHoMObject();
            string key = "";
            object value = default(object);

            DA.GetData(0, ref obj);
            DA.GetData(1, ref key);
            DA.GetData(2, ref value);

            while (value is IGH_Goo)
                value = ((IGH_Goo)value).ScriptVariable();

            if (value.GetType().Namespace.StartsWith("Rhino.Geometry"))
                value = Engine.Rhinoceros.Convert.ToBHoM(value as dynamic);

            BHoMObject newObject = obj.GetShallowClone();
            newObject.SetPropertyValue(key, value);

            DA.SetData(0, newObject);
        }

        /*******************************************/
    }
}
