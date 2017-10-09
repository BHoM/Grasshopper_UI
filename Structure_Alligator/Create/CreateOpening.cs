//using BH.UI.Alligator.Components;
//using BH.oM.Structural.Elements;
//using BH.oM.Structural.Interface;
//using GH = Grasshopper;
//using Grasshopper.Kernel;
//using BH.Engine.Grasshopper;
//using Rhino.Geometry;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using BHG = BH.oM.Geometry;
//using BH.Engine.Grasshopper.Components;
//using Grasshopper.Kernel.Data;

//namespace BH.UI.Alligator.Structural.Elements
//{
//    public class CreateOpening : BHoMBaseComponent<Opening>
//    {
//        public CreateOpening() : base("Create Opening", "CreateOpening", "Create a BH Opening object", "Structure", "Elements") { }

//        public override Guid ComponentGuid
//        {
//            get
//            {
//                return new Guid("{BBC9B157-0C56-46CC-B7CA-7A82EB0AF823}");
//            }
//        }
//        protected override System.Drawing.Bitmap Internal_Icon_24x24
//        {
//            get { return Alligator.Structural.Properties.Resources.BHoM_Opening; }
//        }

//    }

//    public class ExportOpening : ExportComponent<Opening>
//    {
//        public ExportOpening() : base("Export Opening", "SetOpening", "Creates or Replaces the geometry of a Opening", "Structure", "Elements") { }

//        public override GH_Exposure Exposure
//        {
//            get
//            {
//                return GH_Exposure.secondary;
//            }
//        }
     
//        public override Guid ComponentGuid
//        {
//            get { return new Guid("{9AC9B157-0C56-46CC-B7CA-7A82EB0AF823}"); }
//        }
//        /// <summary> Icon (24x24 pixels)</summary>     
//        protected override System.Drawing.Bitmap Internal_Icon_24x24
//        {
//            get { return Alligator.Structural.Properties.Resources.BHoM_Opening_Export; }
//        }

//        protected override List<Opening> SetObjects(IElementAdapter app, List<Opening> objects, out List<string> ids)
//        {
//            app.SetOpenings(objects, out ids);
//            return objects;
//        }
//    }

//    public class ImportOpening : ImportComponent<Opening>
//    {
//        public ImportOpening() : base("Import Opening", "GetOpening", "Get the geometry and properties of a Opening", "Structure", "Elements")
//        {

//        }

//        public override GH_Exposure Exposure
//        {
//            get
//            {
//                return GH_Exposure.tertiary;
//            }
//        }

//        public override List<Opening> GetObjects(IElementAdapter app, List<string> objectIds, out IGH_DataTree geom, out List<string> outIds)
//        {
//            List<Opening> Openings = null;
//            GH.DataTree<GeometryBase> geometry = new GH.DataTree<GeometryBase>();

//            app.Selection = m_Selection;
//            outIds = app.GetOpenings(out Openings, objectIds);

//            for (int i = 0; i < Openings.Count; i++)
//            {
//                geometry.AddRange(Openings[i].Edges.Select(x => GeometryUtils.Convert(x)), new GH.Kernel.Data.GH_Path(i));
//            }
//            geom = geometry;
//            return Openings;
//        }

//        public override Guid ComponentGuid
//        {
//            get { return new Guid("{9AC9B157-0C56-46BB-B7CA-7A82EB0AF823}"); }
//        }
//        /// <summary> Icon (24x24 pixels)</summary>     
//        protected override System.Drawing.Bitmap Internal_Icon_24x24
//        {
//            get { return Alligator.Structural.Properties.Resources.BHoM_Opening_Import; }
//        }
//    }
//}
