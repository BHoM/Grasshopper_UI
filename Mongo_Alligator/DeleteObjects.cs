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
    public class DeleteObjects : GH_Component
    {
        public DeleteObjects() : base("DeleteObjects", "DeleteObjects", "Delete the objects that match the filter from the database", "Alligator", "Mongo") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("9EB7A714-2C00-46BC-A260-C9E393355D6E");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Mongo link", "link", "collection to get the data from", GH_ParamAccess.item);
            pManager.AddTextParameter("filter", "filter", "filter string", GH_ParamAccess.item, "{}");
            pManager.AddBooleanParameter("active", "active", "check if the compoenent currently allows data transfer", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            MA.MongoLink link = GHE.DataUtils.GetGenericData<MA.MongoLink>(DA, 0);
            string filter = GHE.DataUtils.GetData<string>(DA, 1);
            bool active = false; DA.GetData<bool>(2, ref active);

            if (active)
                link.Delete(filter);
        }
    }
}
