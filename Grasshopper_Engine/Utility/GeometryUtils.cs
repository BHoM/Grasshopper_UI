using BHoM.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH = BHoM.Geometry;
using R = Rhino.Geometry;

namespace Grasshopper_Engine
{
    public static class GeometryUtils
    {
        /**********************************************/
        /****  Types                               ****/
        /**********************************************/

        public static Type GetRhinoType(Type bhType)
        {
            if (bhType == typeof(BH.Curve))
            {
                return typeof(R.Curve);
            }
            else if (bhType == typeof(BH.Point))
            {
                return typeof(R.Point);
            }
            else if (bhType == typeof(BH.Surface))
            {
                return typeof(R.Surface);
            }
            else if (bhType == typeof(BH.Plane))
            {
                return typeof(R.Plane);
            }
            else if (bhType == typeof(BH.Mesh))
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
                return typeof(BH.Curve);
            }
            else if (rhinoType == typeof(R.Point))
            {
                return typeof(BH.Point);
            }
            else if (rhinoType == typeof(R.Surface))
            {
                return typeof(BH.Surface);
            }
            else if (rhinoType == typeof(R.Brep))
            {
                return typeof(BH.Brep);
            }
            else if (rhinoType == typeof(R.Plane))
            {
                return typeof(BH.Plane);
            }
            else if (rhinoType == typeof(R.Mesh))
            {
                return typeof(BH.Mesh);
            }
            return null;
        }


        /**********************************************/
        /****  Points & Vectors                    ****/
        /**********************************************/

        public static R.Point3d Convert(BH.Point p)
        {
            return new R.Point3d(p.X, p.Y, p.Z);
        }

        /**********************************************/

        public static BH.Point Convert(R.Point3d pnt)
        {
            R.Point3d p = pnt;
            return new BH.Point(p.X, p.Y, p.Z);
        }

        /**********************************************/

        public static R.Vector3d Convert(BH.Vector p)
        {
            return new R.Vector3d(p.X, p.Y, p.Z);
        }

        /**********************************************/

        public static BH.Vector Convert(R.Vector3d v)
        {
            return new BH.Vector(v.X, v.Y, v.Z);
        }

        /**********************************************/

        public static R.Plane Convert(BH.Plane p)
        {
            return new R.Plane(Convert(p.Origin), Convert(p.Normal));
        }

        /**********************************************/

        public static BH.Plane Convert(R.Plane plane)
        {
            return new BH.Plane(Convert(plane.Origin), Convert(plane.Normal));
        }


        /**********************************************/
        /****  Curves                              ****/
        /**********************************************/

        public static BH.Line Convert(R.Line line)
        {
            return new BH.Line(Convert(line.From), Convert(line.To));
        }

        /**********************************************/

        public static R.Line Convert(BH.Line line)
        {
            return new R.Line(Convert(line.StartPoint), Convert(line.EndPoint));
        }

        /**********************************************/

        public static BH.Circle Convert(R.Circle circle)
        {
            return new BH.Circle(circle.Radius, Convert(circle.Plane));
        }

        /**********************************************/

        public static R.Circle Convert(BH.Circle circle)
        {
            return new R.Circle(Convert(circle.Plane), circle.Radius);
        }

        /**********************************************/

        public static BH.Curve Convert(R.Curve rCurve)
        {
            if (rCurve is R.ArcCurve && (rCurve as R.ArcCurve).AngleRadians < Math.PI * 2 / 3)
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
                double[] knots = new double[nurbCurve.Knots.Count + 2];
                List<BH.Point> points = new List<BHoM.Geometry.Point>();
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
                return BH.NurbCurve.Create(points, degree, knots, weight);
            }
        }

        /**********************************************/

        public static R.Curve Convert(BH.Curve curve)
        {
            R.NurbsCurve c = new R.NurbsCurve(curve.Degree, curve.PointCount);// R.NurbsCurve.c.Create(false, curve.Degree, ConvertPointCollection(curve.ControlPoints));
            for (int i = 1; i < curve.Knots.Length - 1; i++)
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

        /**********************************************/
        /****  Surfaces                            ****/
        /**********************************************/

        public static BH.Surface Convert(R.Surface surface)
        {
            return null; //TODO: Eddy to provide a proper constructor for Surface
        }
                
        /**********************************************/

        public static R.Surface Convert(BH.Surface surface)
        {
            return null; //TODO: Set up the proper convertion
        }

        /**********************************************/

        public static R.Brep Convert(BH.Brep brep)
        {
            if (brep is BH.Extrusion)
            {
                return Convert(brep as BH.Extrusion);
            }
            else if (brep is BH.Pipe)
            {
                return Convert(brep as BH.Pipe);
            }

            return null;
        }

        /**********************************************/

        public static R.Brep Convert(BH.Extrusion extrusion)
        {
            R.Surface result = R.Extrusion.CreateExtrusion(Convert(extrusion.Curve), Convert(extrusion.Direction));
            return extrusion.Capped ? result.ToBrep().CapPlanarHoles(0.001) : result.ToBrep();
        }

        /**********************************************/

        public static R.Brep Convert(BH.Pipe pipe)
        {
            R.Brep[] result = R.Brep.CreatePipe(Convert(pipe.Centreline), pipe.Radius, true, pipe.Capped ? R.PipeCapMode.Flat : R.PipeCapMode.None, true, 0.001, 0.001);
            if (result.Length > 0)
            {
                return result[0];
            }
            return null;
        }

        /**********************************************/

        public static BH.Brep Convert(R.Brep brep)
        {
            return null; // TODO: Create the proper convertion (Eddy?)
        }

        /**********************************************/
        /****               Meshes                 ****/
        /**********************************************/

        public static BH.Mesh Convert(R.Mesh rMesh)
        {
            List<R.Point3f> rVertices = rMesh.Vertices.ToList();
            BH.Group<BH.Point> Vertices = new BH.Group<BH.Point>();             //BH Vertices Group
            for (int i = 0; i < rVertices.Count; i++) { BH.Point Vertex = Convert(rVertices[i]); Vertices.Add(Vertex); }
            List<R.MeshFace> rFaces = rMesh.Faces.ToList();
            List<BH.Face> Faces = new List<BH.Face>();                  //BH Faces list
            for (int i = 0; i < rFaces.Count; i++)
            {
                if (rFaces[i].IsQuad) { BH.Face Face = new BH.Face(rFaces[i].A, rFaces[i].B, rFaces[i].C, rFaces[i].D); Faces.Add(Face); }
                if ((rFaces[i].IsTriangle)) { BH.Face Face = new BH.Face(rFaces[i].A, rFaces[i].B, rFaces[i].C); Faces.Add(Face); }                
            }
            return new BH.Mesh(Vertices,Faces);    
                    
        }

        /**********************************************/

            public static R.Mesh Convert(BH.Mesh mesh)
        {
            List<BH.Point> vertices = mesh.Vertices.ToList();
            List<R.Point3d> rVertices = new List<R.Point3d>();      //Rhino Vertices list
            for (int i = 0; i < vertices.Count; i++)
            {
                R.Point3d rVertex = Convert(vertices[i]);
                rVertices.Add(rVertex);
            }
            List<BH.Face> faces = mesh.Faces.ToList();
            List<R.MeshFace> rFaces = new List<R.MeshFace>();       // Rhino MeshFaces list
            for (int i =0; i< faces.Count; i++)
            {
                if (faces[i].IsQuad) { R.MeshFace rFace = new R.MeshFace(faces[i].A, faces[i].B, faces[i].C, faces[i].D); rFaces.Add(rFace); }
                if (faces[i].IsTriangle) { R.MeshFace rFace = new R.MeshFace(faces[i].A, faces[i].B, faces[i].C); rFaces.Add(rFace); }
            }

            R.Mesh rMesh = new R.Mesh();
            rMesh.Faces.AddFaces(rFaces);
            rMesh.Vertices.AddVertices(rVertices);
            return rMesh;
        }

        /**********************************************/


        /**********************************************/
        /****  Geoemtry bases                      ****/
        /**********************************************/

        public static R.GeometryBase Convert(BH.GeometryBase geom)
        {
            if (geom is BH.Curve)
            {
                return Convert(geom as BH.Curve);
            }
            else if (typeof(BH.Brep).IsAssignableFrom(geom.GetType()))
            {
                return Convert(geom as BH.Brep);
            }
            else if (geom is BH.Point)
            {
                return new R.Point(Convert(geom as BH.Point));
            }
            else if (geom is BH.Mesh)
            {
                return Convert(geom as BH.Mesh);
            }
            
            return null;
        }

        /**********************************************/

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


        /**********************************************/
        /****  Collections                         ****/
        /**********************************************/

        private static IEnumerable<R.Point3d> ConvertPointCollection(IEnumerable<BH.Point> pnts)
        {
            List<R.Point3d> result = new List<Rhino.Geometry.Point3d>();
            foreach (BH.Point p in pnts)
            {
                result.Add(Convert(p));
            }
            return result;
        }

        /**********************************************/

        private static IEnumerable<BH.Point> ConvertPointCollection(IEnumerable<R.Point3d> pnts)
        {
            List<BH.Point> result = new List<BH.Point>();
            foreach (R.Point3d p in pnts)
            {
                result.Add(Convert(p));
            }
            return result;
        }

        /**********************************************/

        public static BH.Group<TBH> ConvertList<TBH, TR>(List<TR> geom) where TBH : BH.GeometryBase where TR : R.GeometryBase
        {
            BH.Group<TBH> group = new BHoM.Geometry.Group<TBH>();
            for (int i = 0; i < geom.Count; i++)
            {
                group.Add(Convert(geom[i]) as TBH);
            }

            return group;
        }

        /**********************************************/

        public static List<R.GeometryBase> ConvertGroup<T>(BH.Group<T> geom) where T : BH.GeometryBase
        {
            List<R.GeometryBase> rGeom = new List<Rhino.Geometry.GeometryBase>();
            foreach (T item in geom)
            {
                if (typeof(BH.IGroup).IsAssignableFrom(item.GetType()))
                {
                    rGeom.AddRange(ConvertGroup<BH.GeometryBase>(((BH.IGroup)item).Cast<BH.GeometryBase>()));
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


        public static BH.Vector ToBHoMVector(this R.Vector3d vec)
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
