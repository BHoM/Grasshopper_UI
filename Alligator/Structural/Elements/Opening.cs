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

    public class ExportOpening : GH_Component
    {
        public ExportOpening() : base("Export Opening", "SetOpening", "Creates or Replaces the geometry of a Opening", "Structure", "Elements") { }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "Application", "Application to export Openings to", GH_ParamAccess.item);
            pManager.AddGenericParameter("Openings", "Ids", "BHoM Openings to export", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Activate", "Activate", "Generate Openings", GH_ParamAccess.item);

            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("Ids", "Ids", "Opening Numbers", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (DataUtils.Run(DA, 2))
            {
                IElementAdapter app =DataUtils.GetGenericData<IElementAdapter>(DA, 0);
                if (app != null)
                {
                    List<Opening> Openings = DataUtils.GetGenericDataList<Opening>(DA, 1);
                    List<string> ids = null;
                    app.SetOpenings(Openings, out ids);

                    DA.SetDataList(0, ids);
                }
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
    }

    public class ImportOpening : ImportComponent
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

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (DataUtils.Run(DA, 2))
            {
                IElementAdapter app = DataUtils.GetGenericData<IElementAdapter>(DA, 0);
                if (app != null)
                {
                    List<string> ids = null;
                    List<Opening> Openings = null;
                    DataTree<GeometryBase> geometry = new DataTree<GeometryBase>();
                    if (m_Selection == ObjectSelection.FromInput)
                        ids = DataUtils.GetDataList<string>(DA, 1);

                    app.Selection = m_Selection;
                    ids = app.GetOpenings(out Openings, ids);

                    for (int i = 0; i < Openings.Count; i++)
                    {
                        geometry.AddRange(GeometryUtils.ConvertGroup<BHG.Curve>(Openings[i].Edges), new Grasshopper.Kernel.Data.GH_Path(i));
                    }

                    DA.SetDataList(0, ids);
                    DA.SetDataList(1, Openings);
                    DA.SetDataTree(2, geometry);
                }
            }
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
