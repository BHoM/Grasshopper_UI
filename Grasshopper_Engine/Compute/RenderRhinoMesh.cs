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

using RHG = Rhino.Geometry;
using Rhino.Display;
using Grasshopper.Kernel;
using System;
using BH.oM.Reflection.Attributes;
using System.Collections.Generic;

namespace BH.Engine.Grasshopper
{
    public static partial class Compute
    {
        /***************************************************/
        /**** Public Methods  - Interfaces              ****/
        /***************************************************/

        public static void IRenderRhinoMeshes(this object geometry, GH_PreviewMeshArgs args)
        {
            if (geometry == null)
            {
                return;
            }
            DisplayMaterial bhMaterial = Query.RenderMaterial(args.Material);

            try
            {
                RenderRhinoMeshes(geometry as dynamic, args.Pipeline, bhMaterial);
            }
            catch (Exception) { }
        }

        /***************************************************/

        public static void IRenderRhinoMeshes(this List<object> geometry, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            foreach(object geo in geometry)
            {
                IRenderRhinoMeshes(geo as dynamic, pipeline, material);
            }
        }


        /***************************************************/
        /**** Public Methods  - Fallback                ****/
        /***************************************************/

        [NotImplemented]
        public static void RenderRhinoMeshes(this object fallback, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            // fallback in case no method is found for the provided runtime type
            return;
        }


        /***************************************************/
        /**** Public Methods  - Vectors                 ****/
        /***************************************************/

        public static void RenderRhinoMeshes(RHG.Point3d point, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            return;
        }

        /***************************************************/

        public static void RenderRhinoMeshes(RHG.Vector3d vector, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            return;
        }

        /***************************************************/

        public static void RenderRhinoMeshes(RHG.Plane plane, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            return;
        }


        /***************************************************/
        /**** Public Methods  - Curves                  ****/
        /***************************************************/

        public static void RenderRhinoMeshes(RHG.Arc arc, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            return;
        }

        /***************************************************/

        public static void RenderRhinoMeshes(RHG.ArcCurve arc, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            return;
        }


        /***************************************************/

        public static void RenderRhinoMeshes(RHG.Circle circle, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            return;
        }

        /***************************************************/

        public static void RenderRhinoMeshes(RHG.Curve curve, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            return;
        }

        /***************************************************/

        public static void RenderRhinoMeshes(RHG.Ellipse ellipse, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            return;
        }

        /***************************************************/

        public static void RenderRhinoMeshes(RHG.Line line, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            return;
        }

        /***************************************************/

        public static void RenderRhinoMeshes(RHG.LineCurve line, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            return;
        }
        /***************************************************/

        public static void RenderRhinoMeshes(RHG.NurbsCurve curve, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            return;
        }

        /***************************************************/

        public static void RenderRhinoMeshes(RHG.PolyCurve polycurve, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            return;
        }

        /***************************************************/

        public static void RenderRhinoMeshes(RHG.Polyline polyline, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            return;
        }

        /***************************************************/

        public static void RenderRhinoMeshes(RHG.PolylineCurve polyline, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            return;
        }


        /***************************************************/
        /**** Public Methods  - Surfaces                ****/
        /***************************************************/

        public static void RenderRhinoMeshes(RHG.Extrusion surface, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            pipeline.DrawBrepShaded(surface.ToBrep(), material);
        }

        /***************************************************/

        public static void RenderRhinoMeshes(RHG.NurbsSurface surface, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            pipeline.DrawBrepShaded(surface.ToBrep(), material);
        }

        /***************************************************/

        public static void RenderRhinoMeshes(RHG.Brep brep, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            pipeline.DrawBrepShaded(brep, material);
        }

        /***************************************************/

        public static void RenderRhinoMeshes(RHG.Surface surface, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            pipeline.DrawBrepShaded(surface.ToBrep(), material);
        }


        /***************************************************/
        /**** Public Methods  - Mesh                    ****/
        /***************************************************/

        public static void RenderRhinoMeshes(RHG.Mesh mesh, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            pipeline.DrawMeshShaded(mesh, material);
        }


        /***************************************************/
        /**** Public Methods  - Miscellanea             ****/
        /***************************************************/

        public static void RenderRhinoMeshes(RHG.BoundingBox bbBox, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            pipeline.DrawBrepShaded(bbBox.ToBrep(), material);
        }

        /***************************************************/
    }
}
