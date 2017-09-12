using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using GHE = Grasshopper_Engine;
using BHE = BH.oM.Structural.Elements;
using BHI = BH.oM.Structural.Interface;


namespace Alligator.Structural.Elements
{
    public class ExportRigidLink : GHE.Components.ExportComponent<BHE.RigidLink>
    {
        public ExportRigidLink() : base("Export Rigid Link", "SetLink", "Creates or Replaces a rigid link", "Structure", "Elements") { }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BH.oM_RigidLink_Export; }
        }


        public override Guid ComponentGuid
        {
            get { return new Guid("E0E6A15B-7EA8-46C1-B9BF-5D48FAC19B1B"); }
        }

        protected override List<BHE.RigidLink> SetObjects(BHI.IElementAdapter app, List<BHE.RigidLink> objects, out List<string> ids)
        {
            app.SetRigidLinks(objects, out ids);
            return objects;
        }
    }

    //public class ExportRigidLink : GH_Component
    //{
    //    public ExportRigidLink() : base("Export Rigid Link", "SetLink", "Creates or Replaces a rigid link", "Structure", "Elements") { }

    //    /// <summary> Icon (24x24 pixels)</summary>
    //    protected override System.Drawing.Bitmap Internal_Icon_24x24
    //    {
    //        get { return Alligator.Properties.Resources.BH.oM_RigidLink_Export; }
    //    }

    //    public override GH_Exposure Exposure
    //    {
    //        get
    //        {
    //            return GH_Exposure.secondary;
    //        }
    //    }

    //    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    //    {
    //        pManager.AddGenericParameter("Application", "A", "Application to export bars to", GH_ParamAccess.item);
    //        pManager.AddGenericParameter("Rigid Links", "RL", "BH.oM bars to export", GH_ParamAccess.list);
    //        pManager.AddBooleanParameter("Activate", "Go", "Generate Bars", GH_ParamAccess.item);

    //        pManager[2].Optional = true;
    //    }

    //    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    //    {
    //        pManager.AddTextParameter("Ids", "Ids", "Rigid Link Numbers", GH_ParamAccess.list);
    //    }

    //    protected override void SolveInstance(IGH_DataAccess DA)
    //    {
    //        if (GHE.DataUtils.Run(DA, 2))
    //        {
    //            BHI.IElementAdapter app = GHE.DataUtils.GetGenericData<BHI.IElementAdapter>(DA, 0);
    //            if (app != null)
    //            {
    //                List<BHE.RigidLink> links = GHE.DataUtils.GetGenericDataList<BHE.RigidLink>(DA, 1);
    //                List<string> ids = null;
    //                app.SetRigidLinks(links, out ids);

    //                DA.SetDataList(0, ids);
    //            }
    //        }
    //    }

    //    public override Guid ComponentGuid
    //    {
    //        get { return new Guid("E0E6A15B-7EA8-46C1-B9BF-5D48FAC19B1B"); }
    //    }
    //}
}
