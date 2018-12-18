using System;
using Grasshopper.Kernel;

namespace BH.UI.Grasshopper.Base
{
    public class FromJson : GH_Component
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Properties.Resources.FromJson; 

        public override Guid ComponentGuid { get; } = new Guid("EB108FE0-A807-4CEA-A8EB-2B8D54ADBC04"); 

        public override GH_Exposure Exposure { get; } = GH_Exposure.hidden;

        public override bool Obsolete { get; } = true;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public FromJson() : base("From Json", "FromJson", "Try to convert a Json string to a BHoM object", "Grasshopper", " Engine") { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Json", "Json", "Json string", GH_ParamAccess.item);
        }

        /*******************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new IObjectParameter(), "object", "object", "object recovered", GH_ParamAccess.item);
        }

        /*******************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BH.Engine.Reflection.Compute.ClearCurrentEvents();

            string json = "";
            DA.GetData(0, ref json);

            DA.SetData(0, BH.Engine.Serialiser.Convert.FromJson(json));

            Logging.ShowEvents(this, BH.Engine.Reflection.Query.CurrentEvents());
        }

        /*******************************************/
    }
}
