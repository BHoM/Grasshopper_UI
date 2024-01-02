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

using RHG = Rhino.Geometry;
using Rhino.Display;
using Grasshopper.Kernel;
using System;
using BH.oM.Base.Attributes;
using System.Collections.Generic;
using BH.oM.Geometry;
using System.Drawing;
using BH.Engine.Rhinoceros;

namespace BH.UI.Grasshopper
{
    public static partial class Render
    {
        /***************************************************/
        /**** Public Methods  - Interfaces              ****/
        /***************************************************/

        public static RHG.Mesh ICreatePreviewMesh(this object obj, RHG.MeshingParameters parameters)
        {
            if (obj == null)
                return null;

            if (parameters == null)
                parameters = RHG.MeshingParameters.Default;

            try
            {
                return CreatePreviewMesh(obj as dynamic, parameters);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /***************************************************/

        public static RHG.Mesh CreatePreviewMesh(this List<object> geometry, RHG.MeshingParameters parameters)
        {
            RHG.Mesh mesh = new RHG.Mesh();
            foreach (object geo in geometry)
            {
                RHG.Mesh itemMesh = CreatePreviewMesh(geo as dynamic, parameters);
                if (itemMesh != null)
                    mesh.Append(itemMesh);
            }
            return mesh;
        }


        /***************************************************/
        /**** Private Methods  - Fallback               ****/
        /***************************************************/

        private static RHG.Mesh CreatePreviewMesh(this object fallback, RHG.MeshingParameters parameters)
        {
            // fallback in case no method is found for the provided runtime type
            return null;
        }


        /***************************************************/
        /**** Public Methods  - Surfaces                ****/
        /***************************************************/

        public static RHG.Mesh CreatePreviewMesh(RHG.Extrusion surface, RHG.MeshingParameters parameters)
        {
            return CreatePreviewMesh(surface.ToBrep(), parameters);
        }

        /***************************************************/

        public static RHG.Mesh CreatePreviewMesh(RHG.NurbsSurface surface, RHG.MeshingParameters parameters)
        {
            return CreatePreviewMesh(surface.ToBrep(), parameters);
        }

        /***************************************************/

        public static RHG.Mesh CreatePreviewMesh(RHG.Brep brep, RHG.MeshingParameters parameters)
        {
            RHG.Mesh[] array = RHG.Mesh.CreateFromBrep(brep, parameters);
            if (array == null)
                return new RHG.Mesh();

            if (array.Length == 1)
                return array[0];

            RHG.Mesh mesh = new RHG.Mesh();
            foreach (var faceMesh in array)
            {
                mesh.Append(faceMesh);
            }
            return mesh;
        }

        /***************************************************/

        public static RHG.Mesh CreatePreviewMesh(RHG.Surface surface, RHG.MeshingParameters parameters)
        {
            return CreatePreviewMesh(surface.ToBrep(), parameters);
        }


        /***************************************************/
        /**** Public Methods - Solids                   ****/
        /***************************************************/

        public static RHG.Mesh CreatePreviewMesh(RHG.Cone cone, RHG.MeshingParameters parameters)
        {
            return CreatePreviewMesh(cone.ToBrep(true), parameters);
        }

        /***************************************************/

        public static RHG.Mesh CreatePreviewMesh(RHG.Cylinder cylinder, RHG.MeshingParameters parameters)
        {
            return CreatePreviewMesh(cylinder.ToBrep(cylinder.IsFinite, cylinder.IsFinite), parameters);
        }

        /***************************************************/

        public static RHG.Mesh CreatePreviewMesh(RHG.Sphere sphere, RHG.MeshingParameters parameters)
        {
            return CreatePreviewMesh(sphere.ToBrep(), parameters);
        }

        /***************************************************/

        public static RHG.Mesh CreatePreviewMesh(RHG.Torus torus, RHG.MeshingParameters parameters)
        {
            return CreatePreviewMesh(torus.ToRevSurface().ToBrep(), parameters);
        }

        /***************************************************/

        public static RHG.Mesh CreatePreviewMesh(RHG.Box box, RHG.MeshingParameters parameters)
        {
            return CreatePreviewMesh(box.ToBrep(), parameters);
        }

        /***************************************************/
        /**** Public Methods  - Mesh                    ****/
        /***************************************************/

        public static RHG.Mesh CreatePreviewMesh(RHG.Mesh mesh, RHG.MeshingParameters parameters)
        {
            return mesh;
        }


        /***************************************************/
        /**** Public Methods  - Miscellanea             ****/
        /***************************************************/

        public static RHG.Mesh CreatePreviewMesh(RHG.BoundingBox bbBox, RHG.MeshingParameters parameters)
        {
            return CreatePreviewMesh(bbBox.ToBrep(), parameters);
        }

        /***************************************************/
    }
}





