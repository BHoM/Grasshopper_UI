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

using BH.UI.Grasshopper.Properties;
using BH.UI.Grasshopper.Templates;
using Grasshopper.Kernel;
using System;
using Rhino;
using Rhino.DocObjects;
using System.Collections.Generic;

namespace BH.UI.Grasshopper.Parameters
{
    public class Param_BHoMObject : BakeableParam<Engine.Grasshopper.Objects.GH_BHoMObject>
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Icon { get; } = Resources.BHoMObject_Param;

        public override Guid ComponentGuid { get; } = new Guid("1DEF3710-FD5B-4617-BCF6-B6293C5C6530");

        public override string TypeName { get; } = "BHoM Object";


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public Param_BHoMObject() : base("BHoM object", "BHoM", "Represents a collection of generic BHoM objects", "Params", "Primitive")
        {
        }

        /*******************************************/

    }

}

