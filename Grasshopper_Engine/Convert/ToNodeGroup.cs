/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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

            List<Guid> nodeIds = components.Select(x => x.InstanceGuid).ToList();
            List<NodeGroup> internalNodeGroups = internalGroups.Select(x => x.ToNodeGroup()).Where(x => x != null).ToList();
            string description = comments.Count == 1 ? comments.First().Text : "";

            // Provide a warning if a group has more than one description
            if (comments.Count > 1)
            {
                List<string> internalDescriptions = GetChildDescriptions(internalNodeGroups);
                List<string> exclusiveDescriptions = comments.Select(x => x.Text).Except(internalDescriptions).ToList();

                if (exclusiveDescriptions.Count == 1)
                {
                    description = exclusiveDescriptions.First();
                    string message = "A group directly contains descriptions that are also contained by its sub-groups. Those descriptions will be ignored.";
                    if (description.Length > 0)
                        message += "\nThat group description is " + description;
                    Base.Compute.RecordWarning(message);
                }
                else if (exclusiveDescriptions.Count > 1)
                {
                    string message = "A group contains more than one description. No description will be provided for that group. Descriptions found:";
                    message += comments.Select(x => "\n - " + x.Text).Aggregate((a, b) => a + b);
                    Base.Compute.RecordWarning(message);
                }
                else
                {
                    string message = "A group contains no description that is not also in a sub-group. No description will be provided for that group. Descriptions found:";
                    message += comments.Select(x => "\n - " + x.Text).Aggregate((a, b) => a + b);
                    Base.Compute.RecordWarning(message);
                }
            }

            // Make sure there is no component also contained in sub-groups
            List<Guid> childNodeIds = GetChildNodeIds(internalNodeGroups);
            List<Guid> safeNodeIds = nodeIds.Except(childNodeIds).ToList();
            if (safeNodeIds.Count < nodeIds.Count)
            {
                nodeIds = safeNodeIds;
                string message = "A group directly contains elements that are also contained by its sub-groups. Those elements will only be part of the sub-group.";
                if (description.Length > 0)
                    message += "\nThat group description is " + description;
                Base.Compute.RecordWarning(message);
            }

            return new NodeGroup
            {
                Description = description,
                NodeIds = nodeIds,
                InternalGroups = internalNodeGroups
            };
        }

        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        private static List<Guid> GetChildNodeIds(List<NodeGroup> groups)
        {
            if (groups.Count == 0)
                return new List<Guid>();
            else
                return groups.SelectMany(x => x.NodeIds.Concat(GetChildNodeIds(x.InternalGroups))).ToList();
        }

        /*******************************************/

        private static List<string> GetChildDescriptions(List<NodeGroup> groups)
        {
            if (groups.Count == 0)
                return new List<string>();
            else
                return groups.SelectMany(x =>
                {
                    List<string> descriptions = GetChildDescriptions(x.InternalGroups);
                    if (x.Description.Length > 0)
                        descriptions.Add(x.Description);
                    return descriptions;
                }).ToList();
        }

        /*******************************************/
    }
}


