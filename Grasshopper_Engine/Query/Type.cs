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

using BH.UI.Components;
using BH.UI.Templates;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BH.Engine.Grasshopper
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Fields                             ****/
        /***************************************************/

        public static Type Type(this IGH_Param param, Caller caller = null)
        {
            if (param == null)
                return typeof(object);

            if (param is Param_ScriptVariable)
                return Type(((Param_ScriptVariable)param).TypeHint, param.Access);

            else if (caller != null && caller is CreateCustomCaller)
                return ((CreateCustomCaller)caller).GetParam(param.NickName).DataType;

            else
                return param.Type;
        }

        /***************************************************/

        public static Type Type(this IGH_TypeHint hint, GH_ParamAccess access)
        {
            if (hint == null)
                return typeof(object);

            switch (hint.TypeName)
            {
                case "null":
                    return Type<object>(access);
                case "BH.oM.Base.BHoMObject":
                    return Type<BH.oM.Base.BHoMObject>(access);
                case "BH.oM.Geometry.IGeometry":
                    return Type<BH.oM.Geometry.IGeometry>(access);
                case "Dictionary":
                    return Type<IDictionary>(access);
                case "System.Enum":
                    return Type<System.Enum>(access);
                case "System.Type":
                    return Type<Type>(access);
                case "bool":
                    return Type<bool>(access);
                case "int":
                    return Type<int>(access);
                case "double":
                    return Type<double>(access);
                case "string":
                    return Type<string>(access);
                case "DateTime":
                    return Type<DateTime>(access);
                case "Color":
                    return Type<System.Drawing.Color>(access);
                case "Guid":
                    return Type<Guid>(access);
                default:
                    return Type<object>(access);
            }
        }

        /***************************************************/

        public static Type Type<T>(GH_ParamAccess access)
        {
            switch (access)
            {
                default:
                    return typeof(T);
                case GH_ParamAccess.list:
                    return typeof(List<T>);
                case GH_ParamAccess.tree:
                    return typeof(List<List<T>>);
            }
        }

        /***************************************************/
    }
}
