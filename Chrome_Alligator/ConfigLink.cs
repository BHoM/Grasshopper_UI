using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
//using GHE = Grasshopper_Engine;
using CA = Chrome_Adapter;

namespace Alligator.Mongo
{
    public class ConfigLink : GH_Component
    {
        public ConfigLink() : base("ConfigLink", "ConfigLink", "Define the configuration of a link between data and config.", "Alligator", "Chrome") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("925ED2D7-4B90-47E1-BEE0-53FA35C7F38E");
            }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.quinary;
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("set", "set", "dataset to link", GH_ParamAccess.item);
            pManager.AddTextParameter("dim", "dim", "Corresponding dimension to link for the dataset.", GH_ParamAccess.item);
            pManager.AddTextParameter("view", "view", "view with the config linked to the dataset.", GH_ParamAccess.item);
            pManager.AddTextParameter("config", "config", "Name of the config property to link.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("link", "link", "link", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string set = "", dim = "", view = "", config = "";
            DA.GetData<string>(0, ref set);
            DA.GetData<string>(1, ref dim);
            DA.GetData<string>(2, ref view);
            DA.GetData<string>(3, ref config);

            string link = "{\"set\": \"" + set + "\", \"dim\": \"" + dim + "\", \"view\": \"" + view + "\", \"config\": \"" + config + "\"}";

            DA.SetData(0, link);
        }
    }
}
