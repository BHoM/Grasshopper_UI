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
    public class CreateLevel : BHoMBaseComponent<Storey>
    {
        public CreateLevel() : base("Create Level", "CreateLevel", "Create a BH Level object", "Structure", "Elements") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("{1239B157-0C56-46CC-B7CA-7A82EB0AF823}");
            }
        }

        /// <summary> Icon (24x24 pixels)</summary>     
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_Level; }
        }
    }

    public class ExportLevel : GH_Component
    {
        public ExportLevel() : base("Export Level", "SetLevel", "Creates or Replaces the geometry of a Level", "Structure", "Elements") { }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "Application", "Application to export Levels to", GH_ParamAccess.item);
            pManager.AddGenericParameter("Levels", "Level", "BHoM Levels to export", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Activate", "Activate", "Generate Levels", GH_ParamAccess.item);

            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Ids", "Ids", "Level Numbers", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (DataUtils.Run(DA, 2))
            {
                IElementAdapter app = DataUtils.GetGenericData<IElementAdapter>(DA, 0);
                if (app != null)
                {
                    List<Storey> Levels = DataUtils.GetGenericDataList<Storey>(DA, 1);
                    List<string> ids = null;
                    app.SetLevels(Levels, out ids);

                    DA.SetDataList(0, ids);
                }
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{9FF9B157-0C56-46CC-B7CA-7A82EB0AF823}"); }
        }
        /// <summary> Icon (24x24 pixels)</summary>     
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_Level_Export; }
        }
    }

    public class ImportLevel : ImportComponent<Storey>
    {
        public ImportLevel() : base("Import Level", "GetLevel", "Get the geometry and properties of a Level", "Structure", "Elements")
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
            if (DataUtils.Run(DA, 2))
            {
                IElementAdapter app = DataUtils.GetGenericData<IElementAdapter>(DA, 0);
                if (app != null)
                {
                    List<string> ids = null;
                    List<Storey> Levels = null;
                    DataTree<Plane> geometry = new DataTree<Plane>();
                    if (m_Selection == ObjectSelection.FromInput)
                        ids = DataUtils.GetDataList<string>(DA, 1);

                    app.Selection = m_Selection;
                    ids = app.GetLevels(out Levels, ids);

                    for (int i = 0; i < Levels.Count; i++)
                    {
                        geometry.Add(GeometryUtils.Convert(Levels[i].Plane));
                    }

                    DA.SetDataList(0, ids);
                    DA.SetDataList(1, Levels);
                    DA.SetDataTree(2, geometry);
                }
            }
        }

        public override List<Storey> GetObjects(IElementAdapter app, List<string> objectIds, out IGH_DataTree geom, out List<string> outIds)
        {
            List<Storey> Levels = null;
            DataTree<Plane> geometry = new DataTree<Plane>();

            app.Selection = m_Selection;
            outIds = app.GetLevels(out Levels, objectIds);

            for (int i = 0; i < Levels.Count; i++)
            {
                geometry.Add(GeometryUtils.Convert(Levels[i].Plane));
            }
            geom = geometry;
            return Levels;
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{9AC9B157-0C56-46BB-B7CA-7A82ECCAF823}"); }
        }
        /// <summary> Icon (24x24 pixels)</summary>     
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_Level_Import; }
        }
    }
}
