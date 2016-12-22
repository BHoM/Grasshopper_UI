using Alligator.Components;
using BHoM.Structural.Elements;
using BHoM.Structural.Interface;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper_Engine;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHG = BHoM.Geometry;
using Grasshopper_Engine.Components;
using Grasshopper.Kernel.Data;

namespace Alligator.Structural.Elements
{
    public class CreateOpening : BHoMBaseComponent<Opening>
    {
        public CreateOpening() : base("Create Opening", "CreateOpening", "Create a BH Opening object", "Structure", "Elements") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("{BBC9B157-0C56-46CC-B7CA-7A82EB0AF823}");
            }
        }
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_Opening; }
        }

    }

    public class ExportOpening : ExportComponent<Opening>
    {
        public ExportOpening() : base("Export Opening", "SetOpening", "Creates or Replaces the geometry of a Opening", "Structure", "Elements") { }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }
     
        public override Guid ComponentGuid
        {
            get { return new Guid("{9AC9B157-0C56-46CC-B7CA-7A82EB0AF823}"); }
        }
        /// <summary> Icon (24x24 pixels)</summary>     
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_Opening_Export; }
        }

        protected override List<Opening> SetObjects(IElementAdapter app, List<Opening> objects, out List<string> ids)
        {
            app.SetOpenings(objects, out ids);
            return objects;
        }
    }

    public class ImportOpening : ImportComponent<Opening>
    {
        public ImportOpening() : base("Import Opening", "GetOpening", "Get the geometry and properties of a Opening", "Structure", "Elements")
        {

        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.tertiary;
            }
        }

        public override List<Opening> GetObjects(IElementAdapter app, List<string> objectIds, out IGH_DataTree geom, out List<string> outIds)
        {
            List<string> ids = null;
            List<Opening> Openings = null;
            DataTree<GeometryBase> geometry = new DataTree<GeometryBase>();

            app.Selection = m_Selection;
            outIds = app.GetOpenings(out Openings, objectIds);

            for (int i = 0; i < Openings.Count; i++)
            {
                geometry.AddRange(GeometryUtils.ConvertGroup<BHG.Curve>(Openings[i].Edges), new Grasshopper.Kernel.Data.GH_Path(i));
            }
            geom = geometry;
            return Openings;
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{9AC9B157-0C56-46BB-B7CA-7A82EB0AF823}"); }
        }
        /// <summary> Icon (24x24 pixels)</summary>     
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_Opening_Import; }
        }
    }
}
