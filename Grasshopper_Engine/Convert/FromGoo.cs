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

using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using System.Runtime.CompilerServices;
using Rhino.Geometry;

namespace BH.Engine.Grasshopper
{
    public static partial class Convert
    {
        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public static T IFromGoo<T>(this IGH_Goo goo, IGH_TypeHint hint = null)
        {
            return goo != null ? FromGoo<T>(goo as dynamic, hint) : null;
        }

        public static T FromGoo<T>(this IGH_Goo goo, IGH_TypeHint hint = null)
        {
            if (goo == null)
                return default(T);

            // Get the data out of the Goo
            object data = goo.ScriptVariable();
            while (data is IGH_Goo)
                data = ((IGH_Goo)data).ScriptVariable();

            if (data == null)
                return default(T);

            // Convert the data to an acceptable format
            if (data is T)
            {
                return (T)data;
            }
            else if (hint != null)
            {
                object result;
                hint.Cast(RuntimeHelpers.GetObjectValue(data), out result);
                data = result;
            }

            if (data.GetType().Namespace.StartsWith("Rhino.Geometry"))
                data = BH.Engine.Rhinoceros.Convert.ToBHoM(data as dynamic);
            return (T)(data as dynamic);
        }

        /*************************************/

        public static T FromGoo<T>(this GH_Surface goo, IGH_TypeHint hint = null)
        {
            Brep brep = goo.ScriptVariable() as Brep;
            if (brep.IsSurface)
                return (T)(brep.Faces[0].UnderlyingSurface() as dynamic);
            return default(T);
        }
    }
}
