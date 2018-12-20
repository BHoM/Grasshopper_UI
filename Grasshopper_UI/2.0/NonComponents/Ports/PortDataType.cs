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

using BH.oM.Base;
using Grasshopper.Kernel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.UI.Grasshopper.Base.NonComponents.Ports
{
    public class PortDataType
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public GH_ParamAccess AccessMode { get; set; } = GH_ParamAccess.item;

        public Type DataType { get; set; } = typeof(object);



        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public PortDataType() { }

        /*******************************************/

        public PortDataType(Type type)
        {
            if (typeof(IDictionary).IsAssignableFrom(type))
            {
                AccessMode = GH_ParamAccess.item;
                DataType = type;
            }
            else if (type != typeof(string) && (typeof(IEnumerable).IsAssignableFrom(type)))
            {
                AccessMode = GH_ParamAccess.list;
                Type subType = null;
                if (type.GenericTypeArguments.Length > 0)
                    subType = type.GenericTypeArguments.First();
                else if (type.HasElementType)
                    subType = type.GetElementType();

                if (subType != null)
                { 
                    PortDataType subDataType = new PortDataType(subType);
                    if (subDataType.AccessMode != GH_ParamAccess.item)
                        AccessMode = GH_ParamAccess.tree;
                    DataType = subDataType.DataType;  
                }
            }
            else
            {
                if (type.IsGenericParameter)
                {
                    type = GetTypeFromGenericParameters(type);
                }
                else if (type.ContainsGenericParameters)
                {
                    Type[] newTypes = type.GetGenericArguments().Select(x => GetTypeFromGenericParameters(x)).ToArray();
                    type = type.GetGenericTypeDefinition().MakeGenericType(newTypes);
                }

                AccessMode = GH_ParamAccess.item;
                DataType = type;
            }
        }

        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public static Type GetTypeFromGenericParameters(Type type)
        {
            Type[] constrains = type.GetGenericParameterConstraints();
            if (constrains.Length == 0)
                return typeof(object);
            else
                return  constrains[0];
        }

        /*******************************************/
    }
}
