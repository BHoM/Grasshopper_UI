using Grasshopper.Kernel;
using System;
using BH.UI.Alligator.Base;
using BH.oM.Base;
using BH.oM.DataManipulation.Queries;
using BH.Adapter;

namespace BH.UI.Alligator.Adapter
{
    public class Delete : GH_Component
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Base.Properties.Resources.Delete; 

        public override Guid ComponentGuid { get; } = new Guid("8E2635F4-0C33-4608-910E-CDD676C03519"); 

        public override GH_Exposure Exposure { get; } = GH_Exposure.primary; 

        public override bool Obsolete { get; } = true;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public Delete() : base("Delete", "Delete", "Delete objects in the external software", "Alligator", " Adapter") { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Adapter", "Adapter", "Adapter to the external software", GH_ParamAccess.item);
            pManager.AddGenericParameter("Filter", "Filter", "Filter Query", GH_ParamAccess.item);
            pManager.AddParameter(new BHoMObjectParameter(), "Config", "Config", "Delete config", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Active", "Active", "Execute the delete", GH_ParamAccess.item, false);
            pManager[1].Optional = true;
            pManager[2].Optional = true;
        }

        /*******************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("#deleted", "#deleted", "Number of objects deleted", GH_ParamAccess.item);
        }

        /*******************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BH.Engine.Reflection.Compute.ClearCurrentEvents();

            BHoMAdapter adapter = null; DA.GetData(0, ref adapter);
            FilterQuery query = null; DA.GetData(1, ref query);
            CustomObject config = new CustomObject(); DA.GetData(2, ref config);
            bool active = false; DA.GetData(3, ref active);

            if (!active) return;

            if (query == null)
                query = new FilterQuery();

            int nb = adapter.Delete(query, config.CustomData);
            DA.SetData(0, nb);

            Logging.ShowEvents(this, BH.Engine.Reflection.Query.CurrentEvents());
        }

        /*******************************************/
    }
}
