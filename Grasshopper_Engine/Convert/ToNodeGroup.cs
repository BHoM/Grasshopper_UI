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
using Grasshopper.Kernel.Special;

namespace BH.Engine.Grasshopper
{
    public static partial class Convert
    {
        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public static NodeGroup ToNodeGroup(this GH_Group group)
        {
            List<IGH_DocumentObject> content = group.Objects();

            List<GH_Group> internalGroups = content.OfType<GH_Group>().ToList();
            content = content.Except(internalGroups.SelectMany(x => x.Objects())).ToList();

            List<GH_Scribble> comments = content.OfType<GH_Scribble>().ToList();
            List<GH_ActiveObject> components = content.OfType<GH_ActiveObject>().ToList();

            return new NodeGroup
            {
                Description = comments.Count > 0 ? comments.First().Text : "",
                NodeIds = components.Select(x => x.InstanceGuid).ToList(),
                InternalGroups = internalGroups.Select(x => x.ToNodeGroup()).ToList()
            };
        }

        /*******************************************/
    }
}
