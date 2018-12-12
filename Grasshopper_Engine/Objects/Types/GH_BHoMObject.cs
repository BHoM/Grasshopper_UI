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
using GH_IO;
using GH_IO.Serialization;
using BH.Engine.Serialiser;

namespace BH.Engine.Alligator.Objects
{
    public class GH_BHoMObject : GH_BHoMGoo<object>, IGH_PreviewData, GH_ISerializable
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override string TypeName { get; } = "BHoMObject";

        public override string TypeDescription { get; } = "Contains a BHoM IObject";

        public override bool IsValid { get { return Value != null; } }

        public virtual Rhino.Geometry.BoundingBox ClippingBox { get { return Bounds(); } }

        public virtual Rhino.Geometry.BoundingBox Boundingbox { get { return Bounds(); } }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public GH_BHoMObject() : base() { }

        /***************************************************/

        public GH_BHoMObject(BHoMObject val) : base(val)
        {
            SetGeometry();
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override IGH_Goo Duplicate()
        {
            return new GH_BHoMObject { Value = Value };
        }

        /***************************************************/

        public virtual Rhino.Geometry.BoundingBox GetBoundingBox(Rhino.Geometry.Transform xform)
        {
            Rhino.Geometry.BoundingBox box = Bounds();
            box.Transform(xform);
            return box;
        }

        /***************************************************/

        public override bool CastFrom(object source)
        {
            while (source is IGH_Goo)
                source = ((IGH_Goo)source).ScriptVariable();

            if (source.GetType().Namespace.StartsWith("Rhino.Geometry"))
                source = BH.Engine.Rhinoceros.Convert.ToBHoM(source as dynamic);

            if (base.CastFrom(source))
            {
                SetGeometry();
                return true;
            }

            return false;
        }

        /***************************************************/

        public override string ToString()
        {
            object val = Value;
            if (val == null)
                return "null";
            else
                return val.ToString();
        }

        /***************************************************/

        public override bool Read(GH_IReader reader)
        {
            string json = "";
            reader.TryGetString("Json", ref json);

            if (json != null && json.Length > 0)
                Value = (BHoMObject)BH.Engine.Serialiser.Convert.FromJson(json);

            return true;
        }

        /***************************************************/

        public override bool Write(GH_IWriter writer)
        {
            if (Value != null)
                writer.SetString("Json", Value.ToJson());
            return true;
        }

        /***************************************************/

        public virtual void DrawViewportMeshes(GH_PreviewMeshArgs args)
        {
            Engine.Alligator.Compute.IRenderMeshes(Geometry, args);
        }

        /***************************************************/

        public virtual void DrawViewportWires(GH_PreviewWireArgs args)
        {
            Engine.Alligator.Compute.IRenderWires(Geometry, args);
        }


        /***************************************************/
        /**** Private Method                            ****/
        /***************************************************/

        private bool SetGeometry()
        {
            if (Value == null)
            {
                return true;
            }
            else if (Value is BHoMObject)
            {
                Geometry = ((BHoMObject)Value).IGeometry();
                return true;
            }
            else if (Value is IGeometry)
            {
                Geometry = Value as IGeometry;
                return true;
            }
            else
            {
                return false;
            }
        }

        /***************************************************/

        private Rhino.Geometry.BoundingBox Bounds()
        {
            try
            {
                if (Value == null)
                    return Rhino.Geometry.BoundingBox.Empty;

                if (Geometry == null)
                    return Rhino.Geometry.BoundingBox.Empty;

                BH.oM.Geometry.BoundingBox bhBox = Geometry.IBounds();
                if (bhBox == null)
                    return Rhino.Geometry.BoundingBox.Empty;

                return bhBox.ToRhino();
            }
            catch
            {
                return Rhino.Geometry.BoundingBox.Empty;
            }
        }


        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private IGeometry Geometry = null;

        /***************************************************/
    }
}
