using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using MA = Mongo_Adapter;
using GHE = Grasshopper_Engine;

namespace Alligator.Mongo
{
    public class FromMongo : GH_Component
    {
        public FromMongo() : base("FromMongo", "FromMongo", "Get BHoM objects from a Mongo database", "Alligator", "Mongo") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("AE8F5C54-8746-48BF-A01D-7B4D28A2D91A");
            }
        }

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
            MA.MongoLink link = GHE.DataUtils.GetGenericData<MA.MongoLink>(DA, 0);
            List<string> query = GHE.DataUtils.GetDataList<string>(DA, 1);
            bool toJson = GHE.DataUtils.GetData<bool>(DA, 2);
            bool active = false; DA.GetData<bool>(3, ref active);

            if (active)
                m_LastResult = link.Query(query, toJson);

            DA.SetDataList(0, m_LastResult);
        }


        private List<object> m_LastResult = new List<object>();
    }
}
