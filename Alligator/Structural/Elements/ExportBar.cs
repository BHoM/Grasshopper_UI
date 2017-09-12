using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using System.Linq;
using GHE = Grasshopper_Engine;
using BHE = BH.oM.Structural.Elements;
using BHI = BH.oM.Structural.Interface;
using GH_IO.Serialization;

namespace Alligator.Structural.Elements
{
    public class ExportBar : GHE.Components.ExportComponent<BHE.Bar>
    {
        public ExportBar() : base("Export Bar", "SetBar", "Creates or Replaces the geometry of a Bar", "Structure", "Elements")
        { }

        protected override List<BHE.Bar> SetObjects(BHI.IElementAdapter app, List<BHE.Bar> objects, out List<string> ids)
        {
            app.SetBars(objects, out ids);

            return objects;
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("2420dc1b-87b3-491a-93fa-1495315ca5a2"); }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BH.oM_Bar_Export; }
        }

    }

    //public class ExportBar : GH_Component
    //{
    //    private List<string> m_ids;

    //    public ExportBar() : base("Export Bar", "SetBar", "Creates or Replaces the geometry of a Bar", "Structure", "Elements")
    //    {
    //        m_ids = null;
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
    //        pManager.AddGenericParameter("Application", "Application", "Application to export bars to", GH_ParamAccess.item);
    //        pManager.AddGenericParameter("Bars", "Bars", "BH.oM bars to export", GH_ParamAccess.list);
    //        pManager.AddBooleanParameter("Activate", "Activate", "Generate Bars", GH_ParamAccess.item);

    //        pManager[2].Optional = true;
    //    }

    //    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    //    {
    //        pManager.AddTextParameter("Ids", "Ids", "Bar Numbers", GH_ParamAccess.list);
    //    }

    //    protected override void SolveInstance(IGH_DataAccess DA)
    //    {
    //        if (GHE.DataUtils.Run(DA, 2))
    //        {
    //            BHI.IElementAdapter app = GHE.DataUtils.GetGenericData<BHI.IElementAdapter>(DA, 0);
    //            if (app != null)
    //            {
    //                //Shallow clone the bars to make sure no changes to them in the export affect upstream elements
    //                List<BHE.Bar> bars = GHE.DataUtils.GetGenericDataList<BHE.Bar>(DA, 1).Select(x => x.ShallowClone() as BHE.Bar).ToList();

    //                bars.ForEach(x => x.CustomData = new Dictionary<string, object>(x.CustomData));

    //                m_ids = null;
    //                app.SetBars(bars, out m_ids);


    //            }
    //        }

    //        DA.SetDataList(0, m_ids);
    //    }

    //    public override Guid ComponentGuid
    //    {
    //        get { return new Guid("2420dc1b-87b3-491a-93fa-1495315ca5a2"); }
    //    }

    //    /// <summary> Icon (24x24 pixels)</summary>
    //    protected override System.Drawing.Bitmap Internal_Icon_24x24
    //    {
    //        get { return Alligator.Properties.Resources.BH.oM_Bar_Export; }
    //    }
    //}
}
