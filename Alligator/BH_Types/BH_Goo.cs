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
using BH.Adapter.Rhinoceros;

namespace BH.UI.Alligator.Base
{
    public class BH_Goo : GH_Goo<IObject>
    {
        #region Constructors
        public BH_Goo()
        {
            this.Value = null;
        }
        public BH_Goo(IObject bh)
        {
            this.Value = bh;
        }
        #endregion

        #region Properties
        public override bool IsValid
        {
            get
            {
                if (Value == null) { return false; }
                return Value != null;
            }
        }
        public override IGH_Goo Duplicate()
        {
            return new BH_Goo();
        }
        public override string ToString()
        {
            if (Value == null)
                return "Null BHoMObject";

            else if (typeof(BH.oM.Base.CustomObject).IsAssignableFrom(Value.GetType()))
            {
                return "BH.oM.Base.Custom." + Value.Name;
            }
            else
                return Value.ToString();
        }
        public override string TypeName
        {
            get { return ("BHoMObject"); }
        }
        public override string TypeDescription
        {
            get { return ("Defines a generic BHoMObject"); }
        }

        public Rhino.Geometry.BoundingBox Boundingbox
        {
            get
            {

                if (Value == null) { return Rhino.Geometry.BoundingBox.Empty; }
                if (Value == null) { return Rhino.Geometry.BoundingBox.Empty; }
                return ((BHoMObject)Value).GetGeometry().IGetBounds().ToRhino();
            }
        }
        public Rhino.Geometry.BoundingBox GetBoundingBox(Rhino.Geometry.Transform xform)
        {
            if (Value == null) { return Rhino.Geometry.BoundingBox.Empty; }
            if (Value == null) { return Rhino.Geometry.BoundingBox.Empty; }
            return ((BHoMObject)Value).GetGeometry().IGetBounds().ToRhino();
        }
        #endregion

        #region Casting Methods
        public override bool CastFrom(object source)
        {
            if (source == null) { return false; }
            if (source.GetType() == typeof(GH_Goo<IObject>))
            {
                this.Value = (IObject)source;
                return true;
            }
            return base.CastFrom(source);
        }
        public override bool CastTo<Q>(ref Q target)
        {
            object ptr = this.Value;
            target = (Q)ptr;
            return true;
        }
        #endregion

        //#region Drawing methods
        //public Rhino.Geometry.BoundingBox ClippingBox
        //{
        //    get { return Boundingbox; }
        //}
        //public void DrawViewportMeshes(GH_PreviewMeshArgs args)
        //{
        //    IBHoMGeometry geometry = ((BHoMObject)Value).GetGeometry();
        //    if (geometry == null) { return; }
        //    if (typeof(BH.oM.Geometry.Mesh).IsAssignableFrom(Value.GetType()))
        //    {
        //        args.Pipeline.DrawMeshWires((Rhino.Geometry.Mesh)(BH.Engine.Grasshopper.GeometryUtils.Convert((BH.oM.Geometry.Mesh)geometry)), args.Material.Diffuse);
        //    }
        //    if (typeof(BH.oM.Geometry.Mesh).IsAssignableFrom(Value.GetType()))
        //    {
        //        args.Pipeline.DrawMeshShaded((Rhino.Geometry.Mesh)(BH.Engine.Grasshopper.GeometryUtils.Convert((BH.oM.Geometry.Mesh)geometry)), args.Material);
        //    }
        //}
        //public void DrawViewportWires(GH_PreviewWireArgs args)
        //{
        //    IBHoMGeometry geometry = ((BHoMObject)Value).GetGeometry();
        //    args.Pipeline.ZBiasMode = 0;
        //    Color BHcolour = new Color();   // To automatically keep the change of color when selected a BHcolour is set as dependent from GH colour
        //    int R = args.Color.R - 59;      // Difference to BuroHappold Green
        //    int G = args.Color.G + 168;     // Difference to BuroHappold Green
        //    int B = args.Color.B;           // Difference to BuroHappold Green
        //    BHcolour = Color.FromArgb(100, R < 255 && R > 0 ? R : 0, G < 255 && G > 0 ? G : 255, B);
        //    if (geometry == null) { return; }
        //    if (typeof(BH.oM.Geometry.Point).IsAssignableFrom(geometry.GetType()))
        //    {
        //        args.Pipeline.DrawPoint((BH.Engine.Grasshopper.GeometryUtils.Convert((BH.oM.Geometry.Point)geometry)), Rhino.Display.PointStyle.ControlPoint, 3, BHcolour);
        //    }
        //    else if (typeof(BH.oM.Geometry.Vector).IsAssignableFrom(geometry.GetType()))
        //    {
        //        //args.Pipeline.DrawLineArrow(BH.Engine.Grasshopper.GeometryUtils.Convert(((BH.oM.Geometry.Vector)geometry)), args.Color, args.Thickness, args.Thickness);
        //    }
        //    else if (typeof(BH.oM.Geometry.Line).IsAssignableFrom(geometry.GetType()))
        //    {
        //        args.Pipeline.DrawLine(BH.Engine.Grasshopper.GeometryUtils.Convert((BH.oM.Geometry.Line)geometry), BHcolour);
        //    }
        //    else if (typeof(BH.oM.Geometry.Circle).IsAssignableFrom(geometry.GetType()))
        //    {
        //        args.Pipeline.DrawCircle(BH.Engine.Grasshopper.GeometryUtils.Convert((BH.oM.Geometry.Circle)geometry), BHcolour);
        //    }
        //    else if (typeof(BH.oM.Geometry.Polyline).IsAssignableFrom(geometry.GetType()))
        //    {
        //        List<BH.oM.Geometry.Point> bhomPoints = ((BH.oM.Geometry.Polyline)geometry).ControlPoints;
        //        IEnumerable<Rhino.Geometry.Point3d> rhinoPoints = bhomPoints.Select(x => BH.Engine.Grasshopper.GeometryUtils.Convert(x));
        //        args.Pipeline.DrawPolyline(rhinoPoints, BHcolour);
        //    }
        //    else if (typeof(BH.oM.Geometry.NurbCurve).IsAssignableFrom(geometry.GetType()))
        //    {
        //    }
        //    else if (typeof(BH.oM.Geometry.Plane).IsAssignableFrom(geometry.GetType()))
        //    {
        //        args.Pipeline.DrawConstructionPlane(new Rhino.DocObjects.ConstructionPlane() { Plane = (BH.Engine.Grasshopper.GeometryUtils.Convert((BH.oM.Geometry.Plane)geometry)) });
        //    }
        //    else if (typeof(BH.oM.Geometry.NurbSurface).IsAssignableFrom(geometry.GetType()))
        //    {
        //    }
        //    else if (typeof(BH.oM.Geometry.Mesh).IsAssignableFrom(geometry.GetType()))
        //    {
        //    }
        //}
        //#endregion
    }
}
