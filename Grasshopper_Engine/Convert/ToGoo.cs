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

using BH.Engine.Grasshopper.Objects;
using BH.oM.Base;
using BH.oM.Geometry;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System;
using System.Collections;

namespace BH.Engine.Grasshopper
{
    public static partial class Convert
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


        /*******************************************/
        /**** Fallback case                     ****/
        /*******************************************/

        public static IGH_Goo ToGoo(this object obj)
        {
            return default(IGH_Goo);
        }


        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public static IGH_Goo ToGoo(this Enum obj)
        {
            return new EnumGoo(obj as Enum);
        }

        /*************************************/

        public static IGH_Goo ToGoo(this IGeometry obj)
        {
            return new IGeometryGoo(obj);
        }

        /*************************************/

        public static IGH_Goo ToGoo(this BHoMObject obj)
        {
            return new BHoMObjectGoo(obj);
        }

        /*************************************/

        public static IGH_Goo ToGoo(this Type obj)
        {
            return new TypeGoo(obj);
        }

        /*************************************/

        public static IGH_Goo ToGoo(this IDictionary obj)
        {
            return new DictionaryGoo(obj);
        }

        /*************************************/

        public static IGH_Goo ToGoo(this CustomObject obj)
        {
            return new DictionaryGoo(obj.CustomData);
        }

        /*************************************/
    }
}
