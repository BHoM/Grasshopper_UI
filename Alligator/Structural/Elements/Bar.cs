using BHoM.Structural;
using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using GHE = Grasshopper_Engine;
using Alligator.Components;
using BHE = BHoM.Structural.Elements;
using BHI = BHoM.Structural.Interface;
using System.Windows.Forms;
using Rhino.Geometry;
using Grasshopper;
using Grasshopper_Engine.Components;

namespace Alligator.Structural.Elements
{
    public class CreateBar : BHoMBaseComponent<BHE.Bar>
    {
        public CreateBar() : base("Create Bar", "CreateBar", "Create a BH Bar object", "Structure", "Elements") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("5FE0E2C4-5E50-410F-BBC7-C255FD1BD2B4");
            }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_Bar; }
        }
    }

    public class ExportBar : GH_Component
    {
        public ExportBar() : base("Export Bar", "SetBar", "Creates or Replaces the geometry of a Bar", "Structure", "Elements") { }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "Application", "Application to export bars to", GH_ParamAccess.item);
            pManager.AddGenericParameter("Bars", "Bars", "BHoM bars to export", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Activate", "Activate", "Generate Bars", GH_ParamAccess.item);

            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Ids", "Ids", "Bar Numbers", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (GHE.DataUtils.Run(DA, 2))
            {
                BHI.IElementAdapter app = GHE.DataUtils.GetGenericData<BHI.IElementAdapter>(DA, 0);
                if (app != null)
                {
                    List<BHE.Bar> bars = GHE.DataUtils.GetGenericDataList<BHE.Bar>(DA, 1);
                    List<string> ids = null;
                    app.SetBars(bars, out ids);

                    DA.SetDataList(0, ids);
                }
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("2420dc1b-87b3-491a-93fa-1495315ca5a2"); }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_Bar_Export; }
        }
    }

    public class ImportBar : ImportComponent
    {       
        public ImportBar() : base("Import Bar", "GetBar", "Get the geometry and properties of a Bar", "Structure", "Elements")
        {
           
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.tertiary;
            }
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (GHE.DataUtils.Run(DA, 2))
            {
                BHI.IElementAdapter app = GHE.DataUtils.GetGenericData<BHI.IElementAdapter>(DA, 0);
                if (app != null)
                {
                    List<string> ids = null;
                    List<BHE.Bar> bars = null;
                    DataTree<Curve> curves = new DataTree<Curve>();
                    if (m_Selection == BHI.ObjectSelection.FromInput)
                        ids = GHE.DataUtils.GetDataList<string>(DA, 1);

                    app.Selection = m_Selection;
                    ids = app.GetBars(out bars, ids);

                    for (int i = 0; i < bars.Count;i++)
                    {
                        curves.Add(GHE.GeometryUtils.Convert(bars[i].Line));
                    }

                    DA.SetDataList(0, ids);
                    DA.SetDataList(1, bars);
                    DA.SetDataTree(2, curves);
                }
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("1320dc1b-87b3-491a-93fa-1495315aa5a2"); }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_Bar_Import; }
        }
    }
}
