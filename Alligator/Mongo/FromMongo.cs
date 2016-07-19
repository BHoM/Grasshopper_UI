using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;

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
            pManager.AddGenericParameter("Mongo link", "link", "collection to send the data to", GH_ParamAccess.item);
            pManager.AddTextParameter("filter", "filter", "filter string", GH_ParamAccess.item, "{}");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("BHoM objects", "objects", "BHoM objects to convert", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Databases_Engine.Mongo.MongoLink link = Utils.GetGenericData<Databases_Engine.Mongo.MongoLink>(DA, 0);
            string filter = Utils.GetData<string>(DA, 1);

            DA.SetDataList(0, link.GetObjects(filter));
        }
    }
}
