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
    public class Polyline : GH_Component
    {
        public Polyline() : base("BHPolyline", "BHPolyline", "Create a BHoM Polyline", "Alligator", "geometry") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("389CA169-0E7B-4529-8271-E732A053A16E");
            }
        }
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Geometry.Properties.Resources.BHoM_PolyLine; }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("polyline", "polyline", "GH polyline to convert", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Polyline", "Polyline", "BHoM Polyline", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            RG.Curve inCurve = null;
            DA.GetData<RG.Curve>(0, ref inCurve);

            RG.Polyline inPolyline = null;
            bool working = inCurve.TryGetPolyline(out inPolyline);

            List<BHG.Point> bhPoints = new List<BHG.Point>();
            if (working)
            {
                foreach (RG.Point3d point in inPolyline)
                    bhPoints.Add(new BHG.Point(point.X, point.Y, point.Z));
            }
            
            BHG.Polyline Polyline = new BHG.Polyline(bhPoints);
            DA.SetData(0, Polyline);
        }
    }
}
