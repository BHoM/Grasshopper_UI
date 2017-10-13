using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using BH.UI.Alligator.Base;
using BH.oM.Base;
using BH.UI.Alligator.Query;
using BH.Adapter.Queries;
using BH.Adapter;

namespace BH.UI.Alligator.Adapter
{
    public class Delete : GH_Component
    {
        public Delete() : base("Delete", "Delete", "Delete objects in the external software", "Alligator", "Adapter") { }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }
        public override Guid ComponentGuid { get { return new Guid("8E2635F4-0C33-4608-910E-CDD676C03519"); } }

        public override GH_Exposure Exposure { get { return GH_Exposure.primary; } }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Adapter", "Adapter", "Adapter to the external software", GH_ParamAccess.item);
            pManager.AddGenericParameter("Filter", "Filter", "Filter Query", GH_ParamAccess.item);
            pManager.AddGenericParameter("Config", "Config", "Delete config", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Active", "Active", "Execute the delete", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("#deleted", "#deleted", "Number of objects deleted", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHoMAdapter adapter = null; DA.BH_GetData(0, adapter);
            BH.Adapter.Queries.FilterQuery query = null; DA.BH_GetData(1, query);
            Dictionary<string, string> config = null; DA.BH_GetData(2, config);
            bool active = false; DA.BH_GetData(3, active);

            if (!active) return;

            int nb = adapter.Delete(query, config);
            DA.BH_SetData(0, nb);
        }
    }
}
