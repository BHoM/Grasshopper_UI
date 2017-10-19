using System;
using Grasshopper.Kernel;
using MA = BH.Adapter.Mongo;
using BH.UI.Alligator;

namespace BH.UI.Alligator.Mongo
{
    public class MongoServer : GH_Component
    {
        public MongoServer() : base("MongoServer", "MongoServer", "Starts a Mongo server on you machine.", "Alligator", "Mongo") { }

        public override Guid ComponentGuid { get { return new Guid("AFBA2519-A2B9-451E-BDE0-821AB7B2E301"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return Properties.Resources.BHoM_Mongo_CreateServer; } }
        public override GH_Exposure Exposure { get { return GH_Exposure.tertiary; } }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("folder", "folder", "folder where the server will be stored", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("server", "server", "created server", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string directory = Environment.CurrentDirectory;
            directory = DA.BH_GetData(0, ref directory);
            MA.MongoServer server = new MA.MongoServer(directory);
            server.Killed += ServerKilled;
            DA.SetData(0, new MA.MongoServer(directory));
        }

        private void ServerKilled()
        {
            throw new Exception("The server is down");
        }
    }
}
