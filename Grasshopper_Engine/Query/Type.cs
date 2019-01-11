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

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using System;
using System.Collections;

namespace BH.Engine.Grasshopper
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Fields                             ****/
        /***************************************************/

        public static Type Type(this IGH_TypeHint hint)
        {
            switch (hint.TypeName)
            {
                case "null":
                    return typeof(object);
                case "BH.oM.Base.BHoMObject":
                    return typeof(BH.oM.Base.BHoMObject);
                case "BH.oM.Geometry.IGeometry":
                    return typeof(BH.oM.Geometry.IGeometry);
                case "Dictionary":
                    return typeof(IDictionary);
                case "System.Enum":
                    return typeof(System.Enum);
                case "System.Type":
                    return typeof(Type);
                case "bool":
                    return typeof(bool);
                case "int":
                    return typeof(int);
                case "double":
                    return typeof(double);
                case "string":
                    return typeof(string);
                case "DateTime":
                    return typeof(DateTime);
                case "Color":
                    return typeof(System.Drawing.Color);
                case "Guid":
                    return typeof(Guid);
                default:
                    return typeof(object);
            }
        }

        /***************************************************/
    }
}
