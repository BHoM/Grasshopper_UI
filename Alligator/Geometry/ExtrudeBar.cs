using BHoM.Structural.Elements;
using BH = BHoM.Geometry;
using R = Rhino.Geometry;
using Grasshopper.Kernel;
using Grasshopper_Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alligator.Geometry
{
    public class ExtrudeBar : GH_Component
    {
        public ExtrudeBar() : base("Extrude Bar", "ExtrudeBar", "Extrudes a Bar cross section", "Structure", "Geometry")
        {

        }
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_Extrude_Bar; }
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("{997457E6-C5C7-44AB-B75E-A61D7E0D0B05}");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Bar", "B", "Bar to Extrude", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("Extrusion", "E", "Extruded Bar", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHoM.Structural.Elements.Bar bar = DataUtils.GetGenericData<Bar>(DA, 0);
            if (bar != null && bar.SectionProperty != null)
            {
                R.Curve centreline = GeometryUtils.Convert(bar.Line);
                double a = bar.SectionProperty.GrossArea;

                List<BH.Curve> curves = BH.Curve.Join(bar.SectionProperty.Edges);

                curves.Sort(delegate (BH.Curve c1, BH.Curve c2)
                {
                    return c2.Length.CompareTo(c1.Length);
                });
                
                R.Curve perimeter = GeometryUtils.Convert(curves[0]);
                perimeter.Rotate(bar.OrientationAngle, R.Vector3d.ZAxis, R.Point3d.Origin);

                DA.SetData(0, GeometryUtils.ExtrudeAlong(perimeter, centreline, new R.Plane(R.Point3d.Origin, R.Vector3d.XAxis, R.Vector3d.YAxis))[0].ToBrep().CapPlanarHoles(0.01));
            }
        }
    }
}
