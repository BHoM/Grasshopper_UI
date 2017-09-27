using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using MA = BH.Adapter.Mongo;
using BH.UI.Alligator.Base;
using BH.Adapter.Queries;
using System.Linq;

namespace BH.UI.Alligator.Mongo
{
    public class MoveData : GH_Component
    {
        public MoveData() : base("MoveObject", "ToCollection", "Move objects from a collection to another", "Alligator", "Mongo") { }
        protected override System.Drawing.Bitmap Icon { get { return null; } }
        public override Guid ComponentGuid { get { return new Guid("bfded40a-d367-46e7-9769-11c054c9112b"); } }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Source", "SourceLink", "Source mongo link collection to move data FROM", GH_ParamAccess.item);
            pManager.AddGenericParameter("Target", "TargetLink", "Target mongo link collection to move data TO", GH_ParamAccess.item);
            pManager.AddTextParameter("Queries", "Queries", "Queries to filter moved objects", GH_ParamAccess.list);
            pManager.AddBooleanParameter("active", "active", "check if the component currently allows data transfer", GH_ParamAccess.item, false);
        }
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Done", "Done", "Done", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            MA.MongoAdapter source = new BH.Adapter.Mongo.MongoAdapter();
            MA.MongoAdapter target = new BH.Adapter.Mongo.MongoAdapter();
            List<string> queries = new List<string>();
            bool active = false;
            source = DA.BH_GetData(0, source);
            target = DA.BH_GetData(1, target);
            queries = DA.BH_GetDataList(2, queries);
            active = DA.BH_GetData(3, active);

            Dictionary<string, string> config = new Dictionary<string, string>();
            if (!active) { return; }
            DA.SetData(0, source.MoveToCollection(queries.Select(x => new CustomQuery(x)), target, config));

        }
    }
}