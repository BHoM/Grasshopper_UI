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
using Rhino;
using Rhino.DocObjects;

namespace BH.UI.Alligator
{
    public class GH_IBHoMGeometry : GH_GeometricGoo<IBHoMGeometry>, IGH_PreviewData, IGH_BakeAwareData
    {
        /***************************************************/
        /**** Properties Override                       ****/
        /***************************************************/

        public override string TypeName
        {
            get { return ("IBHoMGeometry"); }
        }

        /***************************************************/

        public override string TypeDescription
        {
            get { return ("Contains a generic IBHoMGeometry"); }
        }

        /***************************************************/

        public override bool IsValid
        {
            get
            {
                if (Value == null) { return false; }
                return true;
            }
        }

        /***************************************************/

        public override Rhino.Geometry.BoundingBox Boundingbox
        {
            get
            {
                if (Value == null) { return Rhino.Geometry.BoundingBox.Empty; }
                return Value.IGetBounds().ToRhino();
            }
        }

        /***************************************************/

        public Rhino.Geometry.BoundingBox ClippingBox
        {
            get { return Boundingbox; }
        }


        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public GH_IBHoMGeometry()
        {
            this.Value = null;
        }

        /***************************************************/

        public GH_IBHoMGeometry(IBHoMGeometry bh)
        {
            this.Value = bh;
        }


        /***************************************************/
        /**** Public Methods Override                   ****/
        /***************************************************/

        public override IGH_GeometricGoo DuplicateGeometry()
        {
            return new GH_IBHoMGeometry { Value = Value.IGetClone() };
        }

        /***************************************************/

        public override string ToString()
        {
            string type = Value.GetType().ToString();
            if (Value == null) { return "null IBHoMGeometry"; }
            else if (typeof(BH.oM.Geometry.Point).IsAssignableFrom(Value.GetType()))
            {
                BH.oM.Geometry.Point pt = (BH.oM.Geometry.Point)Value;
                return (Value.GetType().ToString() + " {" + pt.X + ", " + pt.Y + ", " + pt.Z + "}");
            }
            else if (typeof(BH.oM.Geometry.Vector).IsAssignableFrom(Value.GetType()))
            {
                BH.oM.Geometry.Vector vec = (BH.oM.Geometry.Vector)Value;
                return (Value.GetType().ToString() + " {" + vec.X + ", " + vec.Y + ", " + vec.Z + "}");
            }
            else
                return Value.ToString();
        }

        /***************************************************/

        public override Rhino.Geometry.BoundingBox GetBoundingBox(Rhino.Geometry.Transform xform)
        {
            if (Value == null) { return Rhino.Geometry.BoundingBox.Empty; }
            return Value.IGetBounds().ToRhino();
        }


        /***************************************************/
        /**** Automatic casting methods                 ****/
        /***************************************************/

        public override bool CastFrom(object source)
        {
            if (source == null) { return false; }

            else if (source is IBHoMGeometry)
                this.Value = (IBHoMGeometry)source;
            else if (source is Rhino.Geometry.GeometryBase)
                this.Value = ((Rhino.Geometry.GeometryBase)source).IToBHoM();
            else if (source is GH_Vector)                   //TODO: Check if there are other exceptions that do not convert to GeometryBase
                Value = ((GH_Vector)source).Value.ToBHoM();
            else if (source is GH_Box)
                Value = (((GH_Box)source).Value).ToBHoM();
            else if (source is GH_Plane)
                Value = (((GH_Plane)source).Value).ToBHoM();
            else if (source is GH_Circle)
                Value = (((GH_Circle)source).Value).ToBHoM();
            else if (source is IGH_GeometricGoo)
                Value = GH_Convert.ToGeometryBase(source).IToBHoM();

            return true;
        }

        /***************************************************/

        public override bool CastTo<Q>(ref Q target)
        {
            object ptr = this.Value;
            target = (Q)ptr;
            return true;
        }


        /***************************************************/
        /**** Transformation methods                    ****/
        /***************************************************/

        public override IGH_GeometricGoo Transform(Rhino.Geometry.Transform xform)
        {
            if (Value == null) { return null; }
            return this;
        }

        /***************************************************/

        public override IGH_GeometricGoo Morph(Rhino.Geometry.SpaceMorph xmorph)
        {
            if (Value == null) { return null; }
            return this;
        }


        /***************************************************/
        /**** IGH_PreviewData methods                   ****/
        /***************************************************/

        public void DrawViewportMeshes(GH_PreviewMeshArgs args)
        {
            if (Value == null) { return; }
            if (typeof(BH.oM.Geometry.Mesh).IsAssignableFrom(Value.GetType()))
                Render.RenderBHoMGeometry((BH.oM.Geometry.Mesh)Value, args);
        }

        /***************************************************/

        public void DrawViewportWires(GH_PreviewWireArgs args)
        {
            if (Value == null) { return; }
            Render.IRenderBHoMGeometry(Value as dynamic, args);
        }


        /***************************************************/
        /**** IGH_BakeAwareData methods                 ****/
        /***************************************************/

        public bool BakeGeometry(RhinoDoc doc, ObjectAttributes att, out Guid obj_guid)
        {
            obj_guid = doc.Objects.Add(Value.IToRhino(), att);
            return true;
        }
    }
}
