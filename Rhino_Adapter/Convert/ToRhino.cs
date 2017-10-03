using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RHG = Rhino.Geometry;
using BHG = BH.oM.Geometry;
using Rhino;

namespace BH.Adapter.Rhinoceros
{
    public static partial class Convert
    {
        #region Public Methods
        public static RHG.GeometryBase ToRhino(this BHG.IBHoMGeometry geometry)
        {
            return Convert.ToRhino(geometry as dynamic);
        }
        //public static List<RHG.GeometryBase> ToRhino(this BHG.CompositeGeometry geometries)
        //{
        //    return geometries.Decompose().Select(x => x.ToRhino()); // TODO Waiting for the CompositeGeometry.Decompose() method
        //}

        #region 1D
        public static RHG.Point3d ToRhino(this BHG.Point point)
        {
            return new RHG.Point3d(point.X, point.Y, point.Z);
        }
        public static RHG.Vector3d ToRhino(this BHG.Vector vector)
        {
            return new RHG.Vector3d(vector.X, vector.Y, vector.Z);
        }
        #endregion

        #region 2D
        publci static RHG.Curve ToRhino(this BHG.ICurve curves)
        {

        }
        public static RHG.Arc ToRhino(this BHG.Arc arc)
        {
            return new RHG.Arc(arc.Start.ToRhino(), arc.Middle.ToRhino(), arc.End.ToRhino());
        }
        public static RHG.Circle ToRhino(this BHG.Circle circle)
        {
            return new RHG.Circle(new RHG.Plane(circle.Centre.ToRhino(), circle.Normal.ToRhino()), circle.Radius);
        }
        public static RHG.Line ToRhino(this BHG.Line line)
        {
            return new RHG.Line(line.Start.ToRhino(), line.End.ToRhino());
        }
        public static RHG.NurbsCurve ToRhino(this BHG.NurbCurve nurbCurve)
        {
            // Old Code is not used since the BHoM2.0 implementatation of NurbCurve lacks some fields
            #region Old Code
            //R.NurbsCurve c = new R.NurbsCurve(curve.GetDegree(), curve.ControlPoints.Count);
            //for (int i = 1; i < curve.Knots.Count - 1; i++)
            //{
            //    if (c.Knots.Count < i)
            //    {
            //        c.Knots.InsertKnot(curve.Knots[i]);
            //    }
            //    else
            //    {
            //        c.Knots[i - 1] = curve.Knots[i];
            //    }
            //}
            //int index = 0;
            //foreach (BHG.Point p in curve.ControlPoints)
            //{
            //    c.Points.SetPoint(index, p.X, p.Y, p.Z, curve.Weights[index]);
            //    index++;
            //}
            //return c;
            #endregion
            throw new NotImplementedException();    // TODO Rhino_Adapter conversion to NurbsCurve
        }
        public static RHG.Plane ToRhino(this BHG.Plane plane)
        {
            return new RHG.Plane(plane.Origin.ToRhino(), plane.Normal.ToRhino());
        }
        public static RHG.PolyCurve ToRhino(this BHG.PolyCurve polyCurve)
        {
            //return Rhino.Geometry.Curve.((polyCurve.Curves.Select(x => x.ToRhino())) as IEnumerable<RHG.Curve>));
            throw new NotImplementedException();
        }
        public static RHG.Polyline ToRhino(this BHG.Polyline polyline)
        {
            return new RHG.Polyline(polyline.ControlPoints.Select(x => x.ToRhino()));
        }
        #endregion

        #region 3D
        public static RHG.BoundingBox ToRhino(this BHG.BoundingBox boundingBox)
        {
            return new RHG.BoundingBox(boundingBox.Min.ToRhino(), boundingBox.Max.ToRhino());
        }
        public static RHG.Surface ToRhino(this BHG.NurbSurface surface)
        {
            throw new NotImplementedException();    // TODO Rhino_Adapter conversion to Surface
        }
        public static RHG.Brep ToRhino(this BHG.PolySurface polySurface)
        {
            throw new NotImplementedException();    // TODO Rhino_Adapter conversion to Brep. Problems with the Brep() access level
        }
        public static RHG.Extrusion ToRhino(this BHG.Extrusion extrusion)
        {
            throw new NotImplementedException(); // TODO Rhino_Adapter conversion to Extrusion
        }
        public static RHG.Mesh ToRhino(this BHG.Mesh mesh)
        {
            List<RHG.Point3d> rVertices = mesh.Vertices.Select(x => x.ToRhino()).ToList();
            List<BHG.Face> faces = mesh.Faces;
            List<RHG.MeshFace> rFaces = new List<RHG.MeshFace>();
            for (int i = 0; i < faces.Count; i++)
            {
                if (faces[i].IsQuad())
                {
                    rFaces.Add(new RHG.MeshFace(faces[i].A, faces[i].B, faces[i].C, faces[i].D));
                }
                else
                {
                    rFaces.Add(new RHG.MeshFace(faces[i].A, faces[i].B, faces[i].C));
                }
            }
            RHG.Mesh rMesh = new RHG.Mesh();
            rMesh.Faces.AddFaces(rFaces);
            rMesh.Vertices.AddVertices(rVertices);
            return rMesh;
        }
        #endregion
        #endregion
    }
}
