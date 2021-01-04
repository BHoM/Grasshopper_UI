/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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

using BH.Engine.Rhinoceros;
using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Programming;
using System.Reflection;

namespace BH.Engine.Grasshopper
{
    public static partial class Convert
    {
        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public static INodeParam ToNodeParam(this IGH_Param param, Guid parentId)
        {
            switch (param.Kind)
            {
                case GH_ParamKind.input:
                    return ToReceiverParam(param, parentId);
                case GH_ParamKind.output:
                    return ToDataParam(param, parentId);
                default:
                    return null;
            }
        }

        /*******************************************/

        public static ReceiverParam ToReceiverParam(this IGH_Param param, Guid parentId)
        {
            return new ReceiverParam
            {
                Name = param.NickName,
                Description = param.Description,
                SourceId = param.SourceCount == 0 ? Guid.Empty : param.Sources.First().InstanceGuid,
                BHoM_Guid = param.InstanceGuid,
                ParentId = parentId
            };
        }

        /*******************************************/

        public static DataParam ToDataParam(this IGH_Param param, Guid parentId)
        {
            return new DataParam
            {
                Name = param.NickName,
                Description = param.Description,
                BHoM_Guid = param.InstanceGuid,
                TargetIds = param.Recipients.Select(r => r.InstanceGuid).ToList(),
                ParentId = parentId
            };
        }

        /*******************************************/
    }
}

