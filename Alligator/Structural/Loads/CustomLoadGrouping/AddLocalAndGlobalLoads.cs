using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHE = BHoM.Structural.Elements;
using BHL = BHoM.Structural.Loads;
using Grasshopper.Kernel;
using Rhino.Geometry;
using GHE = Grasshopper_Engine;
using BHB = BHoM.Base;
using BHoM.Structural.Interface;
using BHoM.Base.Results;
using BHoM.Structural.Results;

namespace Alligator.Structural.Loads.CustomLoadGrouping
{
    public class AddLocalAndGlobalLoads : GH_Component
    {
        public AddLocalAndGlobalLoads() : base("AddLocalAndGlobalLoads", "AddLocalAndGlobalLoads", "AddLocalAndGlobalLoads", "Structure", "Utilities") { }
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("C12B7E77-DA75-46BA-88C0-7D385E61DE85");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("LocalResultServer", "LocalResultServer", "LocalResultServer", GH_ParamAccess.item);
            pManager.AddTextParameter("LocalLoadcases", "LocalLoadcases", "LocalLoadcases", GH_ParamAccess.list);
            pManager.AddGenericParameter("GlobalResultServer", "GlobalResultServer", "GlobalResultServer", GH_ParamAccess.item);
            pManager.AddTextParameter("GlobalLoadcases", "GlobalLoadcases", "GlobalLoadcases", GH_ParamAccess.list);
            pManager.AddTextParameter("BarNumbers", "BarNumbers", "BarNumbers", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Loads", "Loads", "Loads", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IResultAdapter localServer = Grasshopper_Engine.DataUtils.GetGenericData<IResultAdapter>(DA, 0);
            List<string> localLoadcases = Grasshopper_Engine.DataUtils.GetDataList<string>(DA, 1);

            IResultAdapter globalServer = Grasshopper_Engine.DataUtils.GetGenericData<IResultAdapter>(DA, 2);
            List<string> globalLoadcases = Grasshopper_Engine.DataUtils.GetDataList<string>(DA, 3);

            List<string> barNumbers = Grasshopper_Engine.DataUtils.GetDataList<string>(DA, 4);

            Dictionary<string, IResultSet> localSet, globalSet;

            localServer.GetBarForces(barNumbers, localLoadcases, 5, BHB.Results.ResultOrder.Name, out localSet);
            globalServer.GetBarForces(barNumbers, globalLoadcases, 5, ResultOrder.Name, out globalSet);

            List<BarForce> forces = new List<BarForce>();

            foreach (KeyValuePair<string, IResultSet> kvp in localSet)
            {
                List<BarForce> localForces = kvp.Value.AsList<BarForce>();

                IResultSet gloSet;

                if (globalSet.TryGetValue(kvp.Key, out gloSet))
                {
                    List<BarForce> globalForces = gloSet.AsList<BarForce>();

                    foreach (BarForce localForce in localForces)
                    {
                        var matches = globalForces.Where(x => x.ForcePosition == localForce.ForcePosition);

                        foreach (BarForce globalForce in matches)
                        {
                            string localCase = localForce.Loadcase;
                            string globalCase = globalForce.Loadcase;

                            string newCase = globalCase.Substring(0, 2) + localCase.Substring(2, 3);

                            BarForce force = new BarForce(
                               localForce.Name,
                               newCase,
                               localForce.ForcePosition,
                               localForce.Divisions,
                               localForce.TimeStep,
                               localForce.FX + globalForce.FX,
                               localForce.FY + globalForce.FY,
                               localForce.FZ + globalForce.FZ,
                               localForce.MX + globalForce.MX,
                               localForce.MY + globalForce.MY,
                               localForce.MZ + globalForce.MZ
                                );
                            forces.Add(force);
                        }
                    }

                }
            }

            DA.SetDataList(0, forces);
        }
    }
}