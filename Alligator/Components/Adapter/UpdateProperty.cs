using Grasshopper.Kernel;
using System;
using BH.UI.Alligator.Base;
using BH.oM.Base;
using BH.oM.DataManipulation.Queries;
using BH.Adapter;

namespace BH.UI.Alligator.Adapter
{
    public class UpdateProperty : GH_Component
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Base.Properties.Resources.UpdateProperty; 

        public override Guid ComponentGuid { get; } = new Guid("E050834D-F825-4299-BEA9-A3E067691925"); 

        public override GH_Exposure Exposure { get; } = GH_Exposure.primary; 


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public UpdateProperty() : base("UpdateProperty", "UpdateProperty", "Update a property of objects from the external software", "Alligator", " Adapter") { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Adapter", "Adapter", "Adapter to the external software", GH_ParamAccess.item);
            pManager.AddGenericParameter("Filter", "Filter", "Filer Query", GH_ParamAccess.item);
            pManager.AddTextParameter("Property", "Property", "Name of the property to change", GH_ParamAccess.item);
            pManager.AddGenericParameter("NewValue", "NewValue", "New value to assign to the property", GH_ParamAccess.item);
            pManager.AddParameter(new BHoMObjectParameter(), "Config", "Config", "Delete config", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Active", "Active", "Execute the pull", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager[4].Optional = true;
        }

        /*******************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("#updated", "#updated", "Number of objects updated", GH_ParamAccess.item);
        }

        /*******************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHoMAdapter adapter = null; DA.GetData(0, ref adapter);
            FilterQuery query = null; DA.GetData(1, ref query);
            string property = ""; DA.GetData(2, ref property);
            object value = null; DA.GetData(3, ref value);
            CustomObject config = new CustomObject(); DA.GetData(4, ref config);
            bool active = false; DA.GetData(5, ref active);

            if (!active) return;

            if (query == null)
                query = new FilterQuery();

            int nb = adapter.UpdateProperty(query, property, value, config.CustomData);
            DA.SetData(0, nb);
        }

        /*******************************************/
    }
}
