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
    public class GH_BHoMObject : GH_TemplateType<object>, IGH_PreviewData, IGH_BakeAwareData
    {
        public GH_BHoMObject() : base() { }

        /***************************************************/

        public GH_BHoMObject(object val) : base(val) { }

        /***************************************************/

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
        /**** Public Methods Override                   ****/
        /***************************************************/

        public override IGH_Goo Duplicate()
        {
            return new GH_BHoMObject { Value = Value };
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
        /**** IGH_PreviewData methods                   ****/
        /***************************************************/

        public void DrawViewportMeshes(GH_PreviewMeshArgs args)
        {
            if (Geometry == null) { return; }
            /*if (typeof(BH.oM.Geometry.Mesh).IsAssignableFrom(Value.GetType()))
                Render.RenderBHoMGeometry((Mesh)Value, args);*/
        }

        /***************************************************/

        public void DrawViewportWires(GH_PreviewWireArgs args)
        {
            if (Value is BHoMObject) 
                Render.IRenderBHoMObject(Value as BHoMObject, args);
        }


        /***************************************************/
        /**** IGH_BakeAwareData methods                 ****/
        /***************************************************/

        public bool BakeGeometry(RhinoDoc doc, ObjectAttributes att, out Guid obj_guid)
        {
            obj_guid = doc.Objects.Add(Geometry.IToRhino() as Rhino.Geometry.GeometryBase, att); // TODO: Check what happend when geometry is not GeometryBase
            return true;
        }

        /***************************************************/
        /**** Private Properties                        ****/
        /***************************************************/

        private IBHoMGeometry Geometry
        {
            get
            {
                if (Value is BHoMObject)
                    return ((BHoMObject)Value).IGetGeometry();
                else if (Value is IBHoMGeometry)
                    return Value as IBHoMGeometry;
                else
                    return null;
            }
        }
    }
}
