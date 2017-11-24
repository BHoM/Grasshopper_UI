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
using Rhino;
using Rhino.DocObjects;

namespace BH.UI.Alligator
{
    public class GH_BHoMObject : GH_Goo<IObject>, IGH_PreviewData, IGH_BakeAwareData
    {
        /***************************************************/
        /**** Properties Override                       ****/
        /***************************************************/

        public override string TypeName
        {
            get { return ("BHoMObject"); }
        }

        /***************************************************/

        public override string TypeDescription
        {
            get { return ("Contains a generic BHoMObject"); }
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

        public Rhino.Geometry.BoundingBox Boundingbox
        {
            get
            {
                if (Value == null) { return Rhino.Geometry.BoundingBox.Empty; }
                if (Geometry == null) { return Rhino.Geometry.BoundingBox.Empty; }
                BH.oM.Geometry.BoundingBox bhBox = Geometry.IGetBounds();
                if (bhBox == null) { return Rhino.Geometry.BoundingBox.Empty; }
                return bhBox.ToRhino();
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

        public GH_BHoMObject()
        {
            this.Value = null;
        }

        /***************************************************/

        public GH_BHoMObject(IObject bh)
        {
            this.Value = bh;
        }


        /***************************************************/
        /**** Public Methods Override                   ****/
        /***************************************************/

        public override IGH_Goo Duplicate()
        {
            return new GH_BHoMObject();
        }

        /***************************************************/

        public override string ToString()
        {
            if (Value == null)
                return "null BHoMObject";
            return Value.ToString();
        }

        /***************************************************/

        public Rhino.Geometry.BoundingBox GetBoundingBox(Rhino.Geometry.Transform xform)
        {
            if (Value == null) { return Rhino.Geometry.BoundingBox.Empty; }
            if (Geometry == null) { return Rhino.Geometry.BoundingBox.Empty; }
            BH.oM.Geometry.BoundingBox bhBox = Geometry.IGetBounds();
            if (bhBox == null) { return Rhino.Geometry.BoundingBox.Empty; }
            return bhBox.ToRhino();
        }


        /***************************************************/
        /**** Automatic casting methods                 ****/
        /***************************************************/

        public override bool CastFrom(object source)
        {
            if (source == null) { return false; }
            if (source.GetType() == typeof(GH_Goo<IObject>))
            {
                this.Value = (IObject)source;
                return true;
            }
            else
            {
                this.Value = (BHoMObject)source;
                return true;
            }
        }

        /***************************************************/

        public override bool CastTo<Q>(ref Q target)
        {
            object ptr = this.Value;
            target = (Q)ptr;
            return true;
        }


        /***************************************************/
        /**** IGH_PreviewData methods                   ****/
        /***************************************************/

        public void DrawViewportMeshes(GH_PreviewMeshArgs args)
        {
            if (Geometry == null) { return; }
            if (typeof(BH.oM.Geometry.Mesh).IsAssignableFrom(Value.GetType()))
                Render.RenderBHoMGeometry((BH.oM.Geometry.Mesh)Value, args);
        }

        /***************************************************/

        public void DrawViewportWires(GH_PreviewWireArgs args)
        {
            if (Value == null) { return; }
            Render.IRenderBHoMObject(Value, args);
        }


        /***************************************************/
        /**** IGH_BakeAwareData methods                 ****/
        /***************************************************/

        public bool BakeGeometry(RhinoDoc doc, ObjectAttributes att, out Guid obj_guid)
        {
            obj_guid = doc.Objects.Add(Geometry.IToRhino(), att);
            return true;
        }

        /***************************************************/
        /**** Private Properties                        ****/
        /***************************************************/

        private IBHoMGeometry Geometry
        {
            get
            {
                if (Value == null) { return null; }
                return ((BHoMObject)Value).IGetGeometry();
            }
        }
    }
}
