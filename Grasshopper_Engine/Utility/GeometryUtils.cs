using BH.oM.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHG = BH.oM.Geometry;
using R = Rhino.Geometry;

namespace Grasshopper_Engine
{
    public static class GeometryUtils
    {
        ///**********************************************/
        ///****  Types                               ****/
        ///**********************************************/

        //public static Type GetRhinoType(Type bhType)
        //{
        //    if (bhType == typeof(BHG.ICurve))
        //    {
        //        return typeof(R.Curve);
        //    }
        //    else if (bhType == typeof(BHG.Point))
        //    {
        //        return typeof(R.Point);
        //    }
        //    else if (bhType == typeof(BHG.ISurface))
        //    {
        //        return typeof(R.Surface);
        //    }
        //    else if (bhType == typeof(BHG.Plane))
        //    {
        //        return typeof(R.Plane);
        //    }
        //    else if (bhType == typeof(BHG.Mesh))
        //    {
        //        return typeof(R.Mesh);
        //    }
        //    return null;
        //}

        ///**********************************************/

        //public static Type GetBHType(Type rhinoType)
        //{
        //    if (rhinoType == typeof(R.Curve) || rhinoType.IsAssignableFrom(typeof(R.Curve)))
        //    {
        //        return typeof(BHG.ICurve);
        //    }
        //    else if (rhinoType == typeof(R.Point))
        //    {
        //        return typeof(BHG.Point);
        //    }
        //    else if (rhinoType == typeof(R.Surface))
        //    {
        //        return typeof(BHG.ISurface);
        //    }
        //    else if (rhinoType == typeof(R.Brep))
        //    {
        //        return typeof(BHG.ISurface);
        //    }
        //    else if (rhinoType == typeof(R.Plane))
        //    {
        //        return typeof(BHG.Plane);
        //    }
        //    else if (rhinoType == typeof(R.Mesh))
        //    {
        //        return typeof(BHG.Mesh);
        //    }
        //    return null;
        //}


        ///**********************************************/
        ///****  Points & Vectors                    ****/
        ///**********************************************/

        //public static R.Point3d Convert(BHG.Point p)
        //{
        //    return new R.Point3d(p.X, p.Y, p.Z);
        //}

        ///**********************************************/

        //public static BHG.Point Convert(R.Point3d pnt)
        //{
        //    R.Point3d p = pnt;
        //    return new BHG.Point(p.X, p.Y, p.Z);
        //}

        ///**********************************************/

        //public static R.Vector3d Convert(BHG.Vector p)
        //{
        //    return new R.Vector3d(p.X, p.Y, p.Z);
        //}

        ///**********************************************/

        //public static BHG.Vector Convert(R.Vector3d v)
        //{
        //    return new BHG.Vector(v.X, v.Y, v.Z);
        //}

        ///**********************************************/

        //public static R.Plane Convert(BHG.Plane p)
        //{
        //    return new R.Plane(Convert(p.Origin), Convert(p.Normal));
        //}

        ///**********************************************/

        //public static BHG.Plane Convert(R.Plane plane)
        //{
        //    return new BHG.Plane(Convert(plane.Origin), Convert(plane.Normal));
        //}


        ///**********************************************/
        ///****  Curves                              ****/
        ///**********************************************/

        //public static BHG.Line Convert(R.Line line)
        //{
        //    return new BHG.Line(Convert(line.From), Convert(line.To));
        //}

        ///**********************************************/

        //public static R.Line Convert(BHG.Line line)
        //{
        //    return new R.Line(Convert(line.Start), Convert(line.End));
        //}

        ///**********************************************/

        //public static BHG.Circle Convert(R.Circle circle)
        //{
        //    return new BHG.Circle(circle.Radius, Convert(circle.Plane));
        //}

        ///**********************************************/

        //public static R.Circle Convert(BHG.Circle circle)
        //{
        //    return new R.Circle(Convert(circle.Plane), circle.Radius);
        //}

        ///**********************************************/

        ////public static BH.Curve Convert(R.Curve rCurve)
        ////{
        ////    if (rCurve is R.ArcCurve && (rCurve as R.ArcCurve).AngleRadians < Math.PI * 2 / 3)
        ////    {
        ////        R.Arc arc = (rCurve as R.ArcCurve).Arc;
        ////        return new BH.Arc(Convert(arc.StartPoint), Convert(arc.EndPoint), Convert(arc.MidPoint));
        ////    }
        ////    else if (rCurve is R.LineCurve)
        ////    {
        ////        return new BH.Line(Convert(rCurve.PointAtStart), Convert(rCurve.PointAtEnd));
        ////    }
        ////    else if (rCurve is R.PolylineCurve)
        ////    {
        ////        R.PolylineCurve pl = (rCurve as R.PolylineCurve);
        ////        List<R.Point3d> points = new List<R.Point3d>();
        ////        for (int i = 0; i < pl.PointCount; i++)
        ////        {
        ////            points.Add(pl.Point(i));
        ////        }
        ////        return new BH.Polyline(ConvertPointCollection(points).ToList());
        ////    }
        ////    else
        ////    {
        ////        R.NurbsCurve nurbCurve = rCurve.ToNurbsCurve();
        ////        int degree = nurbCurve.Degree;
        ////        double[] knots = new double[nurbCurve.Knots.Count + 2];
        ////        List<BH.Point> points = new List<BH.oM.Geometry.Point>();
        ////        double[] weight = new double[nurbCurve.Points.Count];
        ////        knots[0] = nurbCurve.Knots[0];
        ////        knots[knots.Length - 1] = nurbCurve.Knots[nurbCurve.Knots.Count - 1];
        ////        for (int i = 1; i < nurbCurve.Knots.Count + 1; i++)
        ////        {
        ////            knots[i] = nurbCurve.Knots[i - 1];

        ////        }

        ////        for (int i = 0; i < nurbCurve.Points.Count; i++)
        ////        {
        ////            points.Add(Convert(nurbCurve.Points[i].Location));
        ////            weight[i] = nurbCurve.Points[i].Weight;
        ////        }
        ////        return BH.NurbCurve.Create(points, degree, knots, weight);
        ////    }
        ////}

        ///**********************************************/

        //public static R.Curve Convert(BHG.Curve curve)
        //{
        //    R.NurbsCurve c = new R.NurbsCurve(curve.Degree, curve.PointCount);// R.NurbsCurve.c.Create(false, curve.Degree, ConvertPointCollection(curve.ControlPoints));
        //    for (int i = 1; i < curve.Knots.Length - 1; i++)
        //    {
        //        if (c.Knots.Count < i)
        //        {
        //            c.Knots.InsertKnot(curve.Knots[i]);
        //        }
        //        else
        //        {
        //            c.Knots[i - 1] = curve.Knots[i];
        //        }
        //    }
        //    int index = 0;
        //    foreach (BH.Point p in curve.ControlPoints)
        //    {
        //        c.Points.SetPoint(index, p.X, p.Y, p.Z, curve.Weights[index]);
        //        index++;
        //    }

        //    return c;
        //}

        ///**********************************************/
        ///****  Surfaces                            ****/
        ///**********************************************/

        //public static BH.Surface Convert(R.Surface surface)
        //{
        //    return null; //TODO: Eddy to provide a proper constructor for Surface
        //}
                
        ///**********************************************/

        //public static R.Surface Convert(BH.Surface surface)
        //{
        //    return null; //TODO: Set up the proper convertion
        //}

        ///**********************************************/

        //public static R.Brep Convert(BH.Brep brep)
        //{
        //    if (brep is BH.Extrusion)
        //    {
        //        return Convert(brep as BH.Extrusion);
        //    }
        //    else if (brep is BH.Pipe)
        //    {
        //        return Convert(brep as BH.Pipe);
        //    }

        //    return null;
        //}

        ///**********************************************/

        //public static R.Brep Convert(BH.Extrusion extrusion)
        //{
        //    R.Surface result = R.Extrusion.CreateExtrusion(Convert(extrusion.Curve), Convert(extrusion.Direction));
        //    return extrusion.Capped ? result.ToBrep().CapPlanarHoles(0.001) : result.ToBrep();
        //}

        ///**********************************************/

        //public static R.Brep Convert(BH.Pipe pipe)
        //{
        //    R.Brep[] result = R.Brep.CreatePipe(Convert(pipe.Centreline), pipe.Radius, true, pipe.Capped ? R.PipeCapMode.Flat : R.PipeCapMode.None, true, 0.001, 0.001);
        //    if (result.Length > 0)
        //    {
        //        return result[0];
        //    }
        //    return null;
        //}

        ///**********************************************/

        //public static BH.Brep Convert(R.Brep brep)
        //{
        //    return null; // TODO: Create the proper convertion (Eddy?)
        //}


        ///**********************************************/
        ///****  Geoemtry bases                      ****/
        ///**********************************************/

        ////public static R.GeometryBase Convert(BH.GeometryBase geom)
        ////{
        ////    if (geom is BH.Curve)
        ////    {
        ////        return Convert(geom as BH.Curve);
        ////    }
        ////    else if (typeof(BH.Brep).IsAssignableFrom(geom.GetType()))
        ////    {
        ////        return Convert(geom as BH.Brep);
        ////    }
        ////    else if (geom is BH.Point)
        ////    {
        ////        return new R.Point(Convert(geom as BH.Point));
        ////    }
            
        ////    return null;
        ////}

        /////**********************************************/

        ////public static BH.GeometryBase Convert(R.GeometryBase geom)
        ////{
        ////    if (geom is R.Point)
        ////    {
        ////        return Convert(geom as R.Point);
        ////    }
        ////    else if (geom is R.Curve)
        ////    {
        ////        return Convert(geom as R.Curve);
        ////    }
        ////    return null;
        ////}


        /////**********************************************/
        /////****  Collections                         ****/
        /////**********************************************/

        ////private static IEnumerable<R.Point3d> ConvertPointCollection(IEnumerable<BH.Point> pnts)
        ////{
        ////    List<R.Point3d> result = new List<Rhino.Geometry.Point3d>();
        ////    foreach (BH.Point p in pnts)
        ////    {
        ////        result.Add(Convert(p));
        ////    }
        ////    return result;
        ////}

        /////**********************************************/

        ////private static IEnumerable<BH.Point> ConvertPointCollection(IEnumerable<R.Point3d> pnts)
        ////{
        ////    List<BH.Point> result = new List<BH.Point>();
        ////    foreach (R.Point3d p in pnts)
        ////    {
        ////        result.Add(Convert(p));
        ////    }
        ////    return result;
        ////}

        /////**********************************************/

        ////public static BH.Group<TBH> ConvertList<TBH, TR>(List<TR> geom) where TBH : BH.GeometryBase where TR : R.GeometryBase
        ////{
        ////    BH.Group<TBH> group = new BH.oM.Geometry.Group<TBH>();
        ////    for (int i = 0; i < geom.Count; i++)
        ////    {
        ////        group.Add(Convert(geom[i]) as TBH);
        ////    }

        ////    return group;
        ////}

        /////**********************************************/

        ////public static List<R.GeometryBase> ConvertGroup<T>(BH.Group<T> geom) where T : BH.GeometryBase
        ////{
        ////    List<R.GeometryBase> rGeom = new List<Rhino.Geometry.GeometryBase>();
        ////    foreach (T item in geom)
        ////    {
        ////        if (typeof(BH.IGroup).IsAssignableFrom(item.GetType()))
        ////        {
        ////            rGeom.AddRange(ConvertGroup<BH.GeometryBase>(((BH.IGroup)item).Cast<BH.GeometryBase>()));
        ////        }
        ////        else
        ////        {
        ////            rGeom.Add(Convert(item));
        ////        }
        ////    }
        ////    return rGeom;
        ////}


        ///********************************************/
        ///**** Extention convert methods          ****/
        ///********************************************/


        //public static BH.Vector ToBH.oMVector(this R.Vector3d vec)
        //{
        //    return Convert(vec);
        //}

        //public static List<R.Surface> ExtrudeAlong(R.Curve section, R.Curve centreline, R.Plane sectionPlane)
        //{
        //    R.Vector3d globalUp = R.Vector3d.ZAxis;
        //    R.Vector3d localX = sectionPlane.XAxis;
        //    R.Curve[] baseCurves = centreline.DuplicateSegments();
        //    List<R.Surface> extrustions = new List<R.Surface>();
        //    if (baseCurves.Length == 0) baseCurves = new R.Curve[] { centreline };
        //    for (int i = 0; i < baseCurves.Length; i++)
        //    {
        //        R.Vector3d v = baseCurves[i].PointAtEnd - baseCurves[i].PointAtStart;
        //        R.Curve start = section.Duplicate() as R.Curve;
        //        if (v.IsParallelTo(globalUp) == 0)
        //        {
        //            R.Vector3d direction = sectionPlane.Normal;
        //            double angle = R.Vector3d.VectorAngle(v, direction);
        //            R.Transform alignPerpendicular = R.Transform.Rotation(-angle, R.Vector3d.CrossProduct(v, R.Vector3d.ZAxis), R.Point3d.Origin);
        //            localX.Transform(alignPerpendicular);
        //            direction.Transform(alignPerpendicular);
        //            double angleAxisAlign = R.Vector3d.VectorAngle(localX, R.Vector3d.CrossProduct(globalUp, v));
        //            if (localX * globalUp > 0) angleAxisAlign = -angleAxisAlign;
        //            R.Transform axisAlign = R.Transform.Rotation(angleAxisAlign, v, R.Point3d.Origin);
        //            R.Transform result = R.Transform.Translation(baseCurves[i].PointAtStart - R.Point3d.Origin) * axisAlign * alignPerpendicular;// * axisAlign *                

        //            start.Transform(result);
        //        }
        //        else
        //        {
        //            start.Translate(baseCurves[i].PointAtStart - R.Point3d.Origin);
        //        }
        //        extrustions.Add(R.Extrusion.CreateExtrusion(start, v));
        //    }
        //    return extrustions;

        //}
    }
}
