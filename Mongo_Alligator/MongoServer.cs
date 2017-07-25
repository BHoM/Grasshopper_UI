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
    public class MongoServer : GH_Component
    {
        public MongoServer() : base("MongoServer", "MongoServer", "Starts a Mongo server on you machine.", "Alligator", "Mongo") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("AFBA2519-A2B9-451E-BDE0-821AB7B2E301");
            }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Mongo_Alligator.Properties.Resources.BHoM_Mongo_CreateServer; }
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
            pManager.AddTextParameter("folder", "folder", "folder where the server will be stored", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("server", "server", "created server", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string folder = GHE.DataUtils.GetData<string>(DA, 0);

            MA.MongoServer server = new MA.MongoServer(folder);
            server.Killed += ServerKilled;

            DA.SetData(0, new MA.MongoServer(folder));
        }

        private void ServerKilled()
        {
            throw new Exception("The server is down");
        }
    }
}
