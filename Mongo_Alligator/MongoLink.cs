using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using GHE = BH.Engine.Grasshopper;
using MA = BH.Adapter.Mongo;

namespace Alligator.Mongo
{
    public class MongoAdapter : GH_Component
    {
        public MongoAdapter() : base("MongoAdapter", "MongoAdapter", "Create a link to a Mongo database", "Alligator", "Mongo") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("7E81BE63-D7FC-4EC9-A41D-CAFB7B1097A7");
            }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Mongo_Alligator.Properties.Resources.BHoM_Mongo_Link; }
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("server", "server", "address of the server", GH_ParamAccess.item, "mongodb://localhost:27017");
            pManager.AddTextParameter("database", "database", "name of the database", GH_ParamAccess.item, "project");
            pManager.AddTextParameter("collection", "collection", "name of the collection", GH_ParamAccess.item, "bhomObjects");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("link", "link", "link to the database colelction", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string server = GHE.DataUtils.GetData<string>(DA, 0);
            string database = GHE.DataUtils.GetData<string>(DA, 1);
            string collection = GHE.DataUtils.GetData<string>(DA, 2);

            DA.SetData(0, new MA.MongoAdapter(server, database, collection));
        }
    }
}
