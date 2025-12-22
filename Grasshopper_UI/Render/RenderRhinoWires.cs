/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2026, the respective contributors. All rights reserved.
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

using RHG = Rhino.Geometry;
using Rhino.Display;
using Grasshopper.Kernel;
using System;
using System.Drawing;
using BH.oM.Base.Attributes;
using System.Collections.Generic;
using GH = Grasshopper;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.Engine.Adapters.Rhinoceros;

namespace BH.UI.Grasshopper
{ 
    public static partial class Render
    {
        /***************************************************/
        /**** Public Methods  - Interfaces              ****/
        /***************************************************/

        public static void IRenderRhinoWires(this object geometry, GH_PreviewWireArgs args, Color custom, int thickness)
        {
            if (geometry == null)
            {
                return;
            }
            Color bhColour = RenderColour(args.Color, custom);
            try
            {
                RenderRhinoWires(geometry as dynamic, args.Pipeline, bhColour, thickness);
            }
            catch (Exception) { }
        }

        /***************************************************/

        public static void RenderRhinoWires(this List<object> geometry, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int thickness)
        {
            foreach(object geo in geometry)
            {
                RenderRhinoWires(geo as dynamic, pipeline, bhColour, thickness);
            }
        }


        /***************************************************/
        /**** Private Methods  - Fallback               ****/
        /***************************************************/

        private static void RenderRhinoWires(this object fallback, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int thickness)
        {
            // fallback in case no method is found for the provided runtime type
            return;
        }


        /***************************************************/
        /**** Public Methods  - Vectors                 ****/
        /***************************************************/

        public static void RenderRhinoWires(RHG.Point3d point, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int thickness)
        {
            pipeline.DrawPoint(point, bhColour);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.Plane plane, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int thickness)
        {
            pipeline.DrawConstructionPlane(new Rhino.DocObjects.ConstructionPlane() { Plane = plane, ThickLineColor = bhColour, ThinLineColor = Color.Black, GridLineCount = 10 });
        }


        /***************************************************/
        /**** Public Methods  - Curves                  ****/
        /***************************************************/

        public static void RenderRhinoWires(RHG.Arc arc, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int thickness)
        {
            pipeline.DrawArc(arc, bhColour, thickness);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.ArcCurve arc, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int thickness)
        {
            pipeline.DrawArc(arc.Arc, bhColour, thickness);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.Circle circle, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int thickness)
        {
            pipeline.DrawCircle(circle, bhColour, thickness);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.Curve curve, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int thickness)
        {
            pipeline.DrawCurve(curve, bhColour, thickness);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.Ellipse ellipse, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int thickness)
        {
            pipeline.DrawCurve(ellipse.ToNurbsCurve(), bhColour, thickness);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.Line line, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int thickness)
        {
            pipeline.DrawLine(line, bhColour, thickness);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.LineCurve line, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int thickness)
        {
            pipeline.DrawLine(line.Line, bhColour, thickness);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.NurbsCurve curve, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int thickness)
        {
            pipeline.DrawCurve(curve, bhColour, thickness);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.PolyCurve polycurve, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int thickness)
        {
            pipeline.DrawCurve(polycurve, bhColour, thickness);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.Polyline polyline, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int thickness)
        {
            pipeline.DrawPolyline(polyline, bhColour, thickness);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.PolylineCurve polyline, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int thickness)
        {
            RHG.Polyline poly;
            if (polyline.TryGetPolyline(out poly))
                pipeline.DrawPolyline(poly, bhColour, thickness);
        }

        /***************************************************/
        /**** Public Methods  - Surfaces                ****/
        /***************************************************/

        public static void RenderRhinoWires(RHG.Extrusion surface, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int thickness)
        {
            pipeline.DrawSurface(surface, bhColour, thickness);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.NurbsSurface surface, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int thickness)
        {
            pipeline.DrawSurface(surface, bhColour, thickness);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.Brep brep, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int thickness)
        {
            pipeline.DrawBrepWires(brep, bhColour, 0);
        }


        /***************************************************/
        /**** Public Methods - Solids                   ****/
        /***************************************************/

        public static void RenderRhinoWires(RHG.Cone cone, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int thickness)
        {
            pipeline.DrawCone(cone, bhColour);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.Cylinder cylinder, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int thickness)
        {
            pipeline.DrawCylinder(cylinder, bhColour);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.Sphere sphere, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int thickness)
        {
            pipeline.DrawSphere(sphere, bhColour);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.Torus torus, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int thickness)
        {
            pipeline.DrawTorus(torus, bhColour);
        }


        /***************************************************/
        /**** Public Methods  - Mesh                    ****/
        /***************************************************/

        public static void RenderRhinoWires(RHG.Mesh mesh, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int thickness)
        {
            if (mesh.VertexColors.Count == 0)
                pipeline.DrawMeshWires(mesh, bhColour);
            else if (GH.CentralSettings.PreviewMeshEdges)
                pipeline.DrawMeshWires(mesh, bhColour);
        }


        /***************************************************/
        /**** Public Methods  - Miscellanea             ****/
        /***************************************************/

        public static void RenderRhinoWires(RHG.BoundingBox bbBox, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int thickness)
        {
            pipeline.DrawBox(bbBox, bhColour, thickness);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.Box box, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int thickness)
        {
            pipeline.DrawBox(box, bhColour, thickness);
        }

        /***************************************************/
        /**** Public Methods  - Representations         ****/
        /***************************************************/

        public static void RenderRhinoWires(Text3d text3D, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int thickness)
        {

            pipeline.Draw3dText(text3D, bhColour, text3D.TextPlane); 
        }

        /***************************************************/
    }
}







