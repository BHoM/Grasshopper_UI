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

using BH.Engine.Rhinoceros;
using BHG = BH.oM.Geometry;
using RHG = Rhino.Geometry;
using Grasshopper.Kernel;
using System;
using System.Drawing;
using System.Linq;

namespace BH.UI.Grasshopper
{
    public static partial class Render
    {
        /***************************************************/
        /**** Public Methods  - Interfaces              ****/
        /***************************************************/

        public static void IRenderBHoMGeometry(this BHG.IGeometry geometry, GH_PreviewWireArgs args)
        {
            if (geometry == null) { return; }
            args.Pipeline.ZBiasMode = 0;
            Color bhColour = GetBHColor(args.Color);
            try
            {
                RenderBHoMGeometry(geometry as dynamic, args.Pipeline, bhColour);
            }
            catch (Exception) { }
        }

        /***************************************************/

        public static void IRenderBHoMGeometry(this BHG.IGeometry geometry, IGH_PreviewArgs args)
        {
            if (geometry == null) { return; }
            args.Display.ZBiasMode = 0;
            Color bhColour = GetBHColor(args.WireColour);
            try
            {
                RenderBHoMGeometry(geometry as dynamic, args.Display, bhColour);
            }
            catch (Exception) { }
        }


        /***************************************************/
        /**** Public Methods  - Vectors                 ****/
        /***************************************************/

        public static void RenderBHoMGeometry(BHG.Point point, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawPoint(point.ToRhino(), Rhino.Display.PointStyle.Simple, 4, bhColour);
        }

        /***************************************************/

        public static void RenderBHoMGeometry(BHG.Vector vector, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            //args.Pipeline.DrawLineArrow(vector.ToRhino(), args.Color, args.Thickness, args.Thickness);
        }

        /***************************************************/

        public static void RenderBHoMGeometry(BHG.Plane plane, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawConstructionPlane(new Rhino.DocObjects.ConstructionPlane() { Plane = (plane.ToRhino()), ThickLineColor = bhColour, ThinLineColor = Color.Black, GridLineCount = 10 });
        }


        /***************************************************/
        /**** Public Methods  - Curves                  ****/
        /***************************************************/

        public static void RenderBHoMGeometry(BHG.Arc arc, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawArc(arc.ToRhino(), bhColour, 2);
        }

        /***************************************************/

        public static void RenderBHoMGeometry(BHG.Circle circle, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawCircle(circle.ToRhino(), bhColour, 2);
        }

        /***************************************************/

        public static void RenderBHoMGeometry(BHG.Ellipse ellipse, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawCurve(ellipse.ToRhino().ToNurbsCurve(), bhColour, 2);
        }

        /***************************************************/

        public static void RenderBHoMGeometry(BHG.Line line, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawLine(line.ToRhino(), bhColour, 2);
        }
        /***************************************************/

        public static void RenderBHoMGeometry(BHG.NurbsCurve curve, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawCurve(curve.ToRhino(), bhColour, 2);
        }

        /***************************************************/

        public static void RenderBHoMGeometry(BHG.PolyCurve polycurve, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawCurve(polycurve.ToRhino(), bhColour, 2);
        }

        /***************************************************/

        public static void RenderBHoMGeometry(BHG.Polyline polyline, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawPolyline(polyline.ControlPoints.Select(x => x.ToRhino()), bhColour, 2);
        }


        /***************************************************/
        /**** Public Methods  - Surfaces                ****/
        /***************************************************/

        public static void RenderBHoMGeometry(BHG.BoundingBox bbBox, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawBox(bbBox.ToRhino(), bhColour, 2);
        }

        /***************************************************/

        public static void RenderBHoMGeometry(BHG.NurbsSurface surface, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            pipeline.DrawSurface(surface.ToRhino(), bhColour, 1);
        }

        /***************************************************/

        public static void RenderBHoMGeometry(BHG.PolySurface polySurface, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            polySurface.Surfaces.ForEach(srf => pipeline.DrawSurface(srf.IToRhino(), bhColour, 1));
        }


        /***************************************************/
        /**** Public Methods  - Mesh                    ****/
        /***************************************************/

        public static void RenderBHoMGeometry(BHG.Mesh mesh, GH_PreviewMeshArgs args)
        {
            RHG.Mesh rMesh = mesh.ToRhino();
            args.Pipeline.DrawMeshWires(rMesh, args.Material.Diffuse);
            args.Pipeline.DrawMeshShaded(rMesh, args.Material);
        }

        /***************************************************/

        public static void RenderBHoMGeometry(BHG.Mesh mesh, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            RHG.Mesh rMesh = mesh.ToRhino();
            pipeline.DrawMeshWires(rMesh, bhColour);
            pipeline.DrawMeshShaded(rMesh, new Rhino.Display.DisplayMaterial(bhColour));
        }

        /***************************************************/
        /**** Public Methods  - Miscellanea             ****/
        /***************************************************/

        public static void RenderBHoMGeometry(BHG.CompositeGeometry composite, Rhino.Display.DisplayPipeline pipeline, Color bhColour)
        {
            if (composite.Elements.Count == 0) { return; }
            foreach (BHG.IGeometry geom in composite.Elements)
            {
                try
                {
                    RenderBHoMGeometry(geom as dynamic, pipeline, bhColour);
                }
                catch (Exception) { }
            }
        }


        /***************************************************/
        /**** Private Methods  -                        ****/
        /***************************************************/

        private static Color GetBHColor(Color ghColor)
        {
            int R = ghColor.R - 59;
            int G = ghColor.G + 168;
            int B = ghColor.B;
            return Color.FromArgb(100, R < 255 && R > 0 ? R : 0, G < 255 && G > 0 ? G : 255, B);
        }

        /***************************************************/
    }
}
