using System;
using Grasshopper.Kernel;
using BH.oM.Base;

namespace BH.UI.Alligator.Base
{
    public class ToJson : GH_Component
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Properties.Resources.ToJson; 

        public override Guid ComponentGuid { get; } = new Guid("3564A67C-3444-4A9B-AE6B-591F1CA9A53A"); 

        public override GH_Exposure Exposure { get; } = GH_Exposure.quarternary;

        public override bool Obsolete { get; } = true;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public ToJson() : base("To Json", "ToJson", "Convert the object to a Json string", "Alligator", " Engine") { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new IObjectParameter(), "object", "object", "object to convert", GH_ParamAccess.item);
        }

        /*******************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Json", "Json", "Json string", GH_ParamAccess.item);
        }

        /*******************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BH.Engine.Reflection.Compute.ClearCurrentEvents();

            IObject obj = null;
            DA.GetData(0, ref obj);

            DA.SetData(0, BH.Engine.Serialiser.Convert.ToJson(obj));

            Logging.ShowEvents(this, BH.Engine.Reflection.Query.CurrentEvents());
        }

        /*******************************************/
    }
}
