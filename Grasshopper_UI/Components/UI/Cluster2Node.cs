using BH.Engine.Grasshopper;
using BH.Engine.Node2Code;
using BH.oM.Node2Code;
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

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }

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
                        BHoM_Guid = x.InstanceGuid,
                        ParentId = cluster.InstanceGuid
                    }).ToList(),
                    Outputs = outputs.Select(x => new ReceiverParam {
                        Name = x.NickName,
                        Description = x.Description,
                        SourceId = x.Sources.First().InstanceGuid,
                        BHoM_Guid = x.InstanceGuid,
                        ParentId = cluster.InstanceGuid
                    }).ToList(),
                    InternalNodes = components.Select(x => ToNode(x)).Where(x => x != null).ToList(),
                    InternalParams = parameters.Select(x => ToParam(x)).Where(x => x != null).ToList(),
                    NodeGroups = groups.Select(x => x.ToNodeGroup()).ToList(),
                    BHoM_Guid = cluster.InstanceGuid
                };

                nodeContent = nodeContent.PopulateTypes();

                DA.SetData(0, nodeContent);
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

        private static DataParam ToParam(IGH_Param component) 
        {
            if (component is CallerValueList)
            {
                CallerValueList cc = component as CallerValueList;
                if (cc.Caller != null)
                    return cc.IToParam(cc.Caller.Choices, cc.Caller.SelectedItem);
            }
            else 
            {
                return Engine.Grasshopper.Convert.IToParam(component as dynamic);
            }

            return null;
        }

        /*******************************************/
    }
}
