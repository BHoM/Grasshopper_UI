/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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

using GH = Grasshopper;
using System.Drawing;
using Rhino.Display;

namespace BH.UI.Grasshopper
{
    public static partial class Render
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static DisplayMaterial RenderMaterial(DisplayMaterial material, Color custom)
        {
            Color pColour = GH.Instances.ActiveCanvas.Document.PreviewColour;
            Color ghColour = material.Diffuse;
            if (ghColour.R == pColour.R & // If the color sent by PreviewArgs is the default object PreviewColour
                ghColour.G == pColour.G &
                ghColour.B == pColour.B) // Excluding Alpha channel from comparison
            {
                double transparency = (255 - custom.A) / (double)255;
                return new DisplayMaterial(custom, transparency);
            }
            else
            {
                return material;
            }
        }

        /***************************************************/

        public static DisplayMaterial RenderMaterial(DisplayMaterial material, DisplayMaterial custom)
        {
            if (custom == null)
                return material;

            Color pColour = GH.Instances.ActiveCanvas.Document.PreviewColour;
            Color ghColour = material.Diffuse;
            if (ghColour.R == pColour.R & // If the color sent by PreviewArgs is the default object PreviewColour
                ghColour.G == pColour.G &
                ghColour.B == pColour.B) // Excluding Alpha channel from comparison
            {
                return custom;
            }
            else
            {
                return material;
            }
        }

        /***************************************************/
    }
}



