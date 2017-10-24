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

namespace BH.UI.Alligator.Adapter
{
    public class BatchQuery : GH_Component   
    {
        public BatchQuery() : base("BatchQuery", "BatchQuery", "Create a batch query", "Alligator", "Adapter") { }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }
        public override Guid ComponentGuid { get { return new Guid("C22E7A82-4DB9-41C4-8A43-B786FF2CD5FE"); } }

        public override GH_Exposure Exposure { get { return GH_Exposure.secondary; } }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Queries", "Queries", "List of queries to process as a batch", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Query", "Query", "BatchQuery", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<IQuery> queries = new List<IQuery>();  DA.GetDataList(0, queries);

            BH.Adapter.Queries.BatchQuery query = new BH.Adapter.Queries.BatchQuery(queries);
            DA.SetData(0, query);
        }
    }
}
