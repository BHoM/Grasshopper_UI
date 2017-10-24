using System;
using System.Collections.Generic;
using BHG = BH.oM.Geometry;
using BH.oM.Acoustic;
using Grasshopper.Kernel;
using BH.UI.Alligator.Base;

namespace BH.UI.Alligator.Acoustic
{
    public class CreateReceiver : GH_Component
    {
        public CreateReceiver() : base("CreateReceiver", "Rec", "Creates BHoM Acoustic receiver", "Alligator", "Acoustics") { }
        public override Guid ComponentGuid { get { return new Guid("{8e6a26c4-05f9-46db-8fd0-7328059a3003}"); } }
        protected override System.Drawing.Bitmap Icon { get { return null; } }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new BHoMGeometryParameter(), "Position", "P", "Receiver central point", GH_ParamAccess.item);
            pManager.AddTextParameter("Category", "T", "Category type of Receiver for directivity calculation", GH_ParamAccess.item);
            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "Receiver", "Rec", "BHoM Acoustic Receiver", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Receiver> receivers = new List<Receiver>();
            BHG.Point pos = new BHG.Point();
            string cat = "Omni";
            DA.GetData(0, ref pos);
            DA.GetData(1, ref cat);

            Receiver receiver = new Receiver(pos, cat);

            DA.SetDataList(0, receivers);
        }
    }
}
