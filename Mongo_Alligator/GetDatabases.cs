using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using GHE = Grasshopper_Engine;
using MA = BH.Adapter.Mongo;

namespace Alligator.Mongo
{
    public class GetDatabases : GH_Component
    {
        public GetDatabases() : base("GetDatabases", "GetDatabases", "Get the list of databases curretly available in the Mongo server.", "Alligator", "Mongo") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("551E958A-0EBC-4833-ABED-47ED8E9B7A1B");
            }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Mongo_Alligator.Properties.Resources.BHoM_Mongo_GetDatabases; }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.tertiary;
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("server", "server", "Mongo server", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("databases", "databases", "list of databases names", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            MA.MongoServer server = GHE.DataUtils.GetGenericData<MA.MongoServer>(DA, 0);

            DA.SetDataList(0, server.GetAllDatabases());
        }
    }
}
