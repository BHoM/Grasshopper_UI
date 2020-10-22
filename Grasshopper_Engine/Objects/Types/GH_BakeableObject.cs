/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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

namespace BH.Engine.Grasshopper.Objects
{
    public class GH_BakeableObject<T> : GH_BHoMGoo<T>, IGH_PreviewData, IGH_BakeAwareData, GH_ISerializable
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override string TypeName { get; } = "ViewableObject";

        public override string TypeDescription { get; } = "Contains a generic object that can be viewed in Rhino";

        public override bool IsValid { get { return Value != null; } }

        public virtual Rhino.Geometry.BoundingBox ClippingBox { get { return Bounds(); } }

        public virtual Rhino.Geometry.BoundingBox Boundingbox { get { return Bounds(); } }

        public override T Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                base.Value = value;
                try { SetGeometry(); } catch { }
            }
        }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public GH_BakeableObject() : base() { }

        /***************************************************/

        public GH_BakeableObject(T val) : base(val) { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override IGH_Goo Duplicate()
        {
            return new GH_BakeableObject<T> { Value = Value };
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
            if (source is IGH_Goo)
                return CastFrom(Convert.IFromGoo<object>((IGH_Goo)source));

            if (source.GetType().Namespace.StartsWith("Rhino.Geometry"))
                source = BH.Engine.Rhinoceros.Convert.FromRhino(source as dynamic);

            return base.CastFrom(source);
        }

        /***************************************************/

        public override bool CastTo<Q>(ref Q target)
        {
            try
            {
                if (Value == null)
                    target = default(Q);
                if (target is GH_Vector)
                {
                    GH_Vector vector = null;
                    GH_Convert.ToGHVector(m_RhinoGeometry, GH_Conversion.Both, ref vector);
                    target = (Q)(object)vector;
                }
                else if (target is GH_Curve)
                {
                    GH_Curve curve = null;
                    GH_Convert.ToGHCurve(m_RhinoGeometry, GH_Conversion.Both, ref curve);
                    target = (Q)(object)curve;
                }
                else if (target is GH_Surface)
                {
                    GH_Surface surface = null;
                    GH_Convert.ToGHSurface(m_RhinoGeometry, GH_Conversion.Both, ref surface);
                    target = (Q)(object)surface;
                }
                else if (target is GH_Brep)
                {
                    GH_Brep bRep = null;
                    GH_Convert.ToGHBrep(m_RhinoGeometry, GH_Conversion.Both, ref bRep);
                    target = (Q)(object)bRep;
                }
                else if (target is GH_MeshFace)
                {
                    GH_MeshFace face = null;
                    GH_Convert.ToGHMeshFace(m_RhinoGeometry, GH_Conversion.Both, ref face);
                    target = (Q)(object)face;
                }
                else if (target is GH_Transform)
                {
                    GH_Transform transform = new GH_Transform(m_RhinoGeometry as dynamic);
                    target = (Q)(object)transform;
                }
                else if (target is GH_Matrix)
                {
                    GH_Matrix transform = new GH_Matrix(m_RhinoGeometry as dynamic);
                    target = (Q)(object)transform;
                }
                else if (target is IGH_GeometricGoo)
                    target = (Q)GH_Convert.ToGeometricGoo(m_RhinoGeometry);
                else
                    target = (Q)(object)Value;

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
                Value = (T)BH.Engine.Serialiser.Convert.FromJson(json);

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
            if (m_RhinoGeometry != null)
                Engine.Grasshopper.Compute.IRenderRhinoMeshes(m_RhinoGeometry, args);
        }

        /***************************************************/

        public virtual void DrawViewportWires(GH_PreviewWireArgs args)
        {
            if (m_RhinoGeometry != null)
                Engine.Grasshopper.Compute.IRenderRhinoWires(m_RhinoGeometry, args);
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
                m_Geometry = (Value as BHoMObject).IGeometry();
                m_RhinoGeometry = m_Geometry.IToRhino();
                return true;
            }
            else if (Value is IGeometry)
            {
                m_Geometry = Value as IGeometry;
                m_RhinoGeometry = m_Geometry.IToRhino();
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

                BH.oM.Geometry.BoundingBox bhBox = m_Geometry.IBounds();
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
        /**** IGH_BakeAwareData methods                 ****/
        /***************************************************/

        public bool BakeGeometry(RhinoDoc doc, ObjectAttributes att, out Guid obj_guid)
        {
            if (m_RhinoGeometry != null)
            {
                if (att == null)
                    att = new ObjectAttributes();

                BHoMObject bhObj = Value as BHoMObject;

                if (string.IsNullOrEmpty(att.Name) && bhObj != null && !string.IsNullOrWhiteSpace(bhObj.Name))
                    att.Name = bhObj.Name;

                obj_guid = doc.Objects.Add(GH_Convert.ToGeometryBase(m_RhinoGeometry), att);
                return true;
            }

            obj_guid = Guid.Empty;
            return false;
        }

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private IGeometry m_Geometry = null;

        private object m_RhinoGeometry = null;

        /***************************************************/
    }
}

