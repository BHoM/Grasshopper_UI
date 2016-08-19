using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHoM.Structural;
using Alligator.Components;
using Grasshopper.Kernel;
using GHE = Grasshopper_Engine;
using BHG = BHoM.Geometry;
using BHE = BHoM.Structural.Elements;
using BHI = BHoM.Structural.Interface;
using Rhino.Geometry;
using Grasshopper;

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
    }

    public class ExportGrid : GH_Component
    {
        public ExportGrid() : base("Export Grid", "ExGrid", "Creates or Replaces the geometry of a Grid", "Structure", "Elements") { }

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
            pManager.AddGenericParameter("Grids", "P", "BHoM Grids to export", GH_ParamAccess.list);
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
    }

    public class ImportGrid : ImportComponent
    {
        public ImportGrid() : base("Import Grid", "GridNode", "Get the geometry and properties of a Grid", "Structure", "Elements")
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
                    List<BHE.Grid> Grids = null;
                    DataTree<Curve> geometry = new DataTree<Curve>();
                    if (m_Selection == BHI.ObjectSelection.FromInput)
                        ids = GHE.DataUtils.GetDataList<string>(DA, 1);

                    app.Selection = m_Selection;
                    ids = app.GetGrids(out Grids, ids);

                    for (int i = 0; i < Grids.Count; i++)
                    {
                        geometry.Add(GHE.GeometryUtils.Convert(Grids[i].Line));
                    }

                    DA.SetDataList(0, ids);
                    DA.SetDataList(1, Grids);
                    DA.SetDataTree(2, geometry);
                }
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("5520dc1b-87b3-491a-93fa-149F215ce5a2"); }
        }
    }
}
