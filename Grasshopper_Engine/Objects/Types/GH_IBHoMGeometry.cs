/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using System;
using BH.Engine.Rhinoceros;
using Rhino;
using Rhino.DocObjects;
using GH_IO.Serialization;
using BH.Engine.Serialiser;
using GH_IO;

namespace BH.Engine.Grasshopper.Objects
{
    public class GH_IBHoMGeometry : GH_GeometricGoo<object>, IGH_PreviewData, IGH_BakeAwareData, GH_ISerializable
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override string TypeName { get; } = "IGeometry";

        public override string TypeDescription { get; } = "Contains a generic BHoM Geometry";

        public override bool IsValid { get { return m_Value != null; } }

        public Rhino.Geometry.BoundingBox ClippingBox { get { return Bounds(); } }

        public override Rhino.Geometry.BoundingBox Boundingbox { get { return Bounds(); } }

        public override object Value // This is a Rhino.Geometry object if possibile, a BH.oM.Geometry otherwise
        {
            get
            {
                if (m_Value == null)
                    return null;
                else if (m_RhinoValue != null)
                    return m_RhinoValue;
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
            SetRhinoValue();
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
            // Return object name if 
            object value = Value;
            if (value == null)
                return "Null";

            Type type = value.GetType();
            if (value is IGeometry)
                return type.ToString();

            switch (type.Name)
            {
                case "Arc":
                    return GH_Format.FormatArc((Rhino.Geometry.Arc)value);
                case "Box":
                    return GH_Format.FormatBox((Rhino.Geometry.Box)value);
                case "BoundingBox":
                    return GH_Format.FormatBox(new Rhino.Geometry.Box((Rhino.Geometry.BoundingBox)value));
                case "Brep":
                    return GH_Format.FormatBrep((Rhino.Geometry.Brep)value);
                case "Circle":
                    return GH_Format.FormatCircle((Rhino.Geometry.Circle)value);
                case "Ellipse":
                    return GH_Format.FormatCurve(((Rhino.Geometry.Ellipse)value).ToNurbsCurve());
                case "ArcCurve":
                case "LineCurve":
                case "NurbsCurve":
                case "PolyCurve":
                case "PolylineCurve":
                    return GH_Format.FormatCurve((Rhino.Geometry.Curve)value);
                case "Line":
                    return GH_Format.FormatLine((Rhino.Geometry.Line)value);
                case "Mesh":
                    return GH_Format.FormatMesh((Rhino.Geometry.Mesh)value);
                case "Plane":
                    return GH_Format.FormatPlane((Rhino.Geometry.Plane)value);
                case "Point3d":
                    return GH_Format.FormatPoint((Rhino.Geometry.Point3d)value);
                case "Vector3d":
                    return GH_Format.FormatVector((Rhino.Geometry.Vector3d)value);
                case "MeshFace":
                    return GH_Format.FormatMeshFace(new GH_MeshFace((Rhino.Geometry.MeshFace)value));
                case "Transform":
                    return (new GH_Transform((Rhino.Geometry.Transform)value)).ToString();
                default:
                    return type.ToString();
            }
        }

        /***************************************************/

        public override Rhino.Geometry.BoundingBox GetBoundingBox(Rhino.Geometry.Transform xform)
        {
            try
            {
                if (m_Value == null) { return Rhino.Geometry.BoundingBox.Empty; }
                return m_Value.IBounds().ToRhino();
            }
            catch
            {
                return Rhino.Geometry.BoundingBox.Empty;
            }
        }

        /***************************************************/

        public override object ScriptVariable()
        {
            return m_Value;
        }

        /***************************************************/

        public override bool Read(GH_IReader reader)
        {
            string json = "";
            reader.TryGetString("Json", ref json);

            if (json != null && json.Length > 0)
                m_Value = BH.Engine.Serialiser.Convert.FromJson(json) as IGeometry;

            return true;
        }

        /***************************************************/

        public override bool Write(GH_IWriter writer)
        {
            if (Value != null)
                writer.SetString("Json", m_Value.ToJson());
            return true;
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
            else if (source is IGH_Goo)
                return CastFrom(((IGH_Goo)source).ScriptVariable());
            else if (source is Rhino.Geometry.Vector3d)
                m_Value = ((Rhino.Geometry.Vector3d)source).ToBHoM();
            else if (source is Rhino.Geometry.Plane)
                m_Value = ((Rhino.Geometry.Plane)source).ToBHoM();
            else if (source is Rhino.Geometry.BoundingBox)
                m_Value = ((Rhino.Geometry.BoundingBox)source).ToBHoM();
            else if (source is Rhino.Geometry.Box)
                m_Value = ((Rhino.Geometry.Box)source).ToBHoM();
            else if (source is Rhino.Geometry.MeshFace)
                m_Value = ((Rhino.Geometry.MeshFace)source).ToBHoM();
            else if (source is Rhino.Geometry.Transform)
                m_Value = ((Rhino.Geometry.Transform)source).ToBHoM();
            else if (source is Rhino.Geometry.Matrix)
            {
                GH_Transform transform = new GH_Transform();
                transform.CastFrom(source);
                m_Value = transform.Value.ToBHoM();
            }
            else if (source is Rhino.Geometry.Ellipse)
            {
                m_Value = ((Rhino.Geometry.Ellipse)source).ToBHoM();
            }
            else
                m_Value = GH_Convert.ToGeometryBase(source).IToBHoM();

            SetRhinoValue();
            return true;
        }

        /***************************************************/

        public override bool CastTo<Q>(ref Q target)
        {
            try
            {
                object value = Value;
                if (value == null)
                    target = default(Q);
                if (target is GH_Vector)
                {
                    GH_Vector vector = null;
                    GH_Convert.ToGHVector(value, GH_Conversion.Both, ref vector);
                    target = (Q)(object)vector;
                }
                else if (target is GH_Curve)
                {
                    GH_Curve curve = null;
                    GH_Convert.ToGHCurve(value, GH_Conversion.Both, ref curve);
                    target = (Q)(object)curve;
                }
                else if (target is GH_Surface)
                {
                    GH_Surface surface = null;
                    GH_Convert.ToGHSurface(value, GH_Conversion.Both, ref surface);
                    target = (Q)(object)surface;
                }
                else if (target is GH_Brep)
                {
                    GH_Brep bRep = null;
                    GH_Convert.ToGHBrep(value, GH_Conversion.Both, ref bRep);
                    target = (Q)(object)bRep;
                }
                else if (target is GH_MeshFace)
                {
                    GH_MeshFace face = null;
                    GH_Convert.ToGHMeshFace(value, GH_Conversion.Both, ref face);
                    target = (Q)(object)face;
                }
                else if (target is GH_Transform)
                {
                    GH_Transform transform = new GH_Transform(value as dynamic);
                    target = (Q)(object)transform;
                }
                else if (target is GH_Matrix)
                {
                    GH_Matrix transform = new GH_Matrix(value as dynamic);
                    target = (Q)(object)transform;
                }
                else if (target is IGH_GeometricGoo)
                    target = (Q)GH_Convert.ToGeometricGoo(Value);
                else
                    target = (Q)Value;

                return true;
            }
            catch (Exception)
            {
                string message = string.Format("Impossible to convert {0} into {1}. Check the description of each input for more details on the type of object that need to be provided", Value.GetType().FullName, typeof(Q).FullName);
                BH.Engine.Reflection.Compute.RecordError(message);
                return false;
            }
        }


        /***************************************************/
        /**** Transformation methods                    ****/
        /***************************************************/

        public override IGH_GeometricGoo Transform(Rhino.Geometry.Transform xform)
        {
            if (m_Value == null)
                return null;
            else
                return new GH_IBHoMGeometry { Value = m_Value.ITransform(xform.ToBHoM()) };
        }

        /***************************************************/

        public override IGH_GeometricGoo Morph(Rhino.Geometry.SpaceMorph xmorph)
        {
            object value = Value;
            if (value == null)
                return null;
            else if (value is Rhino.Geometry.Point3d)
                return new GH_IBHoMGeometry { Value = xmorph.MorphPoint((Rhino.Geometry.Point3d)value) };
            else
            {
                Rhino.Geometry.GeometryBase geometry = ((Rhino.Geometry.GeometryBase)value).Duplicate();
                return new GH_IBHoMGeometry { Value = xmorph.Morph(geometry) };
            }
        }


        /***************************************************/
        /**** IGH_PreviewData methods                   ****/
        /***************************************************/

        public void DrawViewportMeshes(GH_PreviewMeshArgs args)
        {
            Engine.Grasshopper.Compute.IRenderRhinoMeshes(Value, args);
        }

        /***************************************************/

        public void DrawViewportWires(GH_PreviewWireArgs args)
        {
            Engine.Grasshopper.Compute.IRenderRhinoWires(Value, args);
        }


        /***************************************************/
        /**** IGH_BakeAwareData methods                 ****/
        /***************************************************/

        public bool BakeGeometry(RhinoDoc doc, ObjectAttributes att, out Guid obj_guid)
        {
            if (m_RhinoValue != null)
            {
                obj_guid = doc.Objects.Add(GH_Convert.ToGeometryBase(m_RhinoValue), att);
                return true;
            }

            obj_guid = Guid.Empty;
            return false;
        }


        /***************************************************/
        /**** Private Method                            ****/
        /***************************************************/

        private Rhino.Geometry.BoundingBox Bounds()
        {
            try
            {
                if (m_Value == null)
                    return Rhino.Geometry.BoundingBox.Empty;

                BH.oM.Geometry.BoundingBox bhBox = m_Value.IBounds();
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

        private bool SetRhinoValue()
        {
            if (BH.Engine.Rhinoceros.Query.IsRhinoEquivalent(m_Value.GetType()))
                m_RhinoValue = m_Value.IToRhino();
            return true;
        }

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private IGeometry m_Value = null;

        private object m_RhinoValue = null;

        /***************************************************/
    }
}
