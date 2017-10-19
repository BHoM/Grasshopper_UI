using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BHG = BH.oM.Geometry;
using BH.UI.Alligator;
using BH.Adapter.Rhinoceros;
using BH.UI.Alligator.Base;

namespace BH.UI.Alligator.Geometry
{
    public class ToRhino : GH_Component
    {
        public ToRhino() : base("ToGHGeometry", "BtoG", "Convert a BHoM geometry into a GH geometry", "Alligator", "geometry") { }

        public override Guid ComponentGuid { get { return new Guid("5F1FE47C-FF14-4101-8048-3FBC00EB5767"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return Properties.Resources.BHoM_Geo_BHToGH; } }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new BHoMGeometryParameter(), "BH Geom", "BH Geom", "BHoM Geometry", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("GH Geom", "GH Geom", "Grasshopper Geometry", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHG.IBHoMGeometry geometry = default(BHG.IBHoMGeometry);
            geometry = DA.BH_GetData(0, ref geometry);

            //DA.SetData(0, geometry.ToRhino());
        }
    }
}
