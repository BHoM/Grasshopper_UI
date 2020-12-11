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

using RHG = Rhino.Geometry;
using Rhino.Display;
using Rhino.DocObjects;
using Grasshopper.Kernel;
using System;
using System.Drawing;
using BH.oM.Reflection.Attributes;
using System.Collections.Generic;
using GH = Grasshopper;

namespace BH.UI.Grasshopper
{ 
    public static partial class Render
    {
        /***************************************************/
        /**** Public Methods  - Interfaces              ****/
        /***************************************************/

        public static void IRenderRhinoWires(this object geometry, GH_PreviewWireArgs args, Color custom, int weight)
        {
            if (geometry == null)
            {
                return;
            }
            Color bhColour = RenderColour(args.Color, custom);
            try
            {
                RenderRhinoWires(geometry as dynamic, args.Pipeline, bhColour, weight);
            }
            catch (Exception) { }
        }

        /***************************************************/

        public static void RenderRhinoWires(this List<object> geometry, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int weight)
        {
            foreach(object geo in geometry)
            {
                RenderRhinoWires(geo as dynamic, pipeline, bhColour, weight);
            }
        }


        /***************************************************/
        /**** Private Methods  - Fallback               ****/
        /***************************************************/

        private static void RenderRhinoWires(this object fallback, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int weight)
        {
            // fallback in case no method is found for the provided runtime type
            return;
        }


        /***************************************************/
        /**** Public Methods  - Vectors                 ****/
        /***************************************************/

        public static void RenderRhinoWires(RHG.Point3d point, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int weight)
        {
            pipeline.DrawPoint(point, bhColour);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.Plane plane, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int weight)
        {
            pipeline.DrawConstructionPlane(new Rhino.DocObjects.ConstructionPlane() { Plane = plane, ThickLineColor = bhColour, ThinLineColor = Color.Black, GridLineCount = 10 });
        }


        /***************************************************/
        /**** Public Methods  - Curves                  ****/
        /***************************************************/

        public static void RenderRhinoWires(RHG.Arc arc, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int weight)
        {
            pipeline.DrawArc(arc, bhColour, weight);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.ArcCurve arc, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int weight)
        {
            pipeline.DrawArc(arc.Arc, bhColour, weight);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.Circle circle, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int weight)
        {
            pipeline.DrawCircle(circle, bhColour, weight);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.Curve curve, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int weight)
        {
            pipeline.DrawCurve(curve, bhColour, weight);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.Ellipse ellipse, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int weight)
        {
            pipeline.DrawCurve(ellipse.ToNurbsCurve(), bhColour, weight);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.Line line, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int weight)
        {
            pipeline.DrawLine(line, bhColour, weight);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.LineCurve line, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int weight)
        {
            pipeline.DrawLine(line.Line, bhColour, weight);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.NurbsCurve curve, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int weight)
        {
            pipeline.DrawCurve(curve, bhColour, weight);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.PolyCurve polycurve, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int weight)
        {
            pipeline.DrawCurve(polycurve, bhColour, weight);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.Polyline polyline, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int weight)
        {
            pipeline.DrawPolyline(polyline, bhColour, weight);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.PolylineCurve polyline, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int weight)
        {
            RHG.Polyline poly;
            if (polyline.TryGetPolyline(out poly))
                pipeline.DrawPolyline(poly, bhColour, weight);
        }


        /***************************************************/
        /**** Public Methods  - Surfaces                ****/
        /***************************************************/

        public static void RenderRhinoWires(RHG.Extrusion surface, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int weight)
        {
            pipeline.DrawSurface(surface, bhColour, weight);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.NurbsSurface surface, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int weight)
        {
            pipeline.DrawSurface(surface, bhColour, weight);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.Brep brep, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int weight)
        {
            pipeline.DrawBrepWires(brep, bhColour, 0);
        }


        /***************************************************/
        /**** Public Methods - Solids                   ****/
        /***************************************************/

        public static void RenderRhinoWires(RHG.Cone cone, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int weight)
        {
            pipeline.DrawCone(cone, bhColour);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.Cylinder cylinder, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int weight)
        {
            pipeline.DrawCylinder(cylinder, bhColour);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.Sphere sphere, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int weight)
        {
            pipeline.DrawSphere(sphere, bhColour);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.Torus torus, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int weight)
        {
            pipeline.DrawTorus(torus, bhColour);
        }


        /***************************************************/
        /**** Public Methods  - Mesh                    ****/
        /***************************************************/

        public static void RenderRhinoWires(RHG.Mesh mesh, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int weight)
        {
            if (mesh.VertexColors.Count == 0)
                pipeline.DrawMeshWires(mesh, bhColour);
            else if (GH.CentralSettings.PreviewMeshEdges)
                pipeline.DrawMeshWires(mesh, bhColour);
        }


        /***************************************************/
        /**** Public Methods  - Miscellanea             ****/
        /***************************************************/

        public static void RenderRhinoWires(RHG.BoundingBox bbBox, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int weight)
        {
            pipeline.DrawBox(bbBox, bhColour, weight);
        }

        /***************************************************/

        public static void RenderRhinoWires(Text3d text3d, Rhino.Display.DisplayPipeline pipeline, Color bhColour, int weight)
        {
            pipeline.Draw3dText(text3d, bhColour);
        }

        /***************************************************/
    }
}

