using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using BH.Engine.Base;
using Grasshopper.Kernel.Parameters;

namespace BH.UI.Alligator.Base
{
    public class BH_GeometricGoo : GH_GeometricGoo<IBHoMGeometry>, IGH_PreviewData
    {
        #region Constructors
        public BH_GeometricGoo()
        {
            this.Value = null;
        }
        public BH_GeometricGoo(IBHoMGeometry bh)
        {
            this.Value = bh;
        }
        public override IGH_GeometricGoo DuplicateGeometry()
        {
            return new BH_GeometricGoo { Value = Value.GetClone() };
        }
        #endregion

        #region Properties
        public override bool IsValid
        {
            get
            {
                if (Value == null) { return false; }
                return true;
            }
        }
        public override string ToString()
        {
            if (Value == null)
                return "Null IBHoMGeometry";
            else if (typeof(BH.oM.Geometry.Point).IsAssignableFrom(Value.GetType()))
            {
                return (Value.GetType().ToString() + " {" + ((BH.oM.Geometry.Point)Value).X + ", " +
                                                            ((BH.oM.Geometry.Point)Value).Y + ", " +
                                                            ((BH.oM.Geometry.Point)Value).Z + "}");
            }
            else
                return Value.ToString();
        }
        public override string TypeName
        {
            get { return ("IBHoMGeometry"); }
        }
        public override string TypeDescription
        {
            get { return ("Defines a generic IBHoMGeometry"); }
        }

        public override Rhino.Geometry.BoundingBox Boundingbox
        {
            get
            {
                if (Value == null) { return Rhino.Geometry.BoundingBox.Empty; }
                if (Value == null) { return Rhino.Geometry.BoundingBox.Empty; }
                BH.oM.Geometry.BoundingBox bb = Value.GetBounds(); // TODO Replace with a proper conversion between Rhino and BH BoundingBox
                return new Rhino.Geometry.BoundingBox(bb.Min.X, bb.Min.Y, bb.Min.Z, bb.Max.X, bb.Max.Y, bb.Max.Z);
            }
        }
        public override Rhino.Geometry.BoundingBox GetBoundingBox(Rhino.Geometry.Transform xform)
        {
            if (Value == null) { return Rhino.Geometry.BoundingBox.Empty; }
            if (Value == null) { return Rhino.Geometry.BoundingBox.Empty; }
            BH.oM.Geometry.BoundingBox bb = Value.GetBounds(); // TODO Replace with a proper conversion between Rhino and BH BoundingBox
            return new Rhino.Geometry.BoundingBox(bb.Min.X, bb.Min.Y, bb.Min.Z, bb.Max.X, bb.Max.Y, bb.Max.Z);
        }
        #endregion

        #region Casting Methods
        public override bool CastFrom(object source)
        {
            if (source == null) { return false; }
            if (typeof(IBHoMGeometry).IsAssignableFrom(source.GetType()))
            {
                this.Value = (IBHoMGeometry)source;
                return true;
            }
            if (typeof(Rhino.Geometry.GeometryBase).IsAssignableFrom(source.GetType()))
            {
                this.Value = BH.Engine.Grasshopper.GeometryUtils.Convert((Rhino.Geometry.GeometryBase)source);
                return true;
            }
            if (typeof(GH_Point).IsAssignableFrom(source.GetType()))
            {
                Value = BH.Engine.Grasshopper.GeometryUtils.Convert(((GH_Point)source).Value);
            }
            else if (typeof(GH_Vector).IsAssignableFrom(source.GetType()))
            {
                Value = BH.Engine.Grasshopper.GeometryUtils.Convert(((GH_Vector)source).Value);
            }
            else if (typeof(GH_Line).IsAssignableFrom(source.GetType()))
            {
                Value = BH.Engine.Grasshopper.GeometryUtils.Convert(((GH_Line)source).Value);
            }
            else if (typeof(GH_Circle).IsAssignableFrom(source.GetType()))
            {
                Value = BH.Engine.Grasshopper.GeometryUtils.Convert(((GH_Circle)source).Value);
            }
            else if (typeof(GH_Curve).IsAssignableFrom(source.GetType()))
            {
                Value = BH.Engine.Grasshopper.GeometryUtils.Convert(((GH_Curve)source).Value);
            }
            else if (typeof(GH_Plane).IsAssignableFrom(source.GetType()))
            {
                Value = BH.Engine.Grasshopper.GeometryUtils.Convert(((GH_Plane)source).Value);
            }
            else if (typeof(GH_Surface).IsAssignableFrom(source.GetType()))
            {
                Value = BH.Engine.Grasshopper.GeometryUtils.Convert(((GH_Surface)source).Value);
            }
            else if (typeof(GH_Brep).IsAssignableFrom(source.GetType()))
            {
                Value = BH.Engine.Grasshopper.GeometryUtils.Convert(((GH_Brep)source).Value);
            }
            else if (typeof(GH_Mesh).IsAssignableFrom(source.GetType()))
            {
                Value = BH.Engine.Grasshopper.GeometryUtils.Convert(((GH_Mesh)source).Value);
            }
            return true;
        }
        public override bool CastTo<Q>(ref Q target)
        {
            object ptr = this.Value;
            target = (Q)ptr;
            return true;
        }
        #endregion

        #region Transformation Methods
        public override IGH_GeometricGoo Transform(Rhino.Geometry.Transform xform)
        {
            // This does not make any sense, so I will just do a validity check
            if (Value == null) { return null; }
            return new BH_GeometricGoo();
        }
        public override IGH_GeometricGoo Morph(Rhino.Geometry.SpaceMorph xmorph)
        {
            // This does not make any sense, so I will just do a validity check
            if (Value == null) { return null; }
            return new BH_GeometricGoo();
        }
        #endregion

        #region Drawing methods
        public Rhino.Geometry.BoundingBox ClippingBox
        {
            get { return Boundingbox; }
        }
        public void DrawViewportMeshes(GH_PreviewMeshArgs args)
        {
            if (typeof(BH.oM.Geometry.Mesh).IsAssignableFrom(Value.GetType()))
            {
                args.Pipeline.DrawMeshWires((Rhino.Geometry.Mesh)(BH.Engine.Grasshopper.GeometryUtils.Convert((BH.oM.Geometry.Mesh)Value)), args.Material.Diffuse);
            }
            if (typeof(BH.oM.Geometry.Mesh).IsAssignableFrom(Value.GetType()))
            {
                args.Pipeline.DrawMeshShaded((Rhino.Geometry.Mesh)(BH.Engine.Grasshopper.GeometryUtils.Convert((BH.oM.Geometry.Mesh)Value)), args.Material);
            }
        }
        public void DrawViewportWires(GH_PreviewWireArgs args)
        {
            args.Pipeline.ZBiasMode = 0;
            Color BHcolour = new Color();   // To automatically keep the change of color when selected a BHcolour is set as dependent from GH colour
            int R = args.Color.R - 59;      // Difference to BuroHappold Green
            int G = args.Color.G + 168;     // Difference to BuroHappold Green
            int B = args.Color.B;           // Difference to BuroHappold Green
            BHcolour = Color.FromArgb(100, R < 255 && R > 0 ? R : 0, G < 255 && G > 0 ? G : 255, B);
            if (Value == null) { return; }
            if (typeof(BH.oM.Geometry.Point).IsAssignableFrom(Value.GetType()))
            {
                args.Pipeline.DrawPoint((BH.Engine.Grasshopper.GeometryUtils.Convert((BH.oM.Geometry.Point)Value)), Rhino.Display.PointStyle.Simple, 3, BHcolour);
            }
            else if (typeof(BH.oM.Geometry.Vector).IsAssignableFrom(Value.GetType()))
            {
                //args.Pipeline.DrawLineArrow(BH.Engine.Grasshopper.GeometryUtils.Convert(((BH.oM.Geometry.Vector)Value)), args.Color, args.Thickness, args.Thickness);
            }
            else if (typeof(BH.oM.Geometry.Line).IsAssignableFrom(Value.GetType()))
            {
                args.Pipeline.DrawLine(BH.Engine.Grasshopper.GeometryUtils.Convert((BH.oM.Geometry.Line)Value), BHcolour);
            }
            else if (typeof(BH.oM.Geometry.Circle).IsAssignableFrom(Value.GetType()))
            {
                args.Pipeline.DrawCircle(BH.Engine.Grasshopper.GeometryUtils.Convert((BH.oM.Geometry.Circle)Value), BHcolour);
            }
            else if (typeof(BH.oM.Geometry.Polyline).IsAssignableFrom(Value.GetType()))
            {
                List<BH.oM.Geometry.Point> bhomPoints = ((BH.oM.Geometry.Polyline)Value).ControlPoints;
                IEnumerable<Rhino.Geometry.Point3d> rhinoPoints = bhomPoints.Select(x => BH.Engine.Grasshopper.GeometryUtils.Convert(x));
                args.Pipeline.DrawPolyline(rhinoPoints, BHcolour);
            }
            else if (typeof(BH.oM.Geometry.NurbCurve).IsAssignableFrom(Value.GetType()))
            {
            }
            else if (typeof(BH.oM.Geometry.Plane).IsAssignableFrom(Value.GetType()))
            {
                args.Pipeline.DrawConstructionPlane(new Rhino.DocObjects.ConstructionPlane() { Plane = (BH.Engine.Grasshopper.GeometryUtils.Convert((BH.oM.Geometry.Plane)Value)) });
            }
            else if (typeof(BH.oM.Geometry.NurbSurface).IsAssignableFrom(Value.GetType()))
            {
            }
            else if (typeof(BH.oM.Geometry.Mesh).IsAssignableFrom(Value.GetType()))
            {
            }
        }
        #endregion
    }
}
