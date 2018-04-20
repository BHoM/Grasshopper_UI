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
    public class GH_IObject : GH_TemplateType<IObject>, IGH_PreviewData, IGH_BakeAwareData
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override string TypeName { get; } = "IObject";

        public override string TypeDescription { get; } = "Contains a generic IObject"; 

        public virtual Rhino.Geometry.BoundingBox ClippingBox { get { return Boundingbox; } }

        public virtual Rhino.Geometry.BoundingBox Boundingbox
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

        public GH_IObject() : base() { }

        /***************************************************/

        public GH_IObject(IObject val) : base(val) { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override IGH_Goo Duplicate()
        {
            return new GH_IObject { Value = Value };
        }

        /***************************************************/

        public virtual Rhino.Geometry.BoundingBox GetBoundingBox(Rhino.Geometry.Transform xform)
        {
            if (Value == null) { return Rhino.Geometry.BoundingBox.Empty; }
            if (Geometry() == null) { return Rhino.Geometry.BoundingBox.Empty; }
            BH.oM.Geometry.BoundingBox bhBox = Geometry().IBounds();
            if (bhBox == null) { return Rhino.Geometry.BoundingBox.Empty; }
            return bhBox.ToRhino();
        }

        /***************************************************/

        public override bool CastFrom(object source)
        {
            while (source is IGH_Goo)
                source = ((IGH_Goo)source).ScriptVariable();

            if (source.GetType().Namespace.StartsWith("Rhino.Geometry"))
                source = Engine.Rhinoceros.Convert.ToBHoM(source as dynamic);

            return base.CastFrom(source);
        }


        /***************************************************/
        /**** IGH_PreviewData methods                   ****/
        /***************************************************/

        public virtual void DrawViewportMeshes(GH_PreviewMeshArgs args)
        {
            if (Geometry() == null) { return; }
            /*if (typeof(BH.oM.Geometry.Mesh).IsAssignableFrom(Value.GetType()))
                Render.RenderBHoMGeometry((Mesh)Value, args);*/
        }

        /***************************************************/

        public virtual void DrawViewportWires(GH_PreviewWireArgs args)
        {
            if (Value is BHoMObject) 
                Render.IRenderBHoMObject(Value as BHoMObject, args);
            else if (Value is IGeometry)
                Render.IRenderBHoMGeometry(Value as IGeometry, args);
        }


        /***************************************************/
        /**** IGH_BakeAwareData methods                 ****/
        /***************************************************/

        public virtual bool BakeGeometry(RhinoDoc doc, ObjectAttributes att, out Guid obj_guid)
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
