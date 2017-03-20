using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using GHE = Grasshopper_Engine;
using MA = Mongo_Adapter;

namespace Mongo_Alligator
{
    public class MoveCollection : GH_Component
    {
        public MoveCollection() : base("MoveCollection", "MoveCollection", "Moves all the content from one collection to another. Overwrites the target collection", "Alligator", "Mongo") { }
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("929F2358-8CCA-4368-8E7D-97EAD50BB730");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Source", "Source", "Source mongo link collection to move data FROM", GH_ParamAccess.item);
            pManager.AddGenericParameter("Target", "Target", "Target mongo link collection to move data TO", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Overwrite", "Overwrite", "If set to true the content of the target collection will be overwritten if the target is non-empty", GH_ParamAccess.item);
            pManager.AddBooleanParameter("active", "active", "check if the component currently allows data transfer", GH_ParamAccess.item, false);

        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Success", "Success", "Success", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            MA.MongoLink source = GHE.DataUtils.GetData<MA.MongoLink>(DA, 0);
            MA.MongoLink target = GHE.DataUtils.GetData<MA.MongoLink>(DA, 1);
            bool overwrite = GHE.DataUtils.GetData<bool>(DA, 2);

            bool success = false;
            if (GHE.DataUtils.Run(DA, 3))
            {
                success = source.MoveCollection(target, overwrite);
            }

            DA.SetData(0, success);
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }
    }
}
