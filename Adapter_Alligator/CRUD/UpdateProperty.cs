using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using BH.UI.Alligator.Base;
using BH.oM.Base;
using BH.UI.Alligator;
using BH.Adapter.Queries;
using BH.Adapter;

namespace BH.UI.Alligator.Adapter
{
    public class UpdateProperty : GH_Component
    {
        public UpdateProperty() : base("UpdateProperty", "UpdateProperty", "Update a property of objects from the external software", "Alligator", "Adapter") { }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }
        public override Guid ComponentGuid { get { return new Guid("E050834D-F825-4299-BEA9-A3E067691925"); } }

        public override GH_Exposure Exposure { get { return GH_Exposure.primary; } }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Adapter", "Adapter", "Adapter to the external software", GH_ParamAccess.item);
            pManager.AddGenericParameter("Filter", "Filter", "Filer Query", GH_ParamAccess.item);
            pManager.AddTextParameter("Property", "Property", "Name of the property to change", GH_ParamAccess.item);
            pManager.AddGenericParameter("NewValue", "NewValue", "New value to assign to the property", GH_ParamAccess.item);
            pManager.AddGenericParameter("Config", "Config", "Delete config", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Active", "Active", "Execute the pull", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("#updated", "#updated", "Number of objects updated", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHoMAdapter adapter = null; DA.BH_GetData(0, ref adapter);
            BH.Adapter.Queries.FilterQuery query = null; DA.BH_GetData(1, ref query);
            string property = ""; DA.BH_GetData(2, ref property);
            object value = null; DA.BH_GetData(3, ref value);
            Dictionary<string, string> config = null; DA.BH_GetData(4, ref config);
            bool active = false; DA.BH_GetData(5, ref active);

            if (!active) return;

            int nb = adapter.UpdateProperty(query, property, value, config);
            DA.BH_SetData(0, nb);
        }
    }
}
