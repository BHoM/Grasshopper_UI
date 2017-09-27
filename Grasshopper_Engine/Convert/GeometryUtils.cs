using BH.oM.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHG = BH.oM.Geometry;
using R = Rhino.Geometry;

namespace BH.Engine.Grasshopper
{
    public static class GeometryUtils       // TODO: This needs to be moved to the Rhino_Adapter
    {
        /**********************************************/
        /****  Types                               ****/
        /**********************************************/

        public static Type GetRhinoType(Type bhType)
        {
            if (bhType == typeof(BHG.ICurve))
            {
                return typeof(R.Curve);
            }
            else if (bhType == typeof(BHG.Point))
            {
                return typeof(R.Point);
            }
            else if (bhType == typeof(BHG.ISurface))
            {
                return typeof(R.Surface);
            }
            else if (bhType == typeof(BHG.Plane))
            {
                return typeof(R.Plane);
            }
            else if (bhType == typeof(BHG.Mesh))
            {
                return typeof(R.Mesh);
            }
            return null;
        }

        /**********************************************/

        public static Type GetBHType(Type rhinoType)
        {
            if (rhinoType == typeof(R.Curve) || rhinoType.IsAssignableFrom(typeof(R.Curve)))
            {
                return typeof(BHG.ICurve);
            }
            else if (rhinoType == typeof(R.Point))
            {
                return typeof(BHG.Point);
            }
            else if (rhinoType == typeof(R.Surface))
            {
                return typeof(BHG.ISurface);
            }
            else if (rhinoType == typeof(R.Brep))
            {
                return typeof(BHG.ISurface);
            }
            else if (rhinoType == typeof(R.Plane))
            {
                return typeof(BHG.Plane);
            }
            else if (rhinoType == typeof(R.Mesh))
            {
                return typeof(BHG.Mesh);
            }
            return null;
        }


        /**********************************************/
        /****  Points & Vectors                    ****/
        /**********************************************/

        public static R.Point3d Convert(BHG.Point p)
        {
            return new R.Point3d(p.X, p.Y, p.Z);
        }

        /**********************************************/

        public static BHG.Point Convert(R.Point3d pnt)
        {
            R.Point3d p = pnt;
            return new BHG.Point(p.X, p.Y, p.Z);
        }

        /**********************************************/

        public static R.Vector3d Convert(BHG.Vector p)
        {
            return new R.Vector3d(p.X, p.Y, p.Z);
        }

        /**********************************************/

        public static BHG.Vector Convert(R.Vector3d v)
        {
            return new BHG.Vector(v.X, v.Y, v.Z);
        }

        /**********************************************/

        public static R.Plane Convert(BHG.Plane p)
        {
            return new R.Plane(Convert(p.Origin), Convert(p.Normal));
        }

        /**********************************************/

        public static BHG.Plane Convert(R.Plane plane)
        {
            return new BHG.Plane(Convert(plane.Origin), Convert(plane.Normal));
        }


        /**********************************************/
        /****  Curves                              ****/
        /**********************************************/

        public static BHG.Line Convert(R.Line line)
        {
            return new BHG.Line(Convert(line.From), Convert(line.To));
        }

        /**********************************************/

        public static R.Line Convert(BHG.Line line)
        {
            return new R.Line(Convert(line.Start), Convert(line.End));
        }

        /**********************************************/

        public static BHG.Circle Convert(R.Circle circle)
        {
            return new BHG.Circle(Convert(circle.Plane.Origin), Convert(circle.Plane.Normal), circle.Radius);
        }

        /**********************************************/

        public static R.Circle Convert(BHG.Circle circle)
        {
            return new R.Circle(new R.Plane(Convert(circle.Centre), Convert(circle.Normal)), circle.Radius);
        }

        /**********************************************/

        public static BHG.ICurve Convert(R.Curve rCurve)
        {
            if (rCurve is R.ArcCurve && (rCurve as R.ArcCurve).AngleRadians < Math.PI * 2 / 3)
            {
                R.Arc arc = (rCurve as R.ArcCurve).Arc;
                return new BHG.Arc(Convert(arc.StartPoint), Convert(arc.EndPoint), Convert(arc.MidPoint));
            }
            else if (rCurve is R.LineCurve)
            {
                return new BHG.Line(Convert(rCurve.PointAtStart), Convert(rCurve.PointAtEnd));
            }
            else if (rCurve is R.PolylineCurve)
            {
                R.PolylineCurve pl = (rCurve as R.PolylineCurve);
                List<R.Point3d> points = new List<R.Point3d>();
                for (int i = 0; i < pl.PointCount; i++)
                {
                    points.Add(pl.Point(i));
                }
                return new BHG.Polyline(points.Select(x => Convert(x)).ToList());
            }
            else
            {
                R.NurbsCurve nurbCurve = rCurve.ToNurbsCurve();
                int degree = nurbCurve.Degree;
                double[] knots = new double[nurbCurve.Knots.Count + 2];
                List<BHG.Point> points = new List<BHG.Point>();
                double[] weight = new double[nurbCurve.Points.Count];
                knots[0] = nurbCurve.Knots[0];
                knots[knots.Length - 1] = nurbCurve.Knots[nurbCurve.Knots.Count - 1];
                for (int i = 1; i < nurbCurve.Knots.Count + 1; i++)
                {
                    knots[i] = nurbCurve.Knots[i - 1];

                }

                for (int i = 0; i < nurbCurve.Points.Count; i++)
                {
                    points.Add(Convert(nurbCurve.Points[i].Location));
                    weight[i] = nurbCurve.Points[i].Weight;
                }
                return new BHG.NurbCurve(points, knots, weight);
            }
        }

        /**********************************************/

        public static R.Curve Convert(BHG.ICurve curve) //TODO: is there a common type between all the rhino curve types ?
        {
            if (curve is BHG.Arc)
                return Convert(curve as BHG.Arc);
            else if (curve is BHG.Circle)
                return Convert(curve as BHG.Circle).ToNurbsCurve();
            else if (curve is BHG.Line)
                return Convert(curve as BHG.Line).ToNurbsCurve();
            else if (curve is BHG.NurbCurve)
                return Convert(curve as BHG.NurbCurve);
            else if (curve is BHG.PolyCurve)
                return Convert(curve as BHG.PolyCurve);
            else if (curve is BHG.Polyline)
                return Convert(curve as BHG.Polyline);
            else
                return null;
        }

        /**********************************************/

        public static R.Curve Convert(BHG.NurbCurve curve)
        {
            R.NurbsCurve c = new R.NurbsCurve(curve.GetDegree(), curve.ControlPoints.Count);// R.NurbsCurve.c.Create(false, curve.Degree, ConvertPointCollection(curve.ControlPoints));
            for (int i = 1; i < curve.Knots.Count - 1; i++)
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
            foreach (BHG.Point p in curve.ControlPoints)
            {
                c.Points.SetPoint(index, p.X, p.Y, p.Z, curve.Weights[index]);
                index++;
            }

            return c;
        }

        /**********************************************/
        /****  Surfaces                            ****/
        /**********************************************/

        public static R.Brep Convert(BHG.ISurface surface)
        {
            if (surface is BHG.Extrusion)
            {
                return Convert(surface as BHG.Extrusion);
            }
            else if (surface is BHG.Pipe)
            {
                return Convert(surface as BHG.Pipe);
            }

            return null;
        }

        /**********************************************/

        public static R.Brep Convert(BHG.Extrusion extrusion)
        {
            R.Surface result = R.Extrusion.CreateExtrusion(Convert(extrusion.Curve), Convert(extrusion.Direction));
            return extrusion.Capped ? result.ToBrep().CapPlanarHoles(0.001) : result.ToBrep();
        }

        /**********************************************/

        public static R.Brep Convert(BHG.Pipe pipe)
        {
            R.Brep[] result = R.Brep.CreatePipe(Convert(pipe.Centreline), pipe.Radius, true, pipe.Capped ? R.PipeCapMode.Flat : R.PipeCapMode.None, true, 0.001, 0.001);
            if (result.Length > 0)
            {
                return result[0];
            }
            return null;
        }

        /**********************************************/

        public static BHG.ISurface Convert(R.Brep brep)
        {
            return null; // TODO: Create the proper convertion (Eddy?)
        }


        /**********************************************/
        /****               Meshes                 ****/
        /**********************************************/

        public static BHG.Mesh Convert(R.Mesh rMesh)
        {
            List<R.Point3f> rVertices = rMesh.Vertices.ToList();
            List<BHG.Point> vertices = new List<BHG.Point>();
            for (int i = 0; i < rVertices.Count; i++)
            {
                BHG.Point Vertex = Convert(rVertices[i]); vertices.Add(Vertex);
            }
            List<R.MeshFace> rFaces = rMesh.Faces.ToList();
            List<BHG.Face> Faces = new List<BHG.Face>();                  //BH Faces list
            for (int i = 0; i < rFaces.Count; i++)
            {
                if (rFaces[i].IsQuad)
                {
                    BHG.Face Face = new BHG.Face(rFaces[i].A, rFaces[i].B, rFaces[i].C, rFaces[i].D);
                    Faces.Add(Face);
                }
                if ((rFaces[i].IsTriangle))
                {
                    BHG.Face Face = new BHG.Face(rFaces[i].A, rFaces[i].B, rFaces[i].C);
                    Faces.Add(Face);
                }
            }
            return new BHG.Mesh(vertices, Faces);
        }

        /**********************************************/

        public static R.Mesh Convert(BHG.Mesh mesh)
        {
            List<BHG.Point> vertices = mesh.Vertices.ToList();
            List<R.Point3d> rVertices = new List<R.Point3d>();      //Rhino Vertices list
            for (int i = 0; i < vertices.Count; i++)
            {
                R.Point3d rVertex = Convert(vertices[i]);
                rVertices.Add(rVertex);
            }
            List<BHG.Face> faces = mesh.Faces;
            List<R.MeshFace> rFaces = new List<R.MeshFace>();       // Rhino MeshFaces list
            for (int i = 0; i < faces.Count; i++)
            {
                if (faces[i].IsQuad())
                {
                    R.MeshFace rFace = new R.MeshFace(faces[i].A, faces[i].B, faces[i].C, faces[i].D);
                    rFaces.Add(rFace);
                }
                else
                {
                    R.MeshFace rFace = new R.MeshFace(faces[i].A, faces[i].B, faces[i].C);
                    rFaces.Add(rFace);
                }
            }
            R.Mesh rMesh = new R.Mesh();
            rMesh.Faces.AddFaces(rFaces);
            rMesh.Vertices.AddVertices(rVertices);
            return rMesh;
        }


        /**********************************************/
        /****  Geoemtry bases                      ****/
        /**********************************************/

        public static R.GeometryBase Convert(BHG.IBHoMGeometry geom) //TODO: This is incomplete. Needs to foolow 2.0 format anyway
        {
            if (geom is BHG.ICurve)
            {
                return Convert(geom as BHG.ICurve);
            }
            else if (geom is BHG.ISurface)
            {
                return Convert(geom as BHG.ISurface);
            }
            else if (geom is BHG.Point)
            {
                return new R.Point(Convert(geom as BHG.Point));
            }
            else if (geom is BHG.Mesh)
            {
                return Convert(geom as BHG.Mesh);
            }

            return null;
        }

        /**********************************************/

        public static BHG.IBHoMGeometry Convert(R.GeometryBase geom)
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


        ///**********************************************/
        ///****  Collections                         ****/
        ///**********************************************/

        //private static IEnumerable<R.Point3d> ConvertPointCollection(IEnumerable<BH.Point> pnts)
        //{
        //    List<R.Point3d> result = new List<Rhino.Geometry.Point3d>();
        //    foreach (BH.Point p in pnts)
        //    {
        //        result.Add(Convert(p));
        //    }
        //    return result;
        //}

        ///**********************************************/

        //private static IEnumerable<BH.Point> ConvertPointCollection(IEnumerable<R.Point3d> pnts)
        //{
        //    List<BH.Point> result = new List<BH.Point>();
        //    foreach (R.Point3d p in pnts)
        //    {
        //        result.Add(Convert(p));
        //    }
        //    return result;
        //}

        ///**********************************************/

        //public static BH.Group<TBH> ConvertList<TBH, TR>(List<TR> geom) where TBH : BH.GeometryBase where TR : R.GeometryBase
        //{
        //    BH.Group<TBH> group = new BH.oM.Geometry.Group<TBH>();
        //    for (int i = 0; i < geom.Count; i++)
        //    {
        //        group.Add(Convert(geom[i]) as TBH);
        //    }

        //    return group;
        //}

        ///**********************************************/

        public static List<R.GeometryBase> ConvertGroup(BHG.GeometryGroup geom)
        {
            List<R.GeometryBase> rGeom = new List<Rhino.Geometry.GeometryBase>();
            foreach (BHG.IBHoMGeometry item in geom.Elements)
            {
                if (typeof(BHG.GeometryGroup).IsAssignableFrom(item.GetType()))
                {
                    rGeom.AddRange(ConvertGroup((BHG.GeometryGroup)item));
                }
                else
                {
                    rGeom.Add(Convert(item));
                }
            }
            return rGeom;
        }


        /********************************************/
        /**** Extention convert methods          ****/
        /********************************************/


        public static BHG.Vector ToBHoMVector(this R.Vector3d vec)
        {
            return Convert(vec);
        }

        public static List<R.Surface> ExtrudeAlong(R.Curve section, R.Curve centreline, R.Plane sectionPlane)
        {
            R.Vector3d globalUp = R.Vector3d.ZAxis;
            R.Vector3d localX = sectionPlane.XAxis;
            R.Curve[] baseCurves = centreline.DuplicateSegments();
            List<R.Surface> extrustions = new List<R.Surface>();
            if (baseCurves.Length == 0) baseCurves = new R.Curve[] { centreline };
            for (int i = 0; i < baseCurves.Length; i++)
            {
                R.Vector3d v = baseCurves[i].PointAtEnd - baseCurves[i].PointAtStart;
                R.Curve start = section.Duplicate() as R.Curve;
                if (v.IsParallelTo(globalUp) == 0)
                {
                    R.Vector3d direction = sectionPlane.Normal;
                    double angle = R.Vector3d.VectorAngle(v, direction);
                    R.Transform alignPerpendicular = R.Transform.Rotation(-angle, R.Vector3d.CrossProduct(v, R.Vector3d.ZAxis), R.Point3d.Origin);
                    localX.Transform(alignPerpendicular);
                    direction.Transform(alignPerpendicular);
                    double angleAxisAlign = R.Vector3d.VectorAngle(localX, R.Vector3d.CrossProduct(globalUp, v));
                    if (localX * globalUp > 0) angleAxisAlign = -angleAxisAlign;
                    R.Transform axisAlign = R.Transform.Rotation(angleAxisAlign, v, R.Point3d.Origin);
                    R.Transform result = R.Transform.Translation(baseCurves[i].PointAtStart - R.Point3d.Origin) * axisAlign * alignPerpendicular;// * axisAlign *                

                    start.Transform(result);
                }
                else
                {
                    start.Translate(baseCurves[i].PointAtStart - R.Point3d.Origin);
                }
                extrustions.Add(R.Extrusion.CreateExtrusion(start, v));
            }
            return extrustions;

        }
    }
}
