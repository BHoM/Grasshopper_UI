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
    public class Pull : GH_Component
    {
        public Pull() : base("Pull", "Pull", "Pull objects from the external software", "Alligator", "Adapter") { }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }
        public override Guid ComponentGuid { get { return new Guid("BA3D716D-3044-4795-AC81-0FECC80781E3"); } }

        public override GH_Exposure Exposure { get { return GH_Exposure.primary; } }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Adapter", "Adapter", "Adapter to the external software", GH_ParamAccess.item);
            pManager.AddGenericParameter("Query", "Query", "BHoM Query", GH_ParamAccess.item);
            pManager.AddGenericParameter("Config", "Config", "Delete config", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Active", "Active", "Execute the pull", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "Objects", "Objects", "Objects obtained from the query", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHoMAdapter adapter = null; DA.BH_GetData(0, adapter);
            BH.Adapter.Queries.IQuery query = null; DA.BH_GetData(1, query);
            Dictionary<string, string> config = null; DA.BH_GetData(2, config);
            bool active = false; DA.BH_GetData(3, active);

            if (!active) return;

            IEnumerable<object> objects = adapter.Pull(query, config);
            DA.BH_SetData(0, objects);
        }
    }
}
