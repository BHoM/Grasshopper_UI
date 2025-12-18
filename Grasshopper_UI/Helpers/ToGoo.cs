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

using BH.oM.Base;
using BH.oM.Geometry;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using GHK = Grasshopper.Kernel.Types;
using BH.UI.Grasshopper.Goos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BH.UI.Grasshopper
{
    public static partial class Helpers
    {
        /*******************************************/
        /**** Interface Methods                 ****/
        /*******************************************/

        public static IGH_Goo IToGoo(this object obj)
        {
            if (obj == null)
                return null;
            else
                return ToGoo(obj as dynamic);
        }

        /*************************************/

        public static List<IGH_Goo> IToGoo<T>(this List<T> list)
        {
            return list.Select(x => x.IToGoo()).ToList();
        }

        /*************************************/

        public static GH_Structure<IGH_Goo> IToGoo<T>(this List<List<T>> tree)
        {
            GH_Structure<IGH_Goo> structure = new GH_Structure<IGH_Goo>();

            for (int i = 0; i < tree.Count; i++)
                structure.AppendRange(tree[i].IToGoo(), new GH_Path(i));

            return structure;
        }


        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public static IGH_Goo ToGoo(this Enum obj)
        {
            return new GH_Enum(obj as Enum);
        }

        /*************************************/

        public static IGH_Goo ToGoo(this IGeometry obj)
        {
            return new GH_IBHoMGeometry(obj);
        }

        /*************************************/

        public static IGH_Goo ToGoo(this IObject obj)
        {
            return new GH_IObject(obj);
        }

        /*************************************/

        public static IGH_Goo ToGoo(this BHoMObject obj)
        {
            return new GH_BHoMObject(obj);
        }

        /*************************************/

        public static IGH_Goo ToGoo(this Type obj)
        {
            return new GH_Type(obj);
        }

        /*************************************/

        public static IGH_Goo ToGoo(this IDictionary obj)
        {
            return new GH_Dictionary(obj);
        }

        /*************************************/

        public static IGH_Goo ToGoo(this bool obj)
        {
            return new GH_Boolean(obj);
        }

        /*************************************/

        public static IGH_Goo ToGoo(this System.Drawing.Color obj)
        {
            return new GH_Colour(obj);
        }

        /*************************************/

        public static IGH_Goo ToGoo(this System.Numerics.Complex obj)
        {
            return new GH_ComplexNumber(new GHK.Complex(obj.Real, obj.Imaginary));
        }

        /*************************************/

        public static IGH_Goo ToGoo(this DateTime obj)
        {
            return new GH_Time(obj);
        }

        /*************************************/

        public static IGH_Goo ToGoo(this double obj)
        {
            return new GH_Number(obj);
        }

        /*************************************/

        public static IGH_Goo ToGoo(this Guid obj)
        {
            return new GH_Guid(obj);
        }

        /*************************************/

        public static IGH_Goo ToGoo(this int obj)
        {
            return new GH_Integer(obj);
        }

        /*************************************/

        public static IGH_Goo ToGoo(this long obj)
        {
            int cast = (int)obj;
            if (obj != cast)
                BH.Engine.Base.Compute.RecordError("Grasshopper does not support 64-bit integers, casting to 32-bit resulted in integer overflow.");

            return new GH_Integer(cast);
        }

        /*************************************/

        public static IGH_Goo ToGoo(this string obj)
        {
            return new GH_String(obj);
        }

        /*************************************/

        public static IGH_Goo ToGoo(this object obj)
        {
            return null;
        }

        /*************************************/
    }
}






