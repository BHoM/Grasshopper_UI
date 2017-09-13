using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using RG = Rhino.Geometry;
using BHG = BH.oM.Geometry;

namespace BH.UI.Alligator.Geometry
{
    public class Point : GH_Component
    {
        public Point() : base("BHPoint", "BHPoint", "Create a BHoM Point", "Alligator", "geometry") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("59458E1C-5E42-4A84-B6B7-3DC3F185D355");
            }
        }
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Geometry.Properties.Resources.BHoM_Point; }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("point", "point", "GH point", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Point", "Point", "BHoM Point", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            RG.Point3d inPt = new RG.Point3d();

            DA.GetData<RG.Point3d>(0, ref inPt);

            BHG.Point Point = new BHG.Point(inPt.X, inPt.Y, inPt.Z);

            DA.SetData(0, Point);
        }
    }
}
