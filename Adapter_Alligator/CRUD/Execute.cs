using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using BH.UI.Alligator.Base;
using BH.oM.Base;
using BH.UI.Alligator;
using BH.Adapter.Queries;
using BH.Adapter;

namespace BH.UI.Alligator.Adapter
{
    public class Execute : GH_Component
    {
        public Execute() : base("Execute", "Execute", "Execute command in the external software", "Alligator", "Adapter") { }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }
        public override Guid ComponentGuid { get { return new Guid("1C89AF97-379F-4432-B243-9C699EB454C2"); } }

        public override GH_Exposure Exposure { get { return GH_Exposure.primary; } }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Adapter", "Adapter", "Adapter to the external software", GH_ParamAccess.item);
            pManager.AddTextParameter("Command", "Command", "Command to run", GH_ParamAccess.item);
            pManager.AddGenericParameter("Params", "Params", "Parameters of the command", GH_ParamAccess.item);
            pManager.AddGenericParameter("Config", "Config", "Delete config", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Active", "Active", "Execute the command", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Success", "Success", "Success", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHoMAdapter adapter = null; DA.BH_GetData(0, adapter);
            string command = ""; DA.BH_GetData(1, command);
            Dictionary<string, object> parameters = null; DA.BH_GetData(2, parameters);
            Dictionary<string, string> config = null; DA.BH_GetData(3, config);
            bool active = false; DA.BH_GetData(4, active);

            if (!active) return;

            bool success = adapter.Execute(command, parameters, config);
            DA.BH_SetData(0, success);
        }
    }
}
