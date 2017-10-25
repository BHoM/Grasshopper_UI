using BH.Adapter.Rhinoceros;
using BHG = BH.oM.Geometry;
using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.UI.Alligator
{
    public static partial class Render
    {
        public static void IRenderBHoMGeometry(this BHG.IBHoMGeometry geometry, GH_PreviewWireArgs args)
        {
            args.Pipeline.ZBiasMode = 0;
            Color bhColour = GetBHColor(args.Color);
            if (geometry != null)
            {
                RenderBHoMGeometry(geometry as dynamic, args.Pipeline, bhColour);
            }
        }

        public static void IRenderBHoMGeometry(this BHG.IBHoMGeometry geometry, IGH_PreviewArgs args)
        {
            args.Display.ZBiasMode = 0;
            Color bhColour = GetBHColor(args.WireColour);

            RenderBHoMGeometry(geometry as dynamic, args.Display, bhColour);
        }

        public static void RenderBHoMGeometry(BHG.Point point, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawPoint(point.ToRhino(), Rhino.Display.PointStyle.Simple, 3, bhColour);
        }
        public static void RenderBHoMGeometry(BHG.Vector vector, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            //args.Pipeline.DrawLineArrow(vector.ToRhino(), args.Color, args.Thickness, args.Thickness);
        }
        public static void RenderBHoMGeometry(BHG.Line line, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawLine(line.ToRhino(), bhColour);
        }
        public static void RenderBHoMGeometry(BHG.Circle circle, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawCircle(circle.ToRhino(), bhColour);
        }
        public static void RenderBHoMGeometry(BHG.Polyline polyline, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            List<BH.oM.Geometry.Point> bhomPoints = polyline.ControlPoints;
            IEnumerable<Rhino.Geometry.Point3d> rhinoPoints = bhomPoints.Select(x => (x).ToRhino());
            pipeline.DrawPolyline(rhinoPoints, bhColour);
        }
        public static void RenderBHoMGeometry(BHG.NurbCurve curve, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
        }
        public static void RenderBHoMGeometry(BHG.Plane plane, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawConstructionPlane(new Rhino.DocObjects.ConstructionPlane() { Plane = (plane.ToRhino()) });
        }
        public static void RenderBHoMGeometry(BHG.NurbSurface surface, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
        }
        public static void RenderBHoMGeometry(BHG.Mesh mesh, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
        }


        private static Color GetBHColor(Color ghColor)
        {
            int R = ghColor.R - 59;      // Difference to BuroHappold Green
            int G = ghColor.G + 168;     // Difference to BuroHappold Green
            int B = ghColor.B;           // Difference to BuroHappold Green
            return Color.FromArgb(100, R < 255 && R > 0 ? R : 0, G < 255 && G > 0 ? G : 255, B);
        }
    }
}
