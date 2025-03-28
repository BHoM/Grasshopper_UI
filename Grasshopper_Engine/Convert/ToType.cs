/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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

using BH.Engine.Adapters.Rhinoceros;
using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Grasshopper
{
    public static partial class Convert
    {
        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public static Type ToType(this string typeString)
        {
            // Try to get teh type from assembly name
            Type type = Type.GetType(typeString);
            if (type != null)
                return type;

            if (typeString.Contains("[[") && typeString.Contains("]]"))
            {
                int outerSplit = typeString.IndexOf("[[");
                int innerLength = typeString.LastIndexOf("]]") - outerSplit - 2;

                Type outer = typeString.Substring(0, outerSplit).ToType();
                Type inner = typeString.Substring(outerSplit + 2, innerLength).ToType();

                return outer.MakeGenericType(new Type[] { inner });
            }

            // Try get to recover type from full name
            string[] parts = typeString.Split(new char[] { ',' });
            if (parts.Length == 0)
                return null;
            else
            {
                try
                {
                    type = BH.Engine.Base.Create.Type(parts[0]);
                }
                catch
                {
                    type = null;
                }
                return type;
            }
        }

        /*******************************************/
    }
}






