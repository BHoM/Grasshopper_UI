using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHE = Grasshopper_Engine;
using BHR = BHoM.Base.Results;
using BHSR = BHoM.Structural.Results;
using BHI = BHoM.Structural.Interface;
using BHD = BHoM.Databases;

namespace Alligator.Structural.Results
{
    public class AddBarForces : GH_Component
    {
        public AddBarForces() : base("Add Bar Force", "AddForce", "Adds up forces with the same ID and loadcombination", "Structural", "Misc") { }
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("23FF53A8-2C9C-4E0B-8D22-6B63F56C9C96");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Result Server 1", "ResultServer1", "Application or Result server to extract results from", GH_ParamAccess.item);
            pManager.AddGenericParameter("Result Server 2", "ResultServer2", "Application or Result server to extract results from", GH_ParamAccess.item);
            pManager.AddGenericParameter("DataBase", "Db", "Database to push to", GH_ParamAccess.item);
            pManager.AddTextParameter("Key", "Key", "Key to push to in db", GH_ParamAccess.item, "");
            pManager.AddBooleanParameter("Activate", "Go", "Activate", GH_ParamAccess.item);

        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Success", "Success", "Result database create successfully", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (!GHE.DataUtils.Run(DA, 4))
            {
                DA.SetData(0, false);
                return;
            }


            BHI.IResultAdapter app1 = GHE.DataUtils.GetGenericData<BHI.IResultAdapter>(DA, 0);
            BHI.IResultAdapter app2 = GHE.DataUtils.GetGenericData<BHI.IResultAdapter>(DA, 1);
            BHD.IDatabaseAdapter db = GHE.DataUtils.GetGenericData<BHD.IDatabaseAdapter>(DA, 2);
            string key = GHE.DataUtils.GetData<string>(DA, 3);

            Dictionary<string, BHoM.Base.Results.IResultSet> res1, res2;

            app1.GetBarForces(null, null, 5, BHoM.Base.Results.ResultOrder.Name, out res1);
            app2.GetBarForces(null, null, 5, BHoM.Base.Results.ResultOrder.Name, out res2);

            List<BHSR.BarForce> forces = new List<BHSR.BarForce>();

            foreach (KeyValuePair<string, BHR.IResultSet> kvp in res1)
            {
                BHR.IResultSet resSet1, resSet2;

                resSet1 = kvp.Value;
                List<BHSR.BarForce> forces1 = resSet1.AsList<BHSR.BarForce>();
                if (res2.TryGetValue(kvp.Key, out resSet2))
                {
                    List<BHSR.BarForce> forces2 = resSet2.AsList<BHSR.BarForce>();

                    foreach (var force1 in forces1)
                    {
                        BHSR.BarForce force;
                        var matches = forces2.Where(x => x.Loadcase == force1.Loadcase && x.ForcePosition == force1.ForcePosition).ToList();

                        if (matches.Count == 1)
                        {
                            force = new BHSR.BarForce(
                                force1.Name,
                                force1.Loadcase,
                                force1.ForcePosition,
                                force1.Divisions,
                                force1.TimeStep,
                                force1.FX + matches[0].FX,
                                force1.FY + matches[0].FY,
                                force1.FZ + matches[0].FZ,
                                force1.MX + matches[0].MX,
                                force1.MY + matches[0].MY,
                                force1.MZ + matches[0].MZ);

                            forces.Add(force);
                            forces2.Remove(matches[0]);
                        }
                    }

                    foreach (var force in forces2)
                        forces.Add(force);

                    res2.Remove(kvp.Key);
                }
                else
                {
                    foreach (var force in forces1)
                        forces.Add(force);
                }

            }

            foreach (KeyValuePair<string, BHR.IResultSet> kvp in res2)
            {
                List<BHSR.BarForce> forces2 = kvp.Value.AsList<BHSR.BarForce>();

                foreach (var force in forces2)
                    forces.Add(force);
            }


            bool success = db.Push("BarForces", forces, key);


            DA.SetData(0, success);
        }
    }
}
