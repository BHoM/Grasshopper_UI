//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Grasshopper.Kernel;
//using BH.oM.Structural.Loads;
//using BH.oM.Structural.Elements;
//using Grasshopper.Kernel.Parameters;
//using Rhino.Geometry;
//using BH.Adapter.Rhinoceros;

//namespace BH.UI.Alligator.Structural.Loads
//{
//    public class CreateBarLoad : GH_Component
//    {
//        public CreateBarLoad() : base("Create Bar Load", "BarLoad", "Create various bar loads", "Structure", "Loads") { }
//        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return Properties.Resources.BHoM_BarForce; } }
//        public override Guid ComponentGuid { get { return new Guid("A8748884-03F9-4CC3-9156-340E7751F2F7"); } }

//        protected override void RegisterInputParams(GH_InputParamManager pManager)
//        {
//            pManager.AddGenericParameter("Bars", "B", "The bars to apply the load to", GH_ParamAccess.list);
//            pManager.AddGenericParameter("Load Case", "LC", "Load case for the load", GH_ParamAccess.item);
//            pManager.AddTextParameter("Name", "Na", "Name of the load", GH_ParamAccess.item);
//            pManager.AddGenericParameter("Custom Data", "CU", "Custom data", GH_ParamAccess.item);

//            pManager[2].Optional = true;
//            pManager[3].Optional = true;

//            AppendEnumOptions("Load Type", typeof(LoadType));
//        }
//        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
//        {
//            pManager.AddGenericParameter("Bar Load", "BL", "The created bar loads", GH_ParamAccess.item);
//        }

//        protected override void SolveInstance(IGH_DataAccess DA)
//        {
//            List<Bar> bars = new List<Bar>();
//            Loadcase loadCase = new Loadcase();
//            string name = "";
//            Dictionary<string, object> customData = new Dictionary<string, object>();

//            Load<Bar> load = default(Load<Bar>);

//            int firstIndex = 4;
//            switch ((LoadType)m_SelectedOption[0])
//            {

//                case LoadType.BarPointLoad:
//                    load = new BarPointLoad();
//                    Vector3d force = Vector3d.Unset;
//                    Vector3d moment = Vector3d.Unset;
//                    double pos = 0;

//                    if (DA.GetData(firstIndex, ref force))
//                        ((BarPointLoad)load).ForceVector = force.FromRhino();

//                    if (DA.GetData(firstIndex + 1, ref moment))
//                        ((BarPointLoad)load).MomentVector = moment.FromRhino();

//                    if (DA.GetData(firstIndex + 2, ref pos))
//                        ((BarPointLoad)load).DistanceFromA = pos;
//                    else
//                    {
//                        AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Position on bar neccessary for bar point load");
//                        return;
//                    }
//                    break;
//                case LoadType.BarVaryingLoad:
//                    load = new BarVaryingDistributedLoad();
//                    Vector3d forceA = Vector3d.Unset;
//                    Vector3d momentA = Vector3d.Unset;
//                    double posA = 0;

//                    Vector3d forceB = Vector3d.Unset;
//                    Vector3d momentB = Vector3d.Unset;
//                    double posB = 0;

//                    if (DA.GetData(firstIndex, ref forceA))
//                        ((BarVaryingDistributedLoad)load).ForceVectorA = forceA.FromRhino();

//                    if (DA.GetData(firstIndex + 1, ref momentA))
//                        ((BarVaryingDistributedLoad)load).MomentVectorA = momentA.FromRhino();

//                    if (DA.GetData(firstIndex + 2, ref posA))
//                        ((BarVaryingDistributedLoad)load).DistanceFromA = posA;
//                    else
//                        ((BarVaryingDistributedLoad)load).DistanceFromA = 0;

//                    if (DA.GetData(firstIndex + 3, ref forceB))
//                        ((BarVaryingDistributedLoad)load).ForceVectorB = forceB.FromRhino();

//                    if (DA.GetData(firstIndex + 4, ref momentB))
//                        ((BarVaryingDistributedLoad)load).MomentVectorB = momentB.FromRhino();

//                    if (DA.GetData(firstIndex + 5, ref posB))
//                        ((BarVaryingDistributedLoad)load).DistanceFromB = posB;
//                    else
//                        ((BarVaryingDistributedLoad)load).DistanceFromB = 0;
//                    break;

//                //case LoadType.BarPrestressLoad:
//                //    load = new BarPrestressLoad();
//                //    double prestressValue = 0;
//                //    if (DA.GetData(firstIndex, ref prestressValue))
//                //        ((BarPrestressLoad)load).PrestressValue = prestressValue;
//                //    break;
//                case LoadType.BarTemperature:
//                    load = new BarTemperatureLoad();
//                    Vector3d tempratureChange = Vector3d.Unset;
//                    if (DA.GetData(firstIndex, ref tempratureChange))
//                        ((BarTemperatureLoad)load).TemperatureChange = tempratureChange.FromRhino();
//                    break;
//                case LoadType.BarUniformLoad:
//                default:
//                    load = new BarUniformlyDistributedLoad();
//                    Vector3d distForce = Vector3d.Unset;
//                    Vector3d distMoment = Vector3d.Unset;
//                    if (DA.GetData(firstIndex, ref distForce))
//                        ((BarUniformlyDistributedLoad)load).ForceVector = distForce.FromRhino();
//                    if (DA.GetData(firstIndex + 1, ref distMoment))
//                        ((BarUniformlyDistributedLoad)load).MomentVector = distMoment.FromRhino();
//                    break;
//            }

//            load.Objects = bars;
//            load.Loadcase = loadCase;

//            DA.SetData(0, load);
//        }

//        private void UpdateInput(object enumSelection)
//        {
//            int firstIndex = 4;

//            if (enumSelection.GetType() != typeof(LoadType))
//                return;

//            switch ((LoadType)enumSelection)
//            {
//                //case LoadType.BarPrestressLoad: // TODO Implement PrestressLoad into BHoM
//                //    CreateParam("Prestress Force", "PSF", "The prestressing force to apply to the bar", GH_ParamAccess.item, ParamType.Number, firstIndex);
//                //    UnregisterParameterFrom(firstIndex + 1);
//                //    break;
//                case LoadType.BarTemperature:
//                    CreateParam("Temperature Change", "T", "Temprature change vector of the bar", GH_ParamAccess.item, ParamType.Vector, firstIndex);
//                    UnregisterParameterFrom(firstIndex + 1);
//                    break;
//                case LoadType.BarPointLoad:
//                    CreateParam("Point Force", "F", "Point force to be applied to the bar", GH_ParamAccess.item, ParamType.Vector, firstIndex);
//                    CreateParam("Point Moment", "M", "Point moment to be applied to the bar", GH_ParamAccess.item, ParamType.Vector, firstIndex + 1);
//                    CreateParam("Distance", "D", "Distance from the start node", GH_ParamAccess.item, ParamType.Number, firstIndex + 2);
//                    UnregisterParameterFrom(firstIndex + 3);
//                    break;

//                case LoadType.BarVaryingLoad:
//                    CreateParam("Point Force A", "FA", "Force to be applied to the bar", GH_ParamAccess.item, ParamType.Vector, firstIndex);
//                    CreateParam("Point Moment A", "MA", "Moment to be applied to the bar", GH_ParamAccess.item, ParamType.Vector, firstIndex + 1);
//                    CreateParam("Distance A", "DA", "Distance from the start node", GH_ParamAccess.item, ParamType.Number, firstIndex + 2);
//                    CreateParam("Point Force B", "FB", "Force to be applied to the bar", GH_ParamAccess.item, ParamType.Vector, firstIndex + 3);
//                    CreateParam("Point Moment B", "MB", "Moment to be applied to the bar", GH_ParamAccess.item, ParamType.Vector, firstIndex + 4);
//                    CreateParam("Distance B", "DB", "Distance from the start node", GH_ParamAccess.item, ParamType.Number, firstIndex + 5);
//                    break;

//                case LoadType.BarUniformLoad:
//                default:
//                    CreateParam("Force", "F", "Uniform force to be applied to the bar", GH_ParamAccess.item, ParamType.Vector, firstIndex);
//                    CreateParam("Moment", "M", "Uniform moment to be applied to the bar", GH_ParamAccess.item, ParamType.Vector, firstIndex + 1);
//                    UnregisterParameterFrom(firstIndex + 2);
//                    break;
//            }
//            OnAttributesChanged();
//        }

//        private enum ParamType
//        {
//            Vector,
//            Number,
//        }

//        private void CreateParam(string name, string nickname, string description, GH_ParamAccess access, ParamType type, int index)
//        {

//            if (Params.Input.Count <= index)
//            {
//                if (type == ParamType.Number)
//                    Params.RegisterInputParam(new Param_Number(), index);
//                else if (type == ParamType.Vector)
//                    Params.RegisterInputParam(new Param_Vector(), index);
//                else
//                    Params.RegisterInputParam(new Param_GenericObject(), index);
//            }
//            else
//            {
//                if ((type == ParamType.Number && Params.Input[index].TypeName != "Number") || (type == ParamType.Vector && Params.Input[index].TypeName != "Vector"))
//                    Params.UnregisterParameter(Params.Input[index]);

//                if (type == ParamType.Number)
//                    Params.RegisterInputParam(new Param_Number(), index);
//                else if (type == ParamType.Vector)
//                    Params.RegisterInputParam(new Param_Vector(), index);
//                else
//                    Params.RegisterInputParam(new Param_GenericObject(), index);
//            }
//            Params.Input[index].Optional = true;
//            Params.Input[index].Name = name;
//            Params.Input[index].NickName = nickname;
//            Params.Input[index].Description = description;
//            Params.Input[index].Access = access;
//        }

//        private void UnregisterParameterFrom(int index)
//        {
//            for (int i = index; i < Params.Input.Count; i++)
//            {
//                IGH_Param p = Params.Input[i--];
//                Params.UnregisterParameter(p);
//            }
//        }
//    }
//}
