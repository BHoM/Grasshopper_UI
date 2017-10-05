using BH.Adapter.Rhinoceros;
using BH.oM.Base;
using BH.oM.Geometry;
using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.UI.Alligator.Base
{
    public static partial class Render
    {
        public static void IDrawBHoMGeometry(this IBHoMGeometry geometry, GH_PreviewWireArgs args)
        {
            args.Pipeline.ZBiasMode = 0;
            Color bhColour = new Color();   // To automatically keep the change of color when selected a BHcolour is set as dependent from GH colour
            int R = args.Color.R - 59;      // Difference to BuroHappold Green
            int G = args.Color.G + 168;     // Difference to BuroHappold Green
            int B = args.Color.B;           // Difference to BuroHappold Green
            bhColour = Color.FromArgb(100, R < 255 && R > 0 ? R : 0, G < 255 && G > 0 ? G : 255, B);

            DrawBHoMGeometry(geometry as dynamic, args, bhColour);
        }
        public static void DrawBHoMGeometry(BH.oM.Geometry.Point point, GH_PreviewWireArgs args, Color bhColour)
        {
            args.Pipeline.DrawPoint(point.ToRhino(), Rhino.Display.PointStyle.Simple, 3, bhColour);
        }
        //if (typeof(BH.oM.Geometry.Point).IsAssignableFrom(Value.GetType()))
        //{
        //    args.Pipeline.DrawPoint(((BH.oM.Geometry.Point)Value).ToRhino(), Rhino.Display.PointStyle.Simple, 3, bhColour);
        //}
        //else if (typeof(BH.oM.Geometry.Vector).IsAssignableFrom(Value.GetType()))
        //{
        //    //args.Pipeline.DrawLineArrow((((BH.oM.Geometry.Vector)Value)), args.Color, args.Thickness, args.Thickness);
        //}
        //else if (typeof(BH.oM.Geometry.Line).IsAssignableFrom(Value.GetType()))
        //{
        //    args.Pipeline.DrawLine(((BH.oM.Geometry.Line)Value).ToRhino(), BHcolour);
        //}
        //else if (typeof(BH.oM.Geometry.Circle).IsAssignableFrom(Value.GetType()))
        //{
        //    args.Pipeline.DrawCircle(((BH.oM.Geometry.Circle)Value).ToRhino(), BHcolour);
        //}
        //else if (typeof(BH.oM.Geometry.Polyline).IsAssignableFrom(Value.GetType()))
        //{
        //    List<BH.oM.Geometry.Point> bhomPoints = ((BH.oM.Geometry.Polyline)Value).ControlPoints;
        //    IEnumerable<Rhino.Geometry.Point3d> rhinoPoints = bhomPoints.Select(x => (x).ToRhino());
        //    args.Pipeline.DrawPolyline(rhinoPoints, BHcolour);
        //}
        //else if (typeof(BH.oM.Geometry.NurbCurve).IsAssignableFrom(Value.GetType()))
        //{
        //}
        //else if (typeof(BH.oM.Geometry.Plane).IsAssignableFrom(Value.GetType()))
        //{
        //    args.Pipeline.DrawConstructionPlane(new Rhino.DocObjects.ConstructionPlane() { Plane = (((BH.oM.Geometry.Plane)Value).ToRhino()) });
        //}
        //else if (typeof(BH.oM.Geometry.NurbSurface).IsAssignableFrom(Value.GetType()))
        //{
        //}
        //else if (typeof(BH.oM.Geometry.Mesh).IsAssignableFrom(Value.GetType()))
        //{
        //}
    }
}
