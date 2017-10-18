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
using BH.Adapter.Rhinoceros;

namespace BH.UI.Alligator
{
    public class GH_BHoMObject : GH_Goo<IObject>, IGH_PreviewData
    {
        #region Constructors
        public GH_BHoMObject()
        {
            this.Value = null;
        }
        public GH_BHoMObject(IObject bh)
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
            return new GH_BHoMObject();
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

        #region Drawing methods
        public Rhino.Geometry.BoundingBox ClippingBox
        {
            get { return Boundingbox; }
        }
        public void DrawViewportMeshes(GH_PreviewMeshArgs args)
        {
            IBHoMGeometry geometry = ((BHoMObject)Value).GetGeometry();
            if (geometry == null) { return; }
            if (typeof(BH.oM.Geometry.Mesh).IsAssignableFrom(Value.GetType()))
            {
                args.Pipeline.DrawMeshWires((((BH.oM.Geometry.Mesh)geometry).ToRhino()), args.Material.Diffuse);
            }
            if (typeof(BH.oM.Geometry.Mesh).IsAssignableFrom(Value.GetType()))
            {
                args.Pipeline.DrawMeshShaded((((BH.oM.Geometry.Mesh)geometry).ToRhino()), args.Material);
            }
        }
        public void DrawViewportWires(GH_PreviewWireArgs args)
        {
            if (Value == null) { return; }
            IBHoMGeometry geometry = ((BHoMObject)Value).GetGeometry();
            Render.IRenderBHoMGeometry(geometry, args);
        }
        #endregion
    }
}
