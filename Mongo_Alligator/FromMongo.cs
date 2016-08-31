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
            pManager.AddTextParameter("filter", "filter", "filter string", GH_ParamAccess.item, "{}");
            pManager.AddBooleanParameter("toBHoM", "toBHoM", "convert to BHoM objects", GH_ParamAccess.item, true);
            pManager.AddBooleanParameter("active", "active", "check if the component currently allows data transfer", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("BHoM objects", "objects", "BHoM objects to convert", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            MA.MongoLink link = GHE.DataUtils.GetGenericData<MA.MongoLink>(DA, 0);
            string filter = GHE.DataUtils.GetData<string>(DA, 1);
            bool toBHoM = GHE.DataUtils.GetData<bool>(DA, 2);
            bool active = false; DA.GetData<bool>(3, ref active);

            if (!active) return;

            if (toBHoM)
                DA.SetDataList(0, link.GetObjects(filter));
            else
                DA.SetDataList(0, link.GetJson(filter));
        }
    }
}
