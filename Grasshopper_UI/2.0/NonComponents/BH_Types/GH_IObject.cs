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

namespace BH.UI.Grasshopper
{
    public class GH_IObject : GH_TemplateType<object>, IGH_PreviewData, IGH_BakeAwareData, GH_ISerializable
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
                try
                {
                    if (Value == null) { return Rhino.Geometry.BoundingBox.Empty; }
                    if (Geometry() == null) { return Rhino.Geometry.BoundingBox.Empty; }
                    BH.oM.Geometry.BoundingBox bhBox = Geometry().IBounds();
                    if (bhBox == null) { return Rhino.Geometry.BoundingBox.Empty; }
                    return bhBox.ToRhino();
                }
                catch
                {
                    return Rhino.Geometry.BoundingBox.Empty;
                }   
            }
        }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public GH_IObject() : base() { }

        /***************************************************/

        public GH_IObject(object val) : base(val) { }


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
            try
            {
                if (Value == null) { return Rhino.Geometry.BoundingBox.Empty; }
                if (Geometry() == null) { return Rhino.Geometry.BoundingBox.Empty; }
                BH.oM.Geometry.BoundingBox bhBox = Geometry().IBounds();
                if (bhBox == null) { return Rhino.Geometry.BoundingBox.Empty; }
                return bhBox.ToRhino();
            }
            catch
            {
                return Rhino.Geometry.BoundingBox.Empty;
            }
            
        }

        /***************************************************/

        public override bool CastFrom(object source)
        {
            while (source is IGH_Goo)
                source = ((IGH_Goo)source).ScriptVariable();

            if (source.GetType().Namespace.StartsWith("Rhino.Geometry"))
                source = BH.Engine.Rhinoceros.Convert.ToBHoM(source as dynamic);

            return base.CastFrom(source);
        }

        /***************************************************/

        public override bool Read(GH_IReader reader)
        {
            string json = "";
            reader.TryGetString("Json", ref json);

            if (json != null && json.Length > 0)
                Value = BH.Engine.Serialiser.Convert.FromJson(json);

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
