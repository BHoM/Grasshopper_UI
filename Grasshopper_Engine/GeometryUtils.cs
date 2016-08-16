using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH = BHoM.Geometry;
using R = Rhino.Geometry;

namespace Grasshopper_Engine
{
    public class GeometryUtils
    {

        private static IEnumerable<R.Point3d> ConvertPointCollection(IEnumerable<BH.Point> pnts)
        {
            List<R.Point3d> result = new List<Rhino.Geometry.Point3d>();
            foreach (BH.Point p in pnts)
            {
                result.Add(Convert(p));
            }
            return result;
        }

        private static IEnumerable<BH.Point> ConvertPointCollection(IEnumerable<R.Point3d> pnts)
        {
            List<BH.Point> result = new List<BH.Point>();
            foreach (R.Point3d p in pnts)
            {
                result.Add(Convert(p));
            }
            return result;
        }

        public static R.Curve Convert(BH.Curve curve)
        {
            R.NurbsCurve c = new R.NurbsCurve(curve.Degree, curve.PointCount);// R.NurbsCurve.c.Create(false, curve.Degree, ConvertPointCollection(curve.ControlPoints));
            for (int i = 1; i < curve.Knots.Length; i++)
            {
                if (c.Knots.Count < i)
                {
                    c.Knots.InsertKnot(curve.Knots[i]);
                }
                else
                {
                    c.Knots[i - 1] = curve.Knots[i];
                }
            }
            int index = 0;
            foreach (BH.Point p in curve.ControlPoints)
            {
                c.Points.SetPoint(index, p.X, p.Y, p.Z, curve.Weights[index]);
                index++;
            }

            return c;
        }

        public static R.Point3d Convert(BH.Point p)
        {
            return new R.Point3d(p.X, p.Y, p.Z);
        }

        public static BH.Point Convert(R.Point3d pnt)
        {
            R.Point3d p = pnt;
            return new BH.Point(p.X, p.Y, p.Z);
        }

        public static BH.Curve Convert(R.Curve rCurve)
        {
            if (rCurve is R.ArcCurve)
            {
                R.Arc arc = (rCurve as R.ArcCurve).Arc;
                return new BH.Arc(Convert(arc.StartPoint), Convert(arc.EndPoint), Convert(arc.MidPoint));
            }
            else if (rCurve is R.LineCurve)
            {
                return new BH.Line(Convert(rCurve.PointAtStart), Convert(rCurve.PointAtEnd));
            }
            else if (rCurve is R.PolylineCurve)
            {
                R.PolylineCurve pl = (rCurve as R.PolylineCurve);
                List<R.Point3d> points = new List<R.Point3d>();
                for (int i = 0; i < pl.PointCount; i++)
                {
                    points.Add(pl.Point(i));
                }
                return new BH.Polyline(ConvertPointCollection(points).ToList());
            }
            else
            {
                R.NurbsCurve nurbCurve = rCurve.ToNurbsCurve();
                int degree = nurbCurve.Degree;
                double[] knots = nurbCurve.Knots.ToArray();
                List<BH.Point> points = new List<BHoM.Geometry.Point>();
                double[] weight = new double[nurbCurve.Points.Count];
                for (int i = 0; i < nurbCurve.Points.Count; i++)
                {
                    points.Add(Convert(nurbCurve.Points[i].Location));
                    weight[i] = nurbCurve.Points[i].Weight;
                }
                return BH.NurbCurve.Create(points, degree, knots, weight);
            }          
        }

        public static List<R.GeometryBase> Convert<T>(BH.Group<T> geom) where T : BH.GeometryBase
        {
            List<R.GeometryBase> rGeom = new List<Rhino.Geometry.GeometryBase>();
            foreach (T item in geom)
            {
                rGeom.Add(Convert(item));
            }
            return rGeom;
        }

        public static R.GeometryBase Convert(BH.GeometryBase geom)
        {
            if (geom is BH.Curve)
            {
                return Convert(geom as BH.Curve);               
            }
            else if (geom is BH.Brep)
            {

            }
            else if (geom is BH.Point)
            {
                return new R.Point(Convert(geom as BH.Point));
            }
            return null;
        }

        public static BH.GeometryBase Convert(R.GeometryBase geom)
        {
            if (geom is R.Point)
            {
                return Convert(geom as R.Point);
            }
            else if (geom is R.Curve)
            {
                return Convert(geom as R.Curve);
            }
            return null;
        }
    }
}
