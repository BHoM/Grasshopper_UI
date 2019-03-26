﻿/*
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

using RHG = Rhino.Geometry;
using Rhino.Display;
using Grasshopper.Kernel;
using System;
using System.Drawing;
using BH.oM.Reflection.Attributes;

namespace BH.Engine.Grasshopper
{
    public static partial class Compute
    {
        /***************************************************/
        /**** Public Methods  - Interfaces              ****/
        /***************************************************/

        public static void IRenderRhinoWires(this object geometry, GH_PreviewWireArgs args)
        {
            if (geometry == null)
            {
                return;
            }
            Color bhColour = Query.RenderColour(args.Color);
            try
            {
                RenderRhinoWires(geometry as dynamic, args.Pipeline, bhColour);
            }
            catch (Exception) { }
        }


        /***************************************************/
        /**** Public Methods  - Fallback                ****/
        /***************************************************/

        [NotImplemented]
        public static void RenderRhinoWires(this object fallback, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            // fallback in case no method is found for the provided runtime type
            return;
        }


        /***************************************************/
        /**** Public Methods  - Vectors                 ****/
        /***************************************************/

        public static void RenderRhinoWires(RHG.Point point, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            return;
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.Vector3d vector, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            //args.Pipeline.DrawLineArrow(vector.ToRhino(), args.Color, args.Thickness, args.Thickness);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.Plane plane, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawConstructionPlane(new Rhino.DocObjects.ConstructionPlane() { Plane = plane, ThickLineColor = bhColour, ThinLineColor = Color.Black, GridLineCount = 10 });
        }


        /***************************************************/
        /**** Public Methods  - Curves                  ****/
        /***************************************************/

        public static void RenderRhinoWires(RHG.Arc arc, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawArc(arc, bhColour, 2);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.ArcCurve arc, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawArc(arc.Arc, bhColour, 2);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.Circle circle, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawCircle(circle, bhColour, 2);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.Curve curve, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawCurve(curve, bhColour, 2);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.Ellipse ellipse, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawCurve(ellipse.ToNurbsCurve(), bhColour, 2);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.Line line, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawLine(line, bhColour, 2);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.LineCurve line, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawLine(line.Line, bhColour, 2);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.NurbsCurve curve, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawCurve(curve, bhColour, 2);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.PolyCurve polycurve, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawCurve(polycurve, bhColour, 2);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.Polyline polyline, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawPolyline(polyline, bhColour, 2);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.PolylineCurve polyline, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            RHG.Polyline poly;
            if (polyline.TryGetPolyline(out poly))
                pipeline.DrawPolyline(poly, bhColour, 2);
        }


        /***************************************************/
        /**** Public Methods  - Surfaces                ****/
        /***************************************************/

        public static void RenderRhinoWires(RHG.Extrusion surface, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawSurface(surface, bhColour, 2);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.NurbsSurface surface, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawSurface(surface, bhColour, 2);
        }

        /***************************************************/

        public static void RenderRhinoWires(RHG.Brep brep, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawBrepWires(brep, bhColour, 0);
        }


        /***************************************************/
        /**** Public Methods  - Mesh                    ****/
        /***************************************************/

        public static void RenderRhinoWires(RHG.Mesh mesh, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawMeshWires(mesh, bhColour);
        }


        /***************************************************/
        /**** Public Methods  - Miscellanea             ****/
        /***************************************************/

        public static void RenderRhinoWires(RHG.BoundingBox bbBox, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawBox(bbBox, bhColour, 2);
        }

        /***************************************************/
    }
}