using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BHL = BH.oM.Structural.Loads;
using BHE = BH.oM.Structural.Elements;
using BH.Engine.Grasshopper.Components;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;
using BH.Engine.Grasshopper;

namespace BH.UI.Alligator.Structural.Loads
{

    public class CreateBarLoad : BHoMBaseComponent<BHL.Load<BHE.Bar>>
    {
        private enum BarLoadTypes
        {
            BarUniformLoad,
            BarPointLoad,
            BarVaryingLoad,
            BarPrestressLoad,
            BarTempratureLoad
        }

        public CreateBarLoad() : base("Create Bar Load", "BL", "Create various bar loads", "Structure", "Loads") { }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Structural.Properties.Resources.BHoM_BarForce; }
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("A8748884-03F9-4CC3-9156-340E7751F2F7");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Bars", "B", "The bars to apply the load to", GH_ParamAccess.list);
            pManager.AddGenericParameter("Load Case", "LC", "Load case for the load", GH_ParamAccess.item);
            pManager.AddTextParameter("Name", "Na", "Name of the load", GH_ParamAccess.item);
            pManager.AddGenericParameter("Custom Data", "CU", "Custom data", GH_ParamAccess.item);

            pManager[2].Optional = true;
            pManager[3].Optional = true;

            AppendEnumOptions("Load Type", typeof(BarLoadTypes));
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Bar Load", "BL", "The created bar loads", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Get bars

            List<BHE.Bar> bars = DataUtils.GetDataList<BHE.Bar>(DA, 0);


            BHL.Load<BHE.Bar> load;


            int ind1 = 4;

            switch ((BarLoadTypes)m_SelectedOption[0])
            {
                
                case BarLoadTypes.BarPointLoad:
                    load = new BHL.BarPointLoad();
                    Vector3d force = Vector3d.Unset;
                    Vector3d moment = Vector3d.Unset;
                    double pos = 0;

                    if (DA.GetData(ind1, ref force))
                        ((BHL.BarPointLoad)load).ForceVector = force.ToBHoMVector();

                    if (DA.GetData(ind1+1, ref moment))
                        ((BHL.BarPointLoad)load).MomentVector = moment.ToBHoMVector();

                    if (DA.GetData(ind1 + 2, ref pos))
                        ((BHL.BarPointLoad)load).DistanceFromA = pos;
                    else
                    {
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Position on bar neccessary for bar point load");
                        return;
                    }
                    break;
                case BarLoadTypes.BarVaryingLoad:
                    load = new BHL.BarVaryingDistributedLoad();
                    Vector3d forceA = Vector3d.Unset;
                    Vector3d momentA = Vector3d.Unset;
                    double posA = 0;

                    Vector3d forceB = Vector3d.Unset;
                    Vector3d momentB = Vector3d.Unset;
                    double posB = 0;

                    if (DA.GetData(ind1, ref forceA))
                        ((BHL.BarVaryingDistributedLoad)load).ForceVectorA = forceA.ToBHoMVector();

                    if (DA.GetData(ind1 + 1, ref momentA))
                        ((BHL.BarVaryingDistributedLoad)load).MomentVectorA = momentA.ToBHoMVector();

                    if (DA.GetData(ind1 + 2, ref posA))
                        ((BHL.BarVaryingDistributedLoad)load).DistanceFromA = posA;
                    else
                        ((BHL.BarVaryingDistributedLoad)load).DistanceFromA = 0;

                    if (DA.GetData(ind1+3, ref forceB))
                        ((BHL.BarVaryingDistributedLoad)load).ForceVectorB = forceB.ToBHoMVector();

                    if (DA.GetData(ind1 + 4, ref momentB))
                        ((BHL.BarVaryingDistributedLoad)load).MomentVectorB = momentB.ToBHoMVector();

                    if (DA.GetData(ind1 + 5, ref posB))
                        ((BHL.BarVaryingDistributedLoad)load).DistanceFromB = posB;
                    else
                        ((BHL.BarVaryingDistributedLoad)load).DistanceFromB = 0;
                    break;

                case BarLoadTypes.BarPrestressLoad:
                    load = new BHL.BarPrestressLoad();
                    double prestressValue = 0;
                    if (DA.GetData(ind1, ref prestressValue))
                        ((BHL.BarPrestressLoad)load).PrestressValue = prestressValue;
                    break;

                case BarLoadTypes.BarTempratureLoad:
                    load = new BHL.BarTemperatureLoad();
                    Vector3d tempratureChange = Vector3d.Unset;
                    if (DA.GetData(ind1, ref tempratureChange))
                        ((BHL.BarTemperatureLoad)load).TemperatureChange = tempratureChange.ToBHoMVector();
                    break;

                case BarLoadTypes.BarUniformLoad:
                default:
                    load = new BHL.BarUniformlyDistributedLoad();
                    Vector3d distForce = Vector3d.Unset;
                    Vector3d distMoment = Vector3d.Unset;
                    if (DA.GetData(ind1, ref distForce))
                        ((BHL.BarUniformlyDistributedLoad)load).ForceVector = distForce.ToBHoMVector();

                    if (DA.GetData(ind1 + 1, ref distMoment))
                        ((BHL.BarUniformlyDistributedLoad)load).MomentVector = distMoment.ToBHoMVector();
                    break;
            }

            string name = "";
            if (DA.GetData(2, ref name))
            {
                load.Name = name;
            }

            load.Objects = bars;

            Dictionary<string, object> customData = DataUtils.GetData<Dictionary<string, object>>(DA, 3);

            if (customData != null)
            {
                foreach (KeyValuePair<string, object> item in customData)
                {
                    load.CustomData.Add(item.Key, item.Value);
                }
            }


            BHL.Loadcase loadCase = DataUtils.GetData<BHL.Loadcase>(DA, 1);
            load.Loadcase = loadCase;


            DA.SetData(0, load);
        }

        protected override void UpdateInput(object enumSelection)
        {
            int ind1 = 4;

            if (enumSelection.GetType() != typeof(BarLoadTypes))
                return;

            switch ((BarLoadTypes)enumSelection)
            {
                case BarLoadTypes.BarPrestressLoad:
                    CreateParam("Prestress Force", "PSF", "The prestressing force to apply to the bar", GH_ParamAccess.item, ParamType.Number, ind1);
                    UnregisterParameterFrom(ind1+1);
                    break;
                case BarLoadTypes.BarTempratureLoad:
                    CreateParam("Temperature Change", "T", "Temprature change vector of the bar", GH_ParamAccess.item, ParamType.Vector, ind1);
                    UnregisterParameterFrom(ind1+1);
                    break;
                case BarLoadTypes.BarPointLoad:
                    CreateParam("Point Force", "F", "Point force to be applied to the bar", GH_ParamAccess.item, ParamType.Vector, ind1);
                    CreateParam("Point Moment", "M", "Point moment to be applied to the bar", GH_ParamAccess.item, ParamType.Vector, ind1 + 1);
                    CreateParam("Distance", "D", "Distance from the start node", GH_ParamAccess.item, ParamType.Number, ind1 + 2);
                    UnregisterParameterFrom(ind1 + 3);
                    break;

                case BarLoadTypes.BarVaryingLoad:
                    CreateParam("Point Force A", "FA", "Force to be applied to the bar", GH_ParamAccess.item, ParamType.Vector, ind1);
                    CreateParam("Point Moment A", "MA", "Moment to be applied to the bar", GH_ParamAccess.item, ParamType.Vector, ind1 + 1);
                    CreateParam("Distance A", "DA", "Distance from the start node", GH_ParamAccess.item, ParamType.Number, ind1 + 2);
                    CreateParam("Point Force B", "FB", "Force to be applied to the bar", GH_ParamAccess.item, ParamType.Vector, ind1 +3);
                    CreateParam("Point Moment B", "MB", "Moment to be applied to the bar", GH_ParamAccess.item, ParamType.Vector, ind1 + 4);
                    CreateParam("Distance B", "DB", "Distance from the start node", GH_ParamAccess.item, ParamType.Number, ind1 + 5);
                    break;

                case BarLoadTypes.BarUniformLoad:
                default:
                    CreateParam("Force", "F", "Uniform force to be applied to the bar", GH_ParamAccess.item, ParamType.Vector, ind1);
                    CreateParam("Moment", "M", "Uniform moment to be applied to the bar", GH_ParamAccess.item, ParamType.Vector, ind1 + 1);
                    UnregisterParameterFrom(ind1 + 2);
                    break;
            }

            OnAttributesChanged();

        }

        private enum ParamType
        {
            Vector,
            Number,
        }

        private void CreateParam(string name, string nickname, string description, GH_ParamAccess access, ParamType type, int index )
        {

            if (Params.Input.Count <= index)
            {
                if (type == ParamType.Number)
                    Params.RegisterInputParam(new Param_Number(), index);
                else if (type == ParamType.Vector)
                    Params.RegisterInputParam(new Param_Vector(), index);
                else
                    Params.RegisterInputParam(new Param_GenericObject(), index);
            }
            else
            {
                if ((type == ParamType.Number && Params.Input[index].TypeName != "Number") || (type == ParamType.Vector && Params.Input[index].TypeName != "Vector"))
                    Params.UnregisterParameter(Params.Input[index]);

                if (type == ParamType.Number)
                    Params.RegisterInputParam(new Param_Number(), index);
                else if (type == ParamType.Vector)
                    Params.RegisterInputParam(new Param_Vector(), index);
                else
                    Params.RegisterInputParam(new Param_GenericObject(), index);

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
