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

using BH.UI.Grasshopper.Properties;
using BH.UI.Grasshopper.Templates;
using Grasshopper.Kernel;
using System;
using Rhino;
using Rhino.DocObjects;
using System.Collections.Generic;
using BH.UI.Grasshopper.Goos;

namespace BH.UI.Grasshopper.Parameters
{
    public class Param_BHoMGeometry : BakeableParam<GH_IBHoMGeometry>
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Icon { get; } = Resources.IBHoMGeometry_Param;

        public override Guid ComponentGuid { get; } = new Guid("EFD86C1F-D674-4905-A660-28C81A807080");

        public override string TypeName { get; } = "BHoM Geometry";


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public Param_BHoMGeometry() : base("BHoM geometry", "BHoMGeo", "Represents a collection of generic BHoM geometries", "Params", "Geometry")
        {
        }

        /*******************************************/
    }
}


