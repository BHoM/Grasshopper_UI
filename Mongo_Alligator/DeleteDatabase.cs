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
    public class DeleteDatabase : GH_Component
    {
        public DeleteDatabase() : base("DeleteDatabase", "DeleteDatabase", "Delete teh specific database from the Mongo server.", "Alligator", "Mongo") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("1F07F868-42C1-4531-8902-AD446038752C");
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
            pManager.AddGenericParameter("server", "server", "Mongo server", GH_ParamAccess.item);
            pManager.AddTextParameter("database", "database", "database name", GH_ParamAccess.item);
            pManager.AddBooleanParameter("activate", "activate", "trigger the command", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("success", "success", "success", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            MA.MongoServer server = GHE.DataUtils.GetGenericData<MA.MongoServer>(DA, 0);
            string database = GHE.DataUtils.GetData<string>(DA, 1);
            bool activate = GHE.DataUtils.GetData<bool>(DA, 2);

            if (!activate)
            {
                DA.SetData(0, false);
            }
            else
            {
                DA.SetData(0, server.DeleteDatabase(database));
            } 

            
        }
    }
}
