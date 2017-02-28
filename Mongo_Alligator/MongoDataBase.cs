using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using GHE = Grasshopper_Engine;
using MA = Mongo_Adapter;

namespace Alligator.Mongo
{
    public class MongoDataBase : GH_Component
    {
        public MongoDataBase() : base("MongoDataBase", "MongoDB", "Create a mongo data base", "Alligator", "Mongo") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("D7BF33B6-0FCE-47B3-9B51-DE1FC5753774");
            }
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
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Data Base", "db", "mongo data base", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string server = GHE.DataUtils.GetData<string>(DA, 0);
            string database = GHE.DataUtils.GetData<string>(DA, 1);

            DA.SetData(0, new MA.MongoDataBase(server, database));
        }
    }
}