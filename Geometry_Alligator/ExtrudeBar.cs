//using BH.oM.Structural.Elements;
//using BHG = BH.oM.Geometry;
//using R = Rhino.Geometry;
//using Grasshopper.Kernel;
//using BH.Engine.Grasshopper;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using BH.Engine.Geometry;
//using BH.Engine.Structure;

//namespace BH.UI.Alligator.Geometry
//{
//    public class ExtrudeBar : GH_Component       //TODO: Requires A method to join curves in the engine
//    {
//        public ExtrudeBar() : base("Extrude Bar", "ExtrudeBar", "Extrudes a Bar cross section", "Structure", "Geometry")
//        {

//        }
//        protected override System.Drawing.Bitmap Internal_Icon_24x24
//        {
//            get { return Alligator.Properties.Resources.BHoM_Extrude_Bar; }
//        }

//        public override Guid ComponentGuid
//        {
//            get
//            {
//                return new Guid("{997457E6-C5C7-44AB-B75E-A61D7E0D0B05}");
//            }
//        }

//        protected override void RegisterInputParams(GH_InputParamManager pManager)
//        {
//            pManager.AddGenericParameter("Bar", "B", "Bar to Extrude", GH_ParamAccess.item);
//        }

//        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
//        {
//            pManager.AddBrepParameter("Extrusion", "E", "Extruded Bar", GH_ParamAccess.item);
//        }

//        protected override void SolveInstance(IGH_DataAccess DA)
//        {
//            BH.oM.Structural.Elements.Bar bar = DataUtils.GetGenericData<Bar>(DA, 0);
//            if (bar != null && bar.SectionProperty != null)
//            {
//                R.Curve centreline = GeometryUtils.Convert(bar.GetCentreline()).ToNurbsCurve();
//                double a = bar.SectionProperty.Area; //TODO: Verifiy that area means the same thing as GrossArea

//                List<BHG.ICurve> curves = BHG.ICurve.Join(bar.SectionProperty.Edges);

//                curves.Sort(delegate (BHG.ICurve c1, BHG.ICurve c2)
//                {
//                    return c2.GetLength().CompareTo(c1.GetLength());
//                });

//                R.Curve perimeter = GeometryUtils.Convert(curves[0]);
//                perimeter.Rotate(bar.OrientationAngle, R.Vector3d.ZAxis, R.Point3d.Origin);

//                DA.SetData(0, GeometryUtils.ExtrudeAlong(perimeter, centreline, new R.Plane(R.Point3d.Origin, R.Vector3d.XAxis, R.Vector3d.YAxis))[0].ToBrep().CapPlanarHoles(0.01));
//            }
//        }
//    }
//}
