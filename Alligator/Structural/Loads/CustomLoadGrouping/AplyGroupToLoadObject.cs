using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHE = BH.oM.Structural.Elements;
using BHL = BH.oM.Structural.Loads;
using Grasshopper.Kernel;
using Rhino.Geometry;
using GHE = Grasshopper_Engine;
using BHB = BH.oM.Base;

namespace Alligator.Structural.Loads.CustomLoadGrouping
{
    public class AplyGroupToLoadObject : GH_Component
    {
        public AplyGroupToLoadObject() : base("AddGroupToLoad", "AddGroupToLoad", "Add a group to a load based on names", "Structure", "Utilities") { }
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("14A3AB69-63D4-470D-8973-1BD413103E87");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Loads", "Loads", "Loads", GH_ParamAccess.list);
            pManager.AddGenericParameter("Groups", "Groups", "Groups", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Loads", "Loads", "Loads", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<BHL.ILoad> loads = GHE.DataUtils.GetGenericDataList<BHL.ILoad>(DA, 0);
            List<BHB.IGroup> groups = GHE.DataUtils.GetGenericDataList<BHB.IGroup>(DA, 1);

            List<BHL.ILoad> newLoads = new List<BHL.ILoad>();

            string objectName;

            foreach (BHL.ILoad load in loads)
            {
                BHL.ILoad newLoad;

                switch (load.LoadType)
                {
                    case BHL.LoadType.Selfweight:
                        newLoad = new BHL.GravityLoad();
                        ((BHL.GravityLoad)newLoad).GravityDirection = ((BHL.GravityLoad)load).GravityDirection;
                        objectName = ((BHL.GravityLoad)load).Objects.Name;
                        break;
                    case BHL.LoadType.PointForce:
                        BHL.PointForce ptForce = (BHL.PointForce)load;
                        newLoad = new BHL.PointForce();
                        ((BHL.PointForce)newLoad).Force = ptForce.Force;
                        ((BHL.PointForce)newLoad).Force = ptForce.Force;
                        objectName = ptForce.Objects.Name;
                        break;
                    case BHL.LoadType.BarUniformLoad:
                        BHL.BarUniformlyDistributedLoad barLoad = (BHL.BarUniformlyDistributedLoad)load;
                        newLoad = new BHL.BarUniformlyDistributedLoad();
                        ((BHL.BarUniformlyDistributedLoad)newLoad).ForceVector = barLoad.ForceVector;
                        ((BHL.BarUniformlyDistributedLoad)newLoad).MomentVector = barLoad.MomentVector;
                        objectName = ((BHL.BarUniformlyDistributedLoad)load).Objects.Name;
                        break;
                    case BHL.LoadType.BarTemperature:
                        newLoad = new BHL.BarTemperatureLoad();
                        ((BHL.BarTemperatureLoad)newLoad).TemperatureChange = ((BHL.BarTemperatureLoad)load).TemperatureChange;
                        objectName = ((BHL.BarTemperatureLoad)load).Objects.Name;
                        break;
                    case BHL.LoadType.AreaUniformLoad:
                        newLoad = new BHL.AreaUniformalyDistributedLoad();
                        ((BHL.AreaUniformalyDistributedLoad)newLoad).Pressure = ((BHL.AreaUniformalyDistributedLoad)load).Pressure;
                        objectName = ((BHL.AreaUniformalyDistributedLoad)load).Objects.Name;
                        break;
                    case BHL.LoadType.Pressure:
                        newLoad = new BHL.BarPrestressLoad();
                        ((BHL.BarPrestressLoad)newLoad).PrestressValue = ((BHL.BarPrestressLoad)load).PrestressValue;
                        objectName = ((BHL.BarPrestressLoad)load).Objects.Name;
                        break;
                    case BHL.LoadType.AreaTemperature:
                    case BHL.LoadType.BarVaryingLoad:
                    case BHL.LoadType.AreaVaryingLoad:
                    case BHL.LoadType.BarPointLoad:
                    case BHL.LoadType.Geometrical:
                    case BHL.LoadType.PointDisplacement:
                    case BHL.LoadType.PointVelocity:
                    case BHL.LoadType.PointAcceleration:
                    case BHL.LoadType.PointMass:
                    default:
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Not implemented for load type " + load.LoadType.ToString());
                        continue;
                }

                newLoad.Loadcase = load.Loadcase;
                newLoad.Name = load.Name;
                newLoad.Projected = load.Projected;
                newLoad.Axis = load.Axis;
                newLoad.CustomData = new Dictionary<string, object>(load.CustomData);

                foreach (BHB.IGroup group in groups)
                {
                    if (objectName != group.Name)
                        continue;

                    if (group.ObjectType == typeof(BHE.IAreaElement) && newLoad is BHL.Load<BHE.IAreaElement>)
                    {
                        ((BHL.Load<BHE.IAreaElement>)newLoad).Objects.Name = objectName;
                        foreach (BHE.IAreaElement elem in group as BHB.Group<BHE.IAreaElement>)
                        {
                            ((BHL.Load<BHE.IAreaElement>)newLoad).Objects.Data.Add(elem);
                        }
                    }
                    else if (group.ObjectType == typeof(BHE.FEMesh) && newLoad is BHL.Load<BHE.IAreaElement>)
                    {
                        ((BHL.Load<BHE.IAreaElement>)newLoad).Objects.Name = objectName;
                        foreach (BHE.IAreaElement elem in group as BHB.Group<BHE.FEMesh>)
                        {
                            ((BHL.Load<BHE.IAreaElement>)newLoad).Objects.Data.Add(elem);
                        }
                    }
                    else if (group.ObjectType == typeof(BHE.Bar) && newLoad is BHL.Load<BHE.Bar>)
                    {
                        ((BHL.Load<BHE.Bar>)newLoad).Objects.Name = objectName;
                        foreach (BHE.Bar elem in group as BHB.Group<BHE.Bar>)
                        {
                            ((BHL.Load<BHE.Bar>)newLoad).Objects.Data.Add(elem);
                        }
                    }
                    else if (group.ObjectType == typeof(BHE.Node) && newLoad is BHL.Load<BHE.Node>)
                    {
                        ((BHL.Load<BHE.Node>)newLoad).Objects.Name = objectName;
                        foreach (BHE.Node elem in group as BHB.Group<BHE.Node>)
                        {
                            ((BHL.Load<BHE.Node>)newLoad).Objects.Data.Add(elem);
                        }
                    }
                    else if (newLoad is BHL.Load<BHB.BHoMObject>)
                    {
                        ((BHL.Load<BHB.BHoMObject>)newLoad).Objects.Name = objectName;
                        foreach (BHB.BHoMObject elem in group.Objects)
                        {
                            ((BHL.Load<BHB.BHoMObject>)newLoad).Objects.Data.Add(elem);
                        }
                    }
                }

                newLoads.Add(newLoad);
            }

            DA.SetDataList(0, newLoads);

        }
    }
}
