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
 # define ADAPTERNAME

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
    public class IGeometryGoo : GH_GeometricGoo<IGeometry>, IGH_PreviewData, IGH_BakeAwareData, GH_ISerializable
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override string TypeName { get; } = "IGeometry";

        public override string TypeDescription { get; } = "Contains a generic BHoM Geometry";

        public override bool IsValid { get { return Value != null; } }

        public Rhino.Geometry.BoundingBox ClippingBox { get { return Bounds(); } }

        public override Rhino.Geometry.BoundingBox Boundingbox { get { return Bounds(); } }

        public override IGeometry Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                base.Value = value;
                SetRhinoValue();
            }
        }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public IGeometryGoo()
        {
            this.Value = null;
        }

        /***************************************************/

        public IGeometryGoo(IGeometry val)
        {
            this.Value = val;
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override IGH_GeometricGoo DuplicateGeometry()
        {
            return new IGeometryGoo { Value = Value.IClone() };
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
                if (Value == null) { return Rhino.Geometry.BoundingBox.Empty; }
                return Value.IBounds().ToRhino();
            }
            catch
            {
                return Rhino.Geometry.BoundingBox.Empty;
            }
        }

        /***************************************************/

        public override object ScriptVariable()
        {
            return Value;
        }

        /***************************************************/

        public override bool Read(GH_IReader reader)
        {
            string json = "";
            reader.TryGetString("Json", ref json);

            if (json != null && json.Length > 0)
                Value = BH.Engine.Serialiser.Convert.FromJson(json) as IGeometry;

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
        /**** Automatic casting methods                 ****/
        /***************************************************/

        public override bool CastFrom(object source)
        {
            if (source == null) { return false; }

            else if (source is IGeometry)
                Value = (IGeometry)source;
            else if (source is Rhino.Geometry.GeometryBase)
                Value = ((Rhino.Geometry.GeometryBase)source).IToBHoM();
            else if (source is IGH_Goo)
                return CastFrom(((IGH_Goo)source).ScriptVariable());
            else if (source is Rhino.Geometry.Vector3d)
                Value = ((Rhino.Geometry.Vector3d)source).ToBHoM();
            else if (source is Rhino.Geometry.Plane)
                Value = ((Rhino.Geometry.Plane)source).ToBHoM();
            else if (source is Rhino.Geometry.BoundingBox)
                Value = ((Rhino.Geometry.BoundingBox)source).ToBHoM();
            else if (source is Rhino.Geometry.Box)
                Value = ((Rhino.Geometry.Box)source).ToBHoM();
            else if (source is Rhino.Geometry.MeshFace)
                Value = ((Rhino.Geometry.MeshFace)source).ToBHoM();
            else if (source is Rhino.Geometry.Transform)
                Value = ((Rhino.Geometry.Transform)source).ToBHoM();
            else if (source is Rhino.Geometry.Matrix)
            {
                GH_Transform transform = new GH_Transform();
                transform.CastFrom(source);
                Value = transform.Value.ToBHoM();
            }
            else if (source is Rhino.Geometry.Ellipse)
            {
                Value = ((Rhino.Geometry.Ellipse)source).ToBHoM();
            }
            else
                Value = GH_Convert.ToGeometryBase(source).IToBHoM();

            SetRhinoValue(); // this call should be redundant. Why is it not?
            return true;
        }

        /***************************************************/

        public override bool CastTo<Q>(ref Q target)
        {
            try
            {
                if (this.m_RhinoValue != null)
                    return Engine.Grasshopper.Modify.CastTo<Q>(this.m_RhinoValue, ref target);
                else
                    return Engine.Grasshopper.Modify.CastTo<Q>(Value, ref target);
            }
            catch (Exception)
            {
                return !BH.Engine.Reflection.Compute.RecordError($"Cannot convert {Value.GetType().FullName} to {typeof(Q).FullName}");
            }
        }


        /***************************************************/
        /**** Transformation methods                    ****/
        /***************************************************/

        public override IGH_GeometricGoo Transform(Rhino.Geometry.Transform xform)
        {
            if (Value == null)
                return null;
            else
                return new IGeometryGoo { Value = Value.ITransform(xform.ToBHoM()) };
        }

        /***************************************************/

        public override IGH_GeometricGoo Morph(Rhino.Geometry.SpaceMorph xmorph)
        {
            object value = Value;
            if (value == null)
                return null;
            else if (value is Rhino.Geometry.Point3d)
                return new IGeometryGoo { Value = xmorph.MorphPoint((Rhino.Geometry.Point3d)value).ToBHoM() };
            else
            {
                Rhino.Geometry.GeometryBase geometry = ((Rhino.Geometry.GeometryBase)value).Duplicate();
                xmorph.Morph(geometry);
                return new IGeometryGoo { Value = geometry.IToBHoM() };
            }
        }


        /***************************************************/
        /**** IGH_PreviewData methods                   ****/
        /***************************************************/

        public void DrawViewportMeshes(GH_PreviewMeshArgs args)
        {
            Engine.Grasshopper.Compute.IRenderRhinoMeshes(m_RhinoValue, args);
        }

        /***************************************************/

        public void DrawViewportWires(GH_PreviewWireArgs args)
        {
            Engine.Grasshopper.Compute.IRenderRhinoWires(m_RhinoValue, args);
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
        /**** Private Methods                           ****/
        /***************************************************/

        private Rhino.Geometry.BoundingBox Bounds()
        {
            try
            {
                if (Value == null)
                    return Rhino.Geometry.BoundingBox.Empty;

                BH.oM.Geometry.BoundingBox bhBox = Value.IBounds();
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
            if (Value == null)
                return true;
            else if (BH.Engine.Rhinoceros.Query.IsRhinoEquivalent(Value.GetType()))
                m_RhinoValue = Value.IToRhino();
            return true;
        }


        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private object m_RhinoValue = null;

        /***************************************************/
    }
}
