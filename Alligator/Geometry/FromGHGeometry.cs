using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using RG = Rhino.Geometry;
using BHG = BHoM.Geometry;
using GHE = Grasshopper_Engine;


namespace Alligator.Geometry
{
    public class FromGHGeometry : GH_Component
    {
        public FromGHGeometry() : base("FromGHGeometry", "GtoB", "Convert a GH geometry into a BHoM geometry", "Alligator", "geometry") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("F99DCFA4-AB31-43FE-9C15-084E7BA184D1");
            }
        }
        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_Geo_GHToBH; }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("GH Geom", "GH Geom", "Grasshopper Geometry", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("BH Geom", "BH Geom", "BH Geometry", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            object geometry = null;
            DA.GetData<object>(0, ref geometry);

            object result = null;
            if (geometry is GH_Point)
                result = GHE.GeometryUtils.Convert(((GH_Point)geometry).Value);
            else if (geometry is GH_Vector)
                result = GHE.GeometryUtils.Convert(((GH_Vector)geometry).Value);
            else if (geometry is GH_Line)
                result = GHE.GeometryUtils.Convert(((GH_Line)geometry).Value);
            else if (geometry is GH_Circle)
                result = GHE.GeometryUtils.Convert(((GH_Circle)geometry).Value);
            else if (geometry is GH_Curve)
                result = GHE.GeometryUtils.Convert(((GH_Curve)geometry).Value);
            else if (geometry is GH_Plane)
                result = GHE.GeometryUtils.Convert(((GH_Plane)geometry).Value);
            else if (geometry is GH_Surface)
                result = GHE.GeometryUtils.Convert(((GH_Surface)geometry).Value);
            else if (geometry is GH_Brep)
                result = GHE.GeometryUtils.Convert(((GH_Brep)geometry).Value);
            else if (geometry is GH_Mesh)
                result = GHE.GeometryUtils.Convert(((GH_Mesh)geometry).Value);

            DA.SetData(0, result);
        }
    }
}