using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using BH.UI.Alligator.Base;
using BH.oM.Base;
using BH.Adapter;

namespace BH.UI.Alligator.Adapter
{
    public class Execute : GH_Component
    {
        public Execute() : base("Execute", "Execute", "Execute command in the external software", "Alligator", "Adapter") { }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return Properties.Resources.Execute; } }
        public override Guid ComponentGuid { get { return new Guid("1C89AF97-379F-4432-B243-9C699EB454C2"); } }

        public override GH_Exposure Exposure { get { return GH_Exposure.primary; } }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Adapter", "Adapter", "Adapter to the external software", GH_ParamAccess.item);
            pManager.AddTextParameter("Command", "Command", "Command to run", GH_ParamAccess.item);
            pManager.AddGenericParameter("Params", "Params", "Parameters of the command", GH_ParamAccess.item);
            pManager.AddParameter(new BHoMObjectParameter(), "Config", "Config", "Delete config", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Active", "Active", "Execute the command", GH_ParamAccess.item);
            pManager[3].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Success", "Success", "Success", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHoMAdapter adapter = null; DA.GetData(0, ref adapter);
            string command = ""; DA.GetData(1, ref command);
            Dictionary<string, object> parameters = null; DA.GetData(2, ref parameters);
            CustomObject config = new CustomObject(); DA.GetData(3, ref config);
            bool active = false; DA.GetData(4, ref active);

            if (!active) return;

            bool success = adapter.Execute(command, parameters, config.CustomData);
            DA.SetData(0, success);
        }
    }
}
