using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BHB = BHoM.Base;
using MA = Mongo_Adapter;
using GHE = Grasshopper_Engine;

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
            pManager.AddTextParameter("key", "key", "key used to tag the saved data", GH_ParamAccess.item);
            pManager.AddBooleanParameter("active", "active", "check if the compoenent currently allows data transfer", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            MA.MongoLink link = GHE.DataUtils.GetGenericData<MA.MongoLink>(DA, 0);
            List<BHB.BHoMObject> bhomObjects = GHE.DataUtils.GetGenericDataList<BHB.BHoMObject>(DA, 1);

            string key = ""; DA.GetData<string>(2, ref key);
            bool active = false; DA.GetData<bool>(3, ref active);

            if (!active) return;
            link.SaveObjects(bhomObjects, key);
        }
    }
}
