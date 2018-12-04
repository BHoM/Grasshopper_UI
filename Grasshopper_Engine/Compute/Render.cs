using BH.Engine.Rhinoceros;
using BHG = BH.oM.Geometry;
using RHG = Rhino.Geometry;
using Grasshopper.Kernel;
using System;
using System.Drawing;
using System.Linq;
using BH.Engine.Base;

namespace BH.Engine.Alligator
{
    public static partial class Compute
    {
        /***************************************************/
        /**** Public Methods  - Interfaces              ****/
        /***************************************************/

        public static bool IRender(this BH.oM.Base.IBHoMObject bhObject, GH_PreviewWireArgs args)
        {
            if (bhObject == null) { return false; }
            args.Pipeline.ZBiasMode = 0;
            Color bhColour = Query.RenderColour(args.Color);
            try
            {
                return Render(bhObject as dynamic, args, bhColour);
            }
            catch (Exception) { }
            return false;
        }

        /***************************************************/

        public static void IRender(this BHG.IGeometry geometry, GH_PreviewWireArgs args)
        {
            if (geometry == null) { return; }
            args.Pipeline.ZBiasMode = 0;
            Color bhColour = Query.RenderColour(args.Color);
            try
            {
                Render(geometry as dynamic, args.Pipeline, bhColour);
            }
            catch (Exception) { }
        }

        /***************************************************/

        public static void IRender(this BHG.IGeometry geometry, IGH_PreviewArgs args)
        {
            if (geometry == null) { return; }
            args.Display.ZBiasMode = 0;
            Color bhColour = Query.RenderColour(args.WireColour);
            try
            {
                Render(geometry as dynamic, args.Display, bhColour);
            }
            catch (Exception) { }
        }


        /***************************************************/
        /**** Public Methods  - Objects                 ****/
        /***************************************************/

        public static bool Render(BH.oM.Base.BHoMObject obj, GH_PreviewWireArgs args, Color bhColour)
        {
            IRender(obj.IGeometry(), args);
            return true;
        }


        /***************************************************/
        /**** Public Methods  - Vectors                 ****/
        /***************************************************/

        public static void Render(BHG.Point point, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawPoint(point.ToRhino(), Rhino.Display.PointStyle.Simple, 4, bhColour);
        }

        /***************************************************/

        public static void Render(BHG.Vector vector, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            //args.Pipeline.DrawLineArrow(vector.ToRhino(), args.Color, args.Thickness, args.Thickness);
        }

        /***************************************************/

        public static void Render(BHG.Plane plane, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawConstructionPlane(new Rhino.DocObjects.ConstructionPlane() { Plane = (plane.ToRhino()), ThickLineColor = bhColour, ThinLineColor = Color.Black, GridLineCount = 10 });
        }

        /***************************************************/

        public static void Render(BHG.CoordinateSystem coordinateSystem, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawConstructionPlane(new Rhino.DocObjects.ConstructionPlane() { Plane = (coordinateSystem.ToRhino()), ThickLineColor = bhColour, ThinLineColor = Color.Black, GridLineCount = 10 });
        }


        /***************************************************/
        /**** Public Methods  - Curves                  ****/
        /***************************************************/

        public static void Render(BHG.Arc arc, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawArc(arc.ToRhino(), bhColour, 2);
        }

        /***************************************************/

        public static void Render(BHG.Circle circle, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawCircle(circle.ToRhino(), bhColour, 2);
        }

        /***************************************************/

        public static void Render(BHG.Ellipse ellipse, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawCurve(ellipse.ToRhino().ToNurbsCurve(), bhColour, 2);
        }

        /***************************************************/

        public static void Render(BHG.Line line, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawLine(line.ToRhino(), bhColour, 2);
        }
        /***************************************************/

        public static void Render(BHG.NurbCurve curve, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawCurve(curve.ToRhino(), bhColour, 2);
        }

        /***************************************************/

        public static void Render(BHG.PolyCurve polycurve, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawCurve(polycurve.ToRhino(), bhColour, 2);
        }

        /***************************************************/

        public static void Render(BHG.Polyline polyline, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawPolyline(polyline.ControlPoints.Select(x => x.ToRhino()), bhColour, 2);
        }


        /***************************************************/
        /**** Public Methods  - Surfaces                ****/
        /***************************************************/

        public static void Render(BHG.Extrusion surface, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawSurface(surface.ToRhino(), bhColour, 1);
        }

        /***************************************************/

        public static void Render(BHG.Loft surface, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawSurface(surface.ToRhino(), bhColour, 1);
        }

        /***************************************************/

        public static void Render(BHG.NurbSurface surface, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawSurface(surface.ToRhino(), bhColour, 1);
        }

        /***************************************************/

        public static void Render(BHG.Pipe surface, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawBrepShaded(surface.ToRhino(), new Rhino.Display.DisplayMaterial(bhColour));
        }

        /***************************************************/

        public static void Render(BHG.PolySurface polySurface, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            polySurface.Surfaces.ForEach(srf => pipeline.DrawSurface(srf.IToRhino(), bhColour, 1));
        }


        /***************************************************/
        /**** Public Methods  - Mesh                    ****/
        /***************************************************/

        public static void Render(BHG.Mesh mesh, GH_PreviewMeshArgs args)
        {
            RHG.Mesh rMesh = mesh.ToRhino();
            args.Pipeline.DrawMeshWires(rMesh, args.Material.Diffuse);
            args.Pipeline.DrawMeshShaded(rMesh, args.Material);
        }

        /***************************************************/

        public static void Render(BHG.Mesh mesh, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            RHG.Mesh rMesh = mesh.ToRhino();
            pipeline.DrawMeshWires(rMesh, bhColour);
            pipeline.DrawMeshShaded(rMesh, new Rhino.Display.DisplayMaterial(bhColour));
        }

        /***************************************************/
        /**** Public Methods  - Miscellanea             ****/
        /***************************************************/

        public static void Render(BHG.BoundingBox bbBox, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawBox(bbBox.ToRhino(), bhColour, 2);
        }

        /***************************************************/

        public static void Render(BHG.CompositeGeometry composite, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            if (composite.Elements.Count == 0) { return; }
            foreach (BHG.IGeometry geom in composite.Elements)
            {
                try
                {
                    Render(geom as dynamic, pipeline, bhColour);
                }
                catch (Exception) { }
            }
        }

        /***************************************************/
    }
}
