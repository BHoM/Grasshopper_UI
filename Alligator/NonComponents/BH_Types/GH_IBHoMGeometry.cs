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
using BH.UI.Alligator.Base;

namespace BH.UI.Alligator
{
    public class GH_IBHoMGeometry : GH_GeometricGoo<IBHoMGeometry>, IGH_PreviewData
    {
        public GH_IBHoMGeometry()
        {
            this.Value = null;
        }
        public GH_IBHoMGeometry(IBHoMGeometry bh)
        {
            this.Value = bh;
        }
        public override IGH_GeometricGoo DuplicateGeometry()
        {
            return new GH_IBHoMGeometry { Value = Value.IGetClone() };
        }

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
                BH.oM.Geometry.BoundingBox bb = Value.IGetBounds();
                return new Rhino.Geometry.BoundingBox(bb.Min.X, bb.Min.Y, bb.Min.Z, bb.Max.X, bb.Max.Y, bb.Max.Z);
            }
        }
        public override Rhino.Geometry.BoundingBox GetBoundingBox(Rhino.Geometry.Transform xform)
        {
            if (Value == null) { return Rhino.Geometry.BoundingBox.Empty; }
            if (Value == null) { return Rhino.Geometry.BoundingBox.Empty; }
            BH.oM.Geometry.BoundingBox bb = Value.IGetBounds();
            return new Rhino.Geometry.BoundingBox(bb.Min.X, bb.Min.Y, bb.Min.Z, bb.Max.X, bb.Max.Y, bb.Max.Z);
        }

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
                this.Value = ((Rhino.Geometry.GeometryBase)source).IToBHoM();
                return true;
            }
            if (typeof(GH_Point).IsAssignableFrom(source.GetType()))
            {
                Value = (((GH_Point)source).Value).ToBHoM();
            }
            else if (typeof(GH_Vector).IsAssignableFrom(source.GetType()))
            {
                Value = (((GH_Vector)source).Value).ToBHoM();
            }
            else if (typeof(GH_Line).IsAssignableFrom(source.GetType()))
            {
                Value = (((GH_Line)source).Value).ToBHoM();
            }
            else if (typeof(GH_Circle).IsAssignableFrom(source.GetType()))
            {
                Value = (((GH_Circle)source).Value).ToBHoM();
            }
            else if (typeof(GH_Curve).IsAssignableFrom(source.GetType()))
            {
                Value = (((GH_Curve)source).Value).ToBHoM();
            }
            else if (typeof(GH_Plane).IsAssignableFrom(source.GetType()))
            {
                Value = (((GH_Plane)source).Value).ToBHoM();
            }
            else if (typeof(GH_Surface).IsAssignableFrom(source.GetType()))
            {
                Value = (((GH_Surface)source).Value).ToBHoM();
            }
            else if (typeof(GH_Brep).IsAssignableFrom(source.GetType()))
            {
                Value = (((GH_Brep)source).Value).ToBHoM();
            }
            else if (typeof(GH_Mesh).IsAssignableFrom(source.GetType()))
            {
                Value = (((GH_Mesh)source).Value).ToBHoM();
            }
            return true;
        }
        public override bool CastTo<Q>(ref Q target)
        {
            object ptr = this.Value;
            target = (Q)ptr;
            return true;
        }

        public override IGH_GeometricGoo Transform(Rhino.Geometry.Transform xform)
        {
            // This does not make any sense, so I will just do a validity check
            if (Value == null) { return null; }
            return new GH_IBHoMGeometry();
        }
        public override IGH_GeometricGoo Morph(Rhino.Geometry.SpaceMorph xmorph)
        {
            // This does not make any sense, so I will just do a validity check
            if (Value == null) { return null; }
            return new GH_IBHoMGeometry();
        }

        public Rhino.Geometry.BoundingBox ClippingBox
        {
            get { return Boundingbox; }
        }
        public void DrawViewportMeshes(GH_PreviewMeshArgs args)
        {
            if (typeof(BH.oM.Geometry.Mesh).IsAssignableFrom(Value.GetType()))
            {
                args.Pipeline.DrawMeshWires(((BH.oM.Geometry.Mesh)Value).ToRhino(), args.Material.Diffuse);
            }
            if (typeof(BH.oM.Geometry.Mesh).IsAssignableFrom(Value.GetType()))
            {
                args.Pipeline.DrawMeshShaded(((BH.oM.Geometry.Mesh)Value).ToRhino(), args.Material);
            }
        }
        public void DrawViewportWires(GH_PreviewWireArgs args)
        {
            if (Value == null) { return; }
            Render.IRenderBHoMGeometry(Value as dynamic, args);
        }

    }
}
