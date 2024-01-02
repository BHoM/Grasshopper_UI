/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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
using BH.Engine.Reflection;
using System.Drawing;
using BH.oM.Graphics;
using Rhino.Render;

namespace BH.UI.Grasshopper.Goos
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
            if (source == null)
                return false;

            if (source is IGH_Goo)
                return CastFrom(Helpers.IFromGoo<T>((IGH_Goo)source));

            if (source.GetType().Namespace.StartsWith("Rhino.Geometry"))
                source = BH.Engine.Rhinoceros.Convert.IFromRhino(source);

            return base.CastFrom(source);
        }

        /***************************************************/

        public override bool CastTo<Q>(ref Q target)
        {
            try
            {
                if (Value == null)
                    target = default(Q);
                if (target is IGH_GeometricGoo || target is GH_Transform || target is GH_Matrix || target is GH_Vector)
                    return Helpers.CastToGoo(m_RhinoGeometry as dynamic, ref target);
                else
                    return Helpers.CastToGoo(Value as dynamic, ref target);
            }
            catch (Exception)
            {
                string message = string.Format("Impossible to convert {0} into {1}. Check the description of each input for more details on the type of object that need to be provided", Value.GetType().FullName, typeof(Q).FullName);
                BH.Engine.Base.Compute.RecordError(message);
                return false;
            }
        }

        /***************************************************/

        public override bool Read(GH_IReader reader)
        {
            string json = "";
            reader.TryGetString("Json", ref json);

            if (json != null && json.Length > 0)
            {
                object fromJson = BH.Engine.Serialiser.Convert.FromJson(json);
                try
                {
                    Value = (T)fromJson;
                }
                catch
                {
                    string message = string.Format("Impossible to convert {0} into {1}. Check the description of each input for more details on the type of object that need to be provided", fromJson.GetType().FullName, typeof(T).IToText());
                    BH.Engine.Base.Compute.RecordError(message);
                    return false;
                }
            }


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
            if (!m_IsMeshPreviewable)   //Flagged as not previewable - return
                return;

            if (m_PreviewMesh == null)  //No preview mesh set yet
            {
                //Create and store mesh. Will return values for surface type objects (surfaces, breps, meshes etc)
                m_PreviewMesh = Render.ICreatePreviewMesh(m_RhinoGeometry, args.MeshingParameters);

                if (m_PreviewMesh == null)
                {
                    m_IsMeshPreviewable = false;    //If no mesh could be extracted, set flag as not required to check again for the same geometry
                    return;
                }
            }

            if (m_PreviewMesh != null)
            {
                if (m_PreviewMesh.VertexColors.Count > 0)   //If colours are set (RenderMeshes) draw these colours
                {
                    args.Pipeline.DrawMeshFalseColors(m_PreviewMesh);
                }
                else
                {
                    Rhino.Display.DisplayMaterial mat = Render.RenderMaterial(args.Material, m_PreviewMaterial);    //If material is default GH material, BHoM default will be used, if not, default GH material is used.
                    args.Pipeline.DrawMeshShaded(m_PreviewMesh, mat);
                }
            }
        }


        /***************************************************/

        public virtual void DrawViewportWires(GH_PreviewWireArgs args)
        {
            if (m_thickness > 0)
            {
                if (m_RhinoGeometry != null)
                    Render.IRenderRhinoWires(m_RhinoGeometry, args, m_Color, m_thickness);
            }
        }


        /***************************************************/
        /**** Private Method                            ****/
        /***************************************************/

        private bool SetGeometry()
        {
            ResetPreviewMeshes();   //Clears cashed preview meshes and resets preview flag.

            if (Value == null)
            {
                m_IsMeshPreviewable = false;
                return true;
            }
            else if (Value is IRender)
            {
                m_RhinoGeometry = (Value as IRender).IToRhino();
                m_Color = (Value as IRender).Colour;
                BH.oM.Graphics.Texture texture = null;

                if (Value is RenderGeometry)
                {
                    RenderGeometry renderGeom = Value as RenderGeometry;
                    m_Geometry = renderGeom.Geometry;
                    m_thickness = renderGeom.EdgeThickness;
                    texture = renderGeom.SurfaceColour;
                }
                else if (Value is RenderCurve)
                {
                    m_thickness = (Value as RenderCurve).Thickness;
                    m_Geometry = (Value as RenderCurve).Curve;
                }

                if (texture != null)
                {
                    m_PreviewMaterial = texture.ToRhino();
                }
                else
                {
                    double transparency = (255 - m_Color.A) / (double)255;
                    m_PreviewMaterial = new Rhino.Display.DisplayMaterial(m_Color, transparency);
                }
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
                m_IsMeshPreviewable = false;
                return false;
            }
        }


        /***************************************************/

        protected virtual void ResetPreviewMeshes()
        {
            if (m_PreviewMesh != null)
            {
                m_PreviewMesh.Dispose();
                m_PreviewMesh = null;
            }

            m_IsMeshPreviewable = true; //Default to true until checked
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
                {
                    att = new ObjectAttributes();
                    att.LayerIndex = doc.Layers.CurrentLayerIndex;
                }
                    
                if(Value is IRender)
                {
                    
                    //bake with render colour always
                    att.ColorSource = ObjectColorSource.ColorFromObject;
                    att.ObjectColor = m_Color;
                    //special case for Text3d as it is not GeometryBase
                    if (Value is RenderText)
                        obj_guid = doc.Objects.AddText((Rhino.Display.Text3d)m_RhinoGeometry, att);
                    else
                        obj_guid = doc.Objects.Add(GH_Convert.ToGeometryBase(m_RhinoGeometry), att);

                }
                else
                {
                    BHoMObject bhObj = Value as BHoMObject;

                    if (string.IsNullOrEmpty(att.Name) && bhObj != null && !string.IsNullOrWhiteSpace(bhObj.Name))
                        att.Name = bhObj.Name;
                    obj_guid = doc.Objects.Add(GH_Convert.ToGeometryBase(m_RhinoGeometry), att);
                }

                return true;
            }

            obj_guid = Guid.Empty;
            return false;
        }

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        protected IGeometry m_Geometry = null;

        protected object m_RhinoGeometry = null;

        protected Color m_Color = Color.FromArgb(80, 255, 41, 105);//BHoM pink!

        protected int m_thickness = 2;

        protected Rhino.Geometry.Mesh m_PreviewMesh = null;

        protected bool m_IsMeshPreviewable = true;

        protected Rhino.Display.DisplayMaterial m_PreviewMaterial = m_DefaultMaterial;

        private static readonly Rhino.Display.DisplayMaterial m_DefaultMaterial = new Rhino.Display.DisplayMaterial(Color.FromArgb(80, 255, 41, 105), 0.58823529);
        
        /***************************************************/
    }
}


