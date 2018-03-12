using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using System;
using BH.Engine.Base;
using BH.Engine.Rhinoceros;
using Rhino;
using Rhino.DocObjects;

namespace BH.UI.Alligator
{
    public class GH_BHoMObject : GH_IObject
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override string TypeName { get; } = "BHoMObject";

        public override string TypeDescription { get; } = "Contains a generic BHoMObject"; 

        public override Rhino.Geometry.BoundingBox ClippingBox { get { return Boundingbox; } }

        public override Rhino.Geometry.BoundingBox Boundingbox
        {
            get
            {
                if (Value == null) { return Rhino.Geometry.BoundingBox.Empty; }
                if (Geometry() == null) { return Rhino.Geometry.BoundingBox.Empty; }
                BH.oM.Geometry.BoundingBox bhBox = Geometry().IBounds();
                if (bhBox == null) { return Rhino.Geometry.BoundingBox.Empty; }
                return bhBox.ToRhino();
            }
        }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public GH_BHoMObject() : base() { }

        /***************************************************/

        public GH_BHoMObject(object val) : base(val) { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override IGH_Goo Duplicate()
        {
            return new GH_BHoMObject { Value = Value };
        }

        /***************************************************/

        public override Rhino.Geometry.BoundingBox GetBoundingBox(Rhino.Geometry.Transform xform)
        {
            if (Value == null) { return Rhino.Geometry.BoundingBox.Empty; }
            if (Geometry() == null) { return Rhino.Geometry.BoundingBox.Empty; }
            BH.oM.Geometry.BoundingBox bhBox = Geometry().IBounds();
            if (bhBox == null) { return Rhino.Geometry.BoundingBox.Empty; }
            return bhBox.ToRhino();
        }


        /***************************************************/
        /**** IGH_PreviewData methods                   ****/
        /***************************************************/

        public override void DrawViewportMeshes(GH_PreviewMeshArgs args)
        {
            if (Geometry() == null) { return; }
            /*if (typeof(BH.oM.Geometry.Mesh).IsAssignableFrom(Value.GetType()))
                Render.RenderBHoMGeometry((Mesh)Value, args);*/
        }

        /***************************************************/

        public override void DrawViewportWires(GH_PreviewWireArgs args)
        {
            if (Value is BHoMObject) 
                Render.IRenderBHoMObject(Value as BHoMObject, args);
        }


        /***************************************************/
        /**** IGH_BakeAwareData methods                 ****/
        /***************************************************/

        public override bool BakeGeometry(RhinoDoc doc, ObjectAttributes att, out Guid obj_guid)
        {
            obj_guid = doc.Objects.Add(Geometry().IToRhino() as Rhino.Geometry.GeometryBase, att); // TODO: Check what happend when geometry is not GeometryBase
            return true;
        }


        /***************************************************/
        /**** Private Method                            ****/
        /***************************************************/

        private IGeometry Geometry()
        {
            if (Value is BHoMObject)
                return ((BHoMObject)Value).IGeometry();
            else if (Value is IGeometry)
                return Value as IGeometry;
            else
                return null;
        }

        /***************************************************/
    }
}
