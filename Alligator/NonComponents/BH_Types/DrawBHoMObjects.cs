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
            Color bhColour = new Color();   // To automatically keep the change of color when selected a BHcolour is set as dependent from GH colour
            int R = args.Color.R - 59;      // Difference to BuroHappold Green
            int G = args.Color.G + 168;     // Difference to BuroHappold Green
            int B = args.Color.B;           // Difference to BuroHappold Green
            bhColour = Color.FromArgb(100, R < 255 && R > 0 ? R : 0, G < 255 && G > 0 ? G : 255, B);

            RenderBHoMGeometry(geometry as dynamic, args, bhColour);
        }
        public static void RenderBHoMGeometry(BHG.Point point, GH_PreviewWireArgs args, Color bhColour)
        {
            args.Pipeline.DrawPoint(point.ToRhino(), Rhino.Display.PointStyle.Simple, 3, bhColour);
        }
        public static void RenderBHoMGeometry(BHG.Vector vector, GH_PreviewWireArgs args, Color bhColour)
        {
            //args.Pipeline.DrawLineArrow(vector.ToRhino(), args.Color, args.Thickness, args.Thickness);
        }
        public static void RenderBHoMGeometry(BHG.Line line, GH_PreviewWireArgs args, Color bhColour)
        {
            args.Pipeline.DrawLine(line.ToRhino(), bhColour);
        }
        public static void RenderBHoMGeometry(BHG.Circle circle, GH_PreviewWireArgs args, Color bhColour)
        {
            args.Pipeline.DrawCircle(circle.ToRhino(), bhColour);
        }
        public static void RenderBHoMGeometry(BHG.Polyline polyline, GH_PreviewWireArgs args, Color bhColour)
        {
            List<BH.oM.Geometry.Point> bhomPoints = polyline.ControlPoints;
            IEnumerable<Rhino.Geometry.Point3d> rhinoPoints = bhomPoints.Select(x => (x).ToRhino());
            args.Pipeline.DrawPolyline(rhinoPoints, bhColour);
        }
        public static void RenderBHoMGeometry(BHG.NurbCurve curve, GH_PreviewWireArgs args, Color bhColour)
        {
        }
        public static void RenderBHoMGeometry(BHG.Plane plane, GH_PreviewWireArgs args, Color bhColour)
        {
            args.Pipeline.DrawConstructionPlane(new Rhino.DocObjects.ConstructionPlane() { Plane = (plane.ToRhino()) });
        }
        public static void RenderBHoMGeometry(BHG.NurbSurface surface, GH_PreviewWireArgs args, Color bhColour)
        {
        }
        public static void RenderBHoMGeometry(BHG.Mesh mesh, GH_PreviewWireArgs args, Color bhColour)
        {
        }
    }
}
