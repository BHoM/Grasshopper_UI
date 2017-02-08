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

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Mongo_Alligator.Properties.Resources.BHoM_Mongo_DeleteObjects; }
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
            pManager.AddGenericParameter("Mongo link", "link", "collection to get the data from", GH_ParamAccess.item);
            pManager.AddTextParameter("filter", "filter", "filter string", GH_ParamAccess.item, "{}");
            pManager.AddBooleanParameter("active", "active", "check if the compoenent currently allows data transfer", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Done", "Done", "return true when the task is finished", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            MA.MongoLink link = GHE.DataUtils.GetGenericData<MA.MongoLink>(DA, 0);
            string filter = GHE.DataUtils.GetData<string>(DA, 1);
            bool active = false; DA.GetData<bool>(2, ref active);

            if (active)
            {
                bool done = link.Delete(filter);
                DA.SetData(0, done);
            } 
            else
                DA.SetData(0, false);
        }
    }
}
