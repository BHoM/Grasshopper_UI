using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using MA = BH.Adapter.Mongo;
using BH.Adapter.Queries;
using BH.UI.Alligator.Base;
using BH.UI.Alligator;

namespace BH.UI.Alligator.Mongo
{
    public class FromMongo : GH_Component
    {
        public FromMongo() : base("FromMongo", "FromMongo", "Get BHoM objects from a Mongo database", "Alligator", "Mongo") { }
        public override Guid ComponentGuid { get { return new Guid("AE8F5C54-8746-48BF-A01D-7B4D28A2D91A"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return Properties.Resources.BHoM_Mongo_From; } }
        public override GH_Exposure Exposure { get { return GH_Exposure.secondary; } }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Mongo link", "link", "collection to get the data from", GH_ParamAccess.item);
            pManager.AddTextParameter("query", "query", "query string", GH_ParamAccess.list);
            pManager.AddBooleanParameter("toJson", "toJson", "output as json instead of objects", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("active", "active", "check if the component currently allows data transfer", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("result", "result", "result from the query", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            MA.MongoAdapter link = new MA.MongoAdapter();
            List<string> query = new List<string>();
            bool toJson = true, active = false;

            link = DA.BH_GetData(0, link);
            query = DA.BH_GetDataList(1, query);
            toJson = DA.BH_GetData(2, toJson);
            active = DA.BH_GetData(3, active);

            Dictionary<string, string> config = new Dictionary<string, string>
            {
                { "keepAsString", toJson.ToString() }
            };

            if (active)
                m_LastResult = link.Pull(new BatchQuery(query.Select(x => new CustomQuery(x))), config) as List<object>;

            DA.BH_SetDataList(0, m_LastResult);
        }

        private List<object> m_LastResult = new List<object>();
    }
}
