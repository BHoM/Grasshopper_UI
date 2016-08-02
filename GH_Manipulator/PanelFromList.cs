using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alligator.GH_Manipulator
{
    public class PanelFromList : GH_Component
    {
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("D768C0FC-0B59-4DC3-9E03-CC3D06608B37");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("data list", "data", "list of strings used to create the panels", GH_ParamAccess.list);
            pManager.AddTextParameter("group name", "groupName", "name to give to the group containing the created panels", GH_ParamAccess.item);
            pManager.AddBooleanParameter("trigger", "trigger", "triggers the panel creation", GH_ParamAccess.item);
            pManager.AddGenericParameter("template", "template", "panel used as template for replication and position", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            throw new NotImplementedException();
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<string> data = new List<string>();
            string groupName = "";
            bool trigger = false;
            object template = null;

            DA.GetDataList<string>(0, data);
            DA.GetData<string>(1, ref groupName);
            DA.GetData<bool>(2, ref trigger);
            DA.GetData<object>(3, ref template);

            // TODO - stat porting the code from GH C#
        }

    }
}
