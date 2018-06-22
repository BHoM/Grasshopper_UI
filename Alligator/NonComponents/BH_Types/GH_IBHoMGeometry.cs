using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using System;
using BH.Engine.Rhinoceros;
using Rhino;
using Rhino.DocObjects;

namespace BH.UI.Alligator
{
    public class GH_IBHoMGeometry : GH_GeometricGoo<object>, IGH_PreviewData, IGH_BakeAwareData
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override string TypeName { get; } = "IGeometry";

        public override string TypeDescription { get; } = "Contains a generic BHoM Geometry";

        public override bool IsValid { get { return m_Value != null;  } }

        public Rhino.Geometry.BoundingBox ClippingBox { get { return Boundingbox; } }

        public override Rhino.Geometry.BoundingBox Boundingbox
        {
            get
            {
                return (Value == null) ? Rhino.Geometry.BoundingBox.Empty : m_Value.IBounds().ToRhino();
            }
        }

        public override object Value
        {
            get
            {
                if (m_Value == null)
                    return null;
                else if (Engine.Rhinoceros.Query.IsRhinoEquivalent(m_Value.GetType()))
                    return m_Value.IToRhino();
                else
                    return m_Value;
            }

            set
            {
                CastFrom(value);
            }
        }


        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public GH_IBHoMGeometry()
        {
            m_Value = null;
        }

        /***************************************************/

        public GH_IBHoMGeometry(object bh)
        {
            Value = bh;
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override IGH_GeometricGoo DuplicateGeometry()
        {
            return new GH_IBHoMGeometry { Value = m_Value.IClone() };
        }

        /***************************************************/

        public override string ToString()
        {
            string type = m_Value.GetType().ToString();
            if (m_Value == null) { return "null IGeometry"; }
            else if (typeof(BH.oM.Geometry.Point).IsAssignableFrom(m_Value.GetType()))
            {
                BH.oM.Geometry.Point pt = (BH.oM.Geometry.Point)m_Value;
                return (m_Value.GetType().ToString() + " {" + pt.X + ", " + pt.Y + ", " + pt.Z + "}");
            }
            else if (typeof(BH.oM.Geometry.Vector).IsAssignableFrom(m_Value.GetType()))
            {
                BH.oM.Geometry.Vector vec = (BH.oM.Geometry.Vector)m_Value;
                return (m_Value.GetType().ToString() + " {" + vec.X + ", " + vec.Y + ", " + vec.Z + "}");
            }
            else
                return m_Value.ToString();
        }

        /***************************************************/

        public override Rhino.Geometry.BoundingBox GetBoundingBox(Rhino.Geometry.Transform xform)
        {
            if (m_Value == null) { return Rhino.Geometry.BoundingBox.Empty; }
            return m_Value.IBounds().ToRhino();
        }

        /***************************************************/

        public override object ScriptVariable()
        {
            return m_Value;
        }


        /***************************************************/
        /**** Automatic casting methods                 ****/
        /***************************************************/

        public override bool CastFrom(object source)
        {
            if (source == null) { return false; }

            else if (source is IGeometry)
                m_Value = (IGeometry)source;
            else if (source is Rhino.Geometry.GeometryBase)
                m_Value = ((Rhino.Geometry.GeometryBase)source).IToBHoM();
            else if (source is GH_Vector)                   //TODO: Check if there are other exceptions that do not convert to GeometryBase
                m_Value = ((GH_Vector)source).Value.ToBHoM();
            else if (source is GH_Box)
                m_Value = (((GH_Box)source).Value).ToBHoM();
            else if (source is GH_Plane)
                m_Value = (((GH_Plane)source).Value).ToBHoM();
            else if (source is GH_Circle)
                m_Value = (((GH_Circle)source).Value).ToBHoM();
            else if (source is GH_Curve)
                m_Value = (((GH_Curve)source).Value).ToBHoM();
            else if (source is IGH_GeometricGoo)
                m_Value = GH_Convert.ToGeometryBase(source).IToBHoM();

            return true;
        }

        /***************************************************/

        public override bool CastTo<Q>(ref Q target)
        {
            try
            {
                object ptr = Value;
                target = (Q)ptr;
                return true;
            }
            catch (Exception)
            {
                string message = string.Format("Impossible to convert {0} into {1}. Check the description of each input for more details on the type of object that need to be provided", Value.GetType().FullName, typeof(Q).FullName);
                throw new Exception(message);
            }
        }


        /***************************************************/
        /**** Transformation methods                    ****/
        /***************************************************/

        public override IGH_GeometricGoo Transform(Rhino.Geometry.Transform xform)
        {
            if (m_Value == null) { return null; }
            return this;
        }

        /***************************************************/

        public override IGH_GeometricGoo Morph(Rhino.Geometry.SpaceMorph xmorph)
        {
            if (m_Value == null) { return null; }
            return this;
        }


        /***************************************************/
        /**** IGH_PreviewData methods                   ****/
        /***************************************************/

        public void DrawViewportMeshes(GH_PreviewMeshArgs args)
        {
            if (m_Value == null) { return; }
            if (typeof(BH.oM.Geometry.Mesh).IsAssignableFrom(m_Value.GetType()))
                Render.RenderBHoMGeometry((BH.oM.Geometry.Mesh)m_Value, args);
        }

        /***************************************************/

        public void DrawViewportWires(GH_PreviewWireArgs args)
        {
            if (m_Value == null) { return; }
            Render.IRenderBHoMGeometry(m_Value as dynamic, args);
        }


        /***************************************************/
        /**** IGH_BakeAwareData methods                 ****/
        /***************************************************/

        public bool BakeGeometry(RhinoDoc doc, ObjectAttributes att, out Guid obj_guid)
        {
            obj_guid = doc.Objects.Add(m_Value.IToRhino() as Rhino.Geometry.GeometryBase, att); // TODO: Check what happend when geometry is not GeometryBase
            return true;
        }

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private IGeometry m_Value = null;


        /***************************************************/
    }
}
