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

using BH.Engine.Grasshopper;
using BH.Engine.Programming;
using BH.oM.Programming;
using BH.UI.Grasshopper.Templates;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BH.UI.Grasshopper.Components.UI
{
    public class Cluster2Node : GH_Component
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return Properties.Resources.ClusterToNode; } }

        public override Guid ComponentGuid { get { return new Guid("FCB6D011-F297-4D65-88B8-490B74AF8187"); } }

        public override GH_Exposure Exposure { get { return GH_Exposure.primary; } }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public Cluster2Node() : base("Cluster2Node", "Cluster2Node", "Convert an input cluster into a BHoM ClusterNode", "BHoM", "UI") { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("cluster", "cluster", "cluster to be turned into a BHoM ClusterNode", GH_ParamAccess.tree);
        }

        /*******************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("node", "node", "Resulting BHoM cluster node", GH_ParamAccess.item);
        }

        /*******************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BH.Engine.Base.Compute.ClearCurrentEvents();

            try
            {
                // Get the input component
                IGH_Param source = Params.Input[0].Sources.FirstOrDefault();
                if (source == null)
                    return;
                IGH_DocumentObject component = source.Attributes.GetTopLevel.DocObject;
                GH_Cluster cluster = source.Attributes.GetTopLevel.DocObject as GH_Cluster;
                List<IGH_DocumentObject> content = cluster.Document("").Objects.ToList();

                List<GH_ClusterInputHook> inputs = content.OfType<GH_ClusterInputHook>().ToList();
                List<GH_ClusterOutputHook> outputs = content.OfType<GH_ClusterOutputHook>().ToList();
                List<GH_Component> components = content.OfType<GH_Component>().ToList();
                List<GH_Group> groups = content.OfType<GH_Group>().ToList();
                List<IGH_Param> parameters = content.OfType<IGH_Param>().ToList();

                groups = groups.Except(groups.SelectMany(x => x.Objects().OfType<GH_Group>())).ToList();

                ClusterContent nodeContent = new ClusterContent
                {
                    Name = cluster.NickName,
                    Inputs = inputs.Select(x => new DataParam {
                        Name = x.NickName,
                        Description = x.Description,
                        TargetIds = x.Recipients.Select(r => r.InstanceGuid).ToList(),
                        BHoM_Guid = x.InstanceGuid
                    }).ToList(),
                    Outputs = outputs.Select(x => new ReceiverParam {
                        Name = x.NickName,
                        Description = x.Description,
                        SourceId = x.Sources.First().InstanceGuid,
                        BHoM_Guid = x.InstanceGuid
                    }).ToList(),
                    InternalNodes = components.Select(x => ToNode(x)).Concat(parameters.Select(x => ToNode(x))).Where(x => x != null).ToList(),
                    NodeGroups = ClearUnsafeGroups(groups.Select(x => x.ToNodeGroup()).ToList()),
                    BHoM_Guid = cluster.InstanceGuid
                };

                nodeContent = nodeContent.PopulateTypes();

                DA.SetData(0, nodeContent);

                Helpers.ShowEvents(this, BH.Engine.Base.Query.CurrentEvents());
            }
            catch (Exception e)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, e.Message);
            }
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        private static INode ToNode(GH_Component component)
        {
            if (component is CallerComponent)
            {
                CallerComponent cc = component as CallerComponent;
                if (cc.Caller != null)
                    return component.IToNode(cc.Caller.SelectedItem, cc.Caller.GetType().Name);     
            }

            return null;
        }

        /*******************************************/

        private static INode ToNode(IGH_Param component) 
        {
            if (component is CallerValueList)
            {
                CallerValueList cc = component as CallerValueList;
                if (cc.Caller != null)
                    return cc.IToNode(cc.Caller.Choices, cc.Caller.SelectedItem);
            }
            else 
            {
                return Engine.Grasshopper.Convert.IToNode(component as dynamic);
            }

            return null;
        }

        /*******************************************/

        private static List<NodeGroup> ClearUnsafeGroups(List<NodeGroup> groups)
        {
            Dictionary<Guid, List<NodeGroup>> dic = new Dictionary<Guid, List<NodeGroup>>();
            foreach (NodeGroup group in groups)
                CollectNodeGroups(group, ref dic);

            List<NodeGroup> clashes = dic.Values.Where(x => x.Count > 1).SelectMany(x => x).Distinct().ToList();
            if (clashes.Count > 0)
            {
                string message = clashes.Count.ToString() + " groups have been found containing elements also contained in other groups. Those groups will be ignored.";
                message += "Group names:" + clashes.Select(x => "\n- " + x.Description).Aggregate((a,b) => a + b);
                Engine.Base.Compute.RecordWarning(message);
                return groups.Except(clashes).ToList();
            }
            else
                return groups;
        }

        /*******************************************/

        private static void CollectNodeGroups(NodeGroup group, ref Dictionary<Guid, List<NodeGroup>> result)
        {
            if (group != null)
            {
                foreach (Guid id in group.NodeIds)
                {
                    if (!result.ContainsKey(id))
                        result[id] = new List<NodeGroup> { group };
                    else
                        result[id].Add(group);
                }

                foreach (NodeGroup child in group.InternalGroups)
                    CollectNodeGroups(child, ref result);
            }
        }

        /*******************************************/
    }
}





