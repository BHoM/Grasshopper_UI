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

using BH.Engine.Geometry;
using BH.Engine.Rhinoceros;
using BH.Engine.Serialiser;
using BH.oM.Geometry;
using GH_IO;
using GH_IO.Serialization;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino;
using Rhino.DocObjects;
using System;

namespace BH.Engine.Grasshopper.Objects
{
    public class BHGoo<T> : GH_Goo<T>, GH_ISerializable, IGH_BakeAwareData
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override bool IsValid { get { return Value != null; } }

        public override string TypeName { get { return typeof(T).Name; } }

        public override string TypeDescription { get { return typeof(T).FullName; } }

        public virtual IGeometry Geometry { get { return m_Geometry; } }

        public override T Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                base.Value = value;
                SetGeometry();
            }
        }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public BHGoo()
        {
            this.Value = default(T);
        }

        /***************************************************/

        public BHGoo(T val)
        {
            this.Value = val;
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override IGH_Goo Duplicate()
        {
            return new BHGoo<T> { Value = Value };
        }

        /*******************************************/

        public override string ToString()
        {
            return Value?.ToString();
        }

        /*******************************************/

        public override object ScriptVariable()
        {
            return Value;
        }

        /*******************************************/

        public override bool CastTo<Q>(ref Q target)
        {
            try
            {
                object ptr = this.Value;
                target = (Q)ptr;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /***************************************************/

        public override bool CastFrom(object source)
        {
            if (source == null)
                return false;

            while (source is IGH_Goo)
                source = ((IGH_Goo)source).ScriptVariable();
            
            if (source is T)
                this.Value = (T)(source);
            else if (source is IGeometry) // This allows cast like BH.oM.Geometry.Point to BH.oM.Structure.Elements.Node
            {
                this.m_Geometry = (IGeometry)source;
                Engine.Reflection.Compute.ClearCurrentEvents();
                Engine.Reflection.Compute.RecordError($"Cannot cast {source.GetType()} to {this.TypeName}");
            }
            else
                return false;
            return true;
        }


        /***************************************************/
        /**** GH_ISerializable Methods                  ****/
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
        /**** IGH_PreviewData Methods                   ****/
        /***************************************************/

        public virtual void DrawViewportMeshes(GH_PreviewMeshArgs args)
        {
            Engine.Grasshopper.Compute.IRenderRhinoMeshes(m_RhinoGeometry, args);
        }

        /***************************************************/

        public virtual void DrawViewportWires(GH_PreviewWireArgs args)
        {
            Engine.Grasshopper.Compute.IRenderRhinoWires(m_RhinoGeometry, args);
        }


        /***************************************************/
        /**** IGH_BakeAwareData Methods                   ****/
        /***************************************************/

        public bool BakeGeometry(RhinoDoc doc, ObjectAttributes att, out Guid obj_guid)
        {
            if (m_RhinoGeometry != null)
            {
                obj_guid = doc.Objects.Add(GH_Convert.ToGeometryBase(m_RhinoGeometry), att);
                return true;
            }

            obj_guid = Guid.Empty;
            return false;
        }


        /***************************************************/
        /**** Virtual Methods                           ****/
        /***************************************************/

        protected virtual bool SetGeometry()
        {
            return false;
        }

        /***************************************************/

        protected virtual Rhino.Geometry.BoundingBox Bounds()
        {
            if (Value == null)
                return Rhino.Geometry.BoundingBox.Empty;

            if (m_Geometry == null)
                return Rhino.Geometry.BoundingBox.Empty;

            try
            {
                BH.oM.Geometry.BoundingBox bhBox = m_Geometry.IBounds();
                if (bhBox == null)
                    return Rhino.Geometry.BoundingBox.Empty;

                return bhBox.ToRhino();
            }
            catch { }

            return Rhino.Geometry.BoundingBox.Empty;
        }


        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        protected IGeometry m_Geometry = null;

        protected object m_RhinoGeometry = null;

        /***************************************************/
    }
}
