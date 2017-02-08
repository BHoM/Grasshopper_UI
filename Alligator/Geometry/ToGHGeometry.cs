using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using RG = Rhino.Geometry;
using BHG = BHoM.Geometry;
using GHE = Grasshopper_Engine;


namespace Alligator.Geometry
{
    public class ToGHGeometry : GH_Component
    {
        public ToGHGeometry() : base("ToGHGeometry", "ToGHGeom", "Convert a BHoM geometry into a GH geometry", "Alligator", "geometry") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("5F1FE47C-FF14-4101-8048-3FBC00EB5767");
            }
        }
        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_Geo_BHToGH; }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("BH Geom", "BH Geom", "BHoM Geometry", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("GH Geom", "GH Geom", "Grasshopper Geometry", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHG.GeometryBase geometry = null;
            DA.GetData<BHG.GeometryBase>(0, ref geometry);

            DA.SetData(0, GHE.GeometryUtils.Convert(geometry));
        }
    }
}
