using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;

namespace Alligator.Mongo
{
    public class ToMongo : GH_Component
    {
        public ToMongo() : base("ToMongo", "ToMongo", "Send BHoM objects to a Mongo database", "Alligator", "Mongo") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("30CA2D65-C265-43A5-A7FA-E183C2B916EB");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Mongo link", "link", "collection to send the data to", GH_ParamAccess.item);
            pManager.AddGenericParameter("BHoM objects", "objects", "BHoM objects to convert", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Databases_Engine.Mongo.MongoLink link = Utils.GetGenericData<Databases_Engine.Mongo.MongoLink>(DA, 0);
            List<BHoM.Global.BHoMObject> bhomObjects = Utils.GetGenericDataList<BHoM.Global.BHoMObject>(DA, 1);

            link.SaveObjects(bhomObjects);
        }
    }
}
