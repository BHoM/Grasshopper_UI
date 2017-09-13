//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using BH.oM.Structural;
//using BH.UI.Alligator.Components;
//using Grasshopper.Kernel;
//using GHE = BH.Engine.Grasshopper;
//using BHG = BH.oM.Geometry;
//using BHE = BH.oM.Structural.Elements;
//using BHI = BH.oM.Structural.Interface;
//using Rhino.Geometry;
//using Grasshopper;
//using BH.Engine.Grasshopper.Components;
//using Grasshopper.Kernel.Data;

//namespace BH.UI.Alligator.Structural.Elements
//{
//    public class CreatePanel : BHoMBaseComponent<BHE.Panel>   //TODO: Need to align with BHoM 2.0
//    {
//        public CreatePanel() : base("Create Panel", "CreatePanel", "Create a BH Panel object", "Structure", "Elements") { }

//        public override Guid ComponentGuid
//        {
//            get
//            {
//                return new Guid("9E64C671-01BD-4D39-94D3-554BD2F8BA52");
//            }
//        }

//        /// <summary> Icon (24x24 pixels)</summary>
//        protected override System.Drawing.Bitmap Internal_Icon_24x24
//        {
//            get { return Alligator.Structural.Properties.Resources.BHoM_Panel; }
//        }
//    }

//    public class SplitOpenings : GH_Component
//    {
//        public SplitOpenings() : base("SplitOpenings", "SplitOpenings", "Remove openings from the panel", "Alligator", "Structural") { }

//        public override GH_Exposure Exposure
//        {
//            get
//            {
//                return GH_Exposure.secondary;
//            }
//        }

//        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
//        {
//            pManager.AddGenericParameter("Panel", "Panel", "BHoM Panel", GH_ParamAccess.item);
//        }

//        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
//        {
//            pManager.AddGenericParameter("Panel", "Panel", "BHoM Panel", GH_ParamAccess.item);
//            pManager.AddGenericParameter("Openings", "Openings", "BHoM Openings", GH_ParamAccess.list);
//        }

//        protected override void SolveInstance(IGH_DataAccess DA)
//        {
//            // Getting the inputs from GH
//            BHE.Panel panel = GHE.DataUtils.GetGenericData<BHE.Panel>(DA, 0);

//            // Creating the panel without openings
//            BHE.Panel newPanel = panel.GetShallowClone() as BHE.Panel;
//            BHG.Group<BHG.Curve> contour = new BHG.Group<BHG.Curve>();
//            foreach (BHG.Curve curve in panel.External_Contours)
//                contour.Add(curve);
//            newPanel.External_Contours = contour;

//            // Getting the openings
//            DA.SetData(0, newPanel);
//            DA.SetDataList(1, panel.Internal_Contours);
//        }

//        public override Guid ComponentGuid
//        {
//            get { return new Guid("3C7E971B-E08B-4605-A526-E6CB3248FFBC"); }
//        }


//    }

//    public class ExportPanel : ExportComponent<BHE.Panel>
//    {
//        public ExportPanel() : base("Export Panel", "SetPanel", "Creates or Replaces the geometry of a Panel", "Structure", "Elements") { }

//        public override GH_Exposure Exposure
//        {
//            get
//            {
//                return GH_Exposure.secondary;
//            }
//        }

//        protected override List<BHE.Panel> SetObjects(BHI.IElementAdapter app, List<BHE.Panel> objects, out List<string> ids)
//        {
//            app.SetPanels(objects, out ids);
//            return objects;
//        }

//        public override Guid ComponentGuid
//        {
//            get { return new Guid("72fb2007-5da5-4037-b481-6e134df8c583"); }
//        }
//        /// <summary> Icon (24x24 pixels)</summary>     
//        protected override System.Drawing.Bitmap Internal_Icon_24x24
//        {
//            get { return Alligator.Structural.Properties.Resources.BHoM_Panel_Export; }
//        }
//    }

//    public class ImportPanel : ImportComponent<BHE.Panel>
//    {
//        public ImportPanel() : base("Import Panel", "PanelNode", "Get the geometry and properties of a panel", "Structure", "Elements")
//        {

//        }

//        public override GH_Exposure Exposure
//        {
//            get
//            {
//                return GH_Exposure.tertiary;
//            }
//        }

//        //protected override void SolveInstance(IGH_DataAccess DA)
//        //{
//        //    if (GHE.DataUtils.Run(DA, 2))
//        //    {
//        //        BHI.IElementAdapter app = GHE.DataUtils.GetGenericData<BHI.IElementAdapter>(DA, 0);
//        //        if (app != null)
//        //        {
//        //            List<string> ids = null;
//        //            List<BHE.Panel> panels = null;
//        //            DataTree<GeometryBase> geometry = new DataTree<GeometryBase>();
//        //            if (m_Selection == BHI.ObjectSelection.FromInput)
//        //                ids = GHE.DataUtils.GetDataList<string>(DA, 1);

//        //            app.Selection = m_Selection;
//        //            ids = app.GetPanels(out panels, ids);

//        //            for (int i = 0; i < panels.Count; i++)
//        //            {
//        //                geometry.AddRange(GHE.GeometryUtils.ConvertGroup<BHG.Curve>(panels[i].External_Contours), new GH.Kernel.Data.GH_Path(i));
//        //                geometry.AddRange(GHE.GeometryUtils.ConvertGroup<BHG.Curve>(panels[i].Internal_Contours), new GH.Kernel.Data.GH_Path(i));
//        //            }

//        //            DA.SetDataList(0, ids);
//        //            DA.SetDataList(1, panels);
//        //            DA.SetDataTree(2, geometry);
//        //        }
//        //    }
//        //}

//        public override List<BHE.Panel> GetObjects(BHI.IElementAdapter app, List<string> objectIds, out IGH_DataTree geom, out List<string> outIds)
//        {
//            List<BHE.Panel> panels = null;
//            DataTree<GeometryBase> geometry = new DataTree<GeometryBase>();
           
//            app.Selection = m_Selection;
//            outIds = app.GetPanels(out panels, objectIds);

//            for (int i = 0; i < panels.Count; i++)
//            {
//                geometry.AddRange(GHE.GeometryUtils.ConvertGroup<BHG.Curve>(panels[i].External_Contours), new GH.Kernel.Data.GH_Path(i));
//                geometry.AddRange(GHE.GeometryUtils.ConvertGroup<BHG.Curve>(panels[i].Internal_Contours), new GH.Kernel.Data.GH_Path(i));
//            }
//            geom = geometry;
//            return panels;
//        }

//        public override Guid ComponentGuid
//        {
//            get { return new Guid("5520dc1b-87b3-491a-93fa-1495315ce5a2"); }
//        }

//        /// <summary> Icon (24x24 pixels)</summary>
//        protected override System.Drawing.Bitmap Internal_Icon_24x24
//        {
//            get { return Alligator.Structural.Properties.Resources.BHoM_Panel_Import; }
//        }
//    }
//}
