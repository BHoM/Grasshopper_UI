using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Structural;
using Alligator.Components;
using Grasshopper.Kernel;
using GHE = Grasshopper_Engine;
using BHG = BH.oM.Geometry;
using BHE = BH.oM.Structural.Elements;
using BHI = BH.oM.Structural.Interface;
using Rhino.Geometry;
using Grasshopper;
using Grasshopper_Engine.Components;
using Grasshopper.Kernel.Data;

namespace Alligator.Structural.Elements
{
    public class CreateGrid : BHoMBaseComponent<BHE.Grid>
    {
        public CreateGrid() : base("Create Grid", "CreateGrid", "Create a BH Grid object", "Structure", "Elements") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("9E64C671-01BD-45B9-94D3-554BD2F8BA52");
            }
        }
        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BH.oM_Grid; }
        }
    }

    public class ExportGrid : GH_Component
    {
        public ExportGrid() : base("Export Grid", "SetGrid", "Creates or Replaces the geometry of a Grid", "Structure", "Elements") { }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "App", "Application to export bars to", GH_ParamAccess.item);
            pManager.AddGenericParameter("Grids", "P", "BH.oM Grids to export", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Execute", "R", "Generate Grids", GH_ParamAccess.item);

            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("Ids", "Ids", "Bar Numbers", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (GHE.DataUtils.Run(DA, 2))
            {
                BHI.IElementAdapter app = GHE.DataUtils.GetGenericData<BHI.IElementAdapter>(DA, 0);
                if (app != null)
                {
                    List<BHE.Grid> Grids = GHE.DataUtils.GetGenericDataList<BHE.Grid>(DA, 1);
                    List<string> ids = null;
                    app.SetGrids(Grids, out ids);

                    DA.SetDataList(0, ids);
                }
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("72fb2007-5da5-46A7-b481-6e134df8c583"); }
        }
        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BH.oM_Grid_Export; }
        }
    }

    public class ImportGrid : ImportComponent<BHE.Grid>
    {
        public ImportGrid() : base("Import Grid", "GetGrid", "Get the geometry and properties of a Grid", "Structure", "Elements")
        {

        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.tertiary;
            }
        }

        public override List<BHE.Grid> GetObjects(BHI.IElementAdapter app, List<string> objectIds, out IGH_DataTree geom, out List<string> outIds)
        {
            List<BHE.Grid> Grids = null;
            DataTree<Curve> geometry = new DataTree<Curve>();
            app.Selection = m_Selection;
            outIds = app.GetGrids(out Grids, objectIds);

            for (int i = 0; i < Grids.Count; i++)
            {
                geometry.Add(GHE.GeometryUtils.Convert(Grids[i].Line));
            }
            geom = geometry;
            return Grids;
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("5520dc1b-87b3-491a-93fa-149F215ce5a2"); }
        }
        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BH.oM_Grid_Import; }
        }
    }
}
