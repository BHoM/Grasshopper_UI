using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BHB = BH.oM.Base;
using BHL = BH.oM.Structural.Loads;
using BHE = BH.oM.Structural.Elements;
using BH.Engine.Grasshopper.Components;
using Grasshopper.Kernel.Parameters;
using GHE = BH.Engine.Grasshopper;
using Rhino.Geometry;
using GH = Grasshopper;

namespace BH.UI.Alligator.Structural.Loads
{
    public class CreateNodalLoad : BHoMBaseComponent<BHL.Load<BHE.Node>>
    {
        private enum NodalLoadTypes
        {
            PointForce,
            PointDisplacement,
            PointVelocity,
            PointAcceleration,
        }

        public CreateNodalLoad() : base("Create Nodal Load", "NL", "Create various nodal loads", "Structure", "Loads") { }
        
        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Structural.Properties.Resources.BHoM_NodeForce; }
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("8E328E57-8412-4C83-BBC3-049A229EB826");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Nodes", "No", "The position or node to apply to load to allternativly a group of nodes to apply the load to", GH_ParamAccess.list);
            pManager.AddGenericParameter("Load Case", "LC", "Load case for the load", GH_ParamAccess.item);
            pManager.AddTextParameter("Name", "Na", "Name of the load", GH_ParamAccess.item);
            pManager.AddGenericParameter("Custom Data", "CU", "Custom data", GH_ParamAccess.item);

            pManager[2].Optional = true;
            pManager[3].Optional = true;

            AppendEnumOptions("Load Type", typeof(NodalLoadTypes));
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Nodal Load", "NL", "The created nodal loads", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<BHE.Node> nodes;

            GetNodesOrGroup(DA, 0, out nodes);

            //if(!GHE.DataUtils.GetNodeListFromPointOrNodes(DA, 0, out nodes)) { return; }


            BHL.Loadcase loadCase = GHE.DataUtils.GetData<BHL.Loadcase>(DA, 1);

            BHL.Load<BHE.Node> load;

            Vector3d trans = Vector3d.Unset;
            Vector3d rot = Vector3d.Unset;

            bool hasTrans = DA.GetData(4, ref trans);
            bool hasRot = DA.GetData(5, ref rot);

            switch ((NodalLoadTypes)m_SelectedOption[0])
            {
                case NodalLoadTypes.PointDisplacement:
                    load = new BHL.PointDisplacement();
                    if (hasTrans)
                        ((BHL.PointDisplacement)load).Translation = GHE.GeometryUtils.Convert(trans);

                    if (hasRot)
                        ((BHL.PointDisplacement)load).Rotation = GHE.GeometryUtils.Convert(rot);
                    break;
                case NodalLoadTypes.PointVelocity:
                    load = new BHL.PointVelocity();
                    if (hasTrans)
                        ((BHL.PointVelocity)load).TranslationalVelocity = GHE.GeometryUtils.Convert(trans);

                    if (hasRot)
                        ((BHL.PointVelocity)load).RotationalVelocity = GHE.GeometryUtils.Convert(rot);
                    break;
                case NodalLoadTypes.PointAcceleration:
                    load = new BHL.PointAcceleration();
                    if (hasTrans)
                        ((BHL.PointAcceleration)load).TranslationalAcceleration = GHE.GeometryUtils.Convert(trans);

                    if (hasRot)
                        ((BHL.PointAcceleration)load).RotationalAcceleration = GHE.GeometryUtils.Convert(rot);
                    break;
                case NodalLoadTypes.PointForce:
                default:
                    load = new BHL.PointForce();
                    if (hasTrans)
                        ((BHL.PointForce)load).SetForce(trans.X, trans.Y, trans.Z);

                    if (hasRot)
                        ((BHL.PointForce)load).SetMoment(rot.X, rot.Y, rot.Z);
                    break;
            }

            string name = "";
            if (DA.GetData(2, ref name))
            {
                load.Name = name;
            }

            load.Objects = nodes;

            Dictionary<string, object> customData = GHE.DataUtils.GetData<Dictionary<string, object>>(DA, 3);

            if (customData != null)
            {
                foreach (KeyValuePair<string,object> item in customData)
                {
                    load.CustomData.Add(item.Key, item.Value);
                }
            }

            load.Loadcase = loadCase;
            

            DA.SetData(0, load);
        }

        private bool GetNodesOrGroup(IGH_DataAccess DA, int index, out  List<BHE.Node> nodes)
        {
            List<GH.Kernel.Types.GH_Goo<object>> goObjs = new List<GH.Kernel.Types.GH_Goo<object>>();
            

            if(!DA.GetDataList(index, goObjs))
            {
                nodes = null;
                return false;
            }

            if(goObjs.Count < 1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Load needs a load group or nodes");
                nodes = null;
                return false;
            }

            if (goObjs[0].Value is List<BHE.Node>)
            {
                nodes = (List<BHE.Node>)goObjs[0].Value;
                for (int i = 1; i < goObjs.Count; i++)
                {
                    nodes.AddRange((List<BHE.Node>)(List<BHE.Node>)goObjs[i].Value);
                }
                return true;
            }

            nodes = new List<BHE.Node>();

            for (int i = 0; i < goObjs.Count; i++)
            {
                BHE.Node node;
                if (GHE.DataUtils.PointOrNodeToNode(goObjs[i].Value, out node))
                    nodes.Add(node);
            }

            return true;

        }

        protected override void UpdateInput(object enumSelection)
        {
            int ind1 = 4;

            if (enumSelection.GetType() != typeof(NodalLoadTypes))
                return;

            switch ((NodalLoadTypes)enumSelection)
            {

                case NodalLoadTypes.PointDisplacement:
                    CreateParam("Point Translation", "T", "Translation to be applied to the node [m]", GH_ParamAccess.item, ind1);
                    CreateParam("Point Rotation", "R", "Rotation to be applied to the node", GH_ParamAccess.item, ind1 + 1);
                    break;
                case NodalLoadTypes.PointVelocity:
                    CreateParam("Point Translation Velocity", "TV", "Translation Velocity to be applied to the node", GH_ParamAccess.item, ind1);
                    CreateParam("Point Rotation Velocity", "RV", "Rotation Velocity to be applied to the node", GH_ParamAccess.item, ind1 + 1);
                    break;
                case NodalLoadTypes.PointAcceleration:
                    CreateParam("Point Translation Acceleration", "TA", "Translation Acceleration to be applied to the node", GH_ParamAccess.item, ind1);
                    CreateParam("Point Rotation Acceleration", "RA", "Rotation Acceleration to be applied to the node", GH_ParamAccess.item, ind1 + 1);
                    break;
                case NodalLoadTypes.PointForce:
                default:
                    CreateParam("Point Force", "F", "Force to be applied to the node", GH_ParamAccess.item, ind1);
                    CreateParam("Point Moment", "M", "Moment to be applied to the node", GH_ParamAccess.item, ind1 + 1);
                    break;
            }
            
        }

        private void CreateParam(string name, string nickname, string description, GH_ParamAccess access, int index)
        {
            if (Params.Input.Count <= index)
            {
                Params.RegisterInputParam(new Param_Vector(), index);
            }
            Params.Input[index].Optional = true;
            Params.Input[index].Name = name;
            Params.Input[index].NickName = nickname;
            Params.Input[index].Description = description;
            Params.Input[index].Access = access;
        }

        private void UnregisterParameterFrom(int index)
        {
            for (int i = index; i < Params.Input.Count; i++)
            {
                IGH_Param p = Params.Input[i--];
                Params.UnregisterParameter(p);
            }
        }

    }
}
