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

namespace Alligator.Structural.Loads.CustomLoadGrouping
{
    public class LocalLoadCreation : GH_Component
    {
        public LocalLoadCreation() : base("LocalLoadMapping", "LocalLoadMapping", "LocalLoadMapping", "Structure", "Utilities") { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("FEMeshes", "FEMeshes", "FEMeshes", GH_ParamAccess.list);
            pManager.AddGenericParameter("VertLoadcases", "VertLoadcases", "VertLoadcases", GH_ParamAccess.list);
            pManager.AddGenericParameter("HorLoadcases", "HorLoadcases", "HorLoadcases", GH_ParamAccess.list);
            pManager.AddCurveParameter("GlobalCurves", "GlobalCurves", "GlobalCurves", GH_ParamAccess.list);
            pManager.AddTextParameter("GlobalTags", "GlobalTags", "GlobalTags", GH_ParamAccess.list);
            pManager.AddCurveParameter("LocalCurves", "LocalCurves", "LocalCurves", GH_ParamAccess.list);
            pManager.AddTextParameter("LocalTags", "LocalTags", "LocalTags", GH_ParamAccess.list);
            pManager.AddNumberParameter("UpliftForce", "UpliftForce", "UpliftForce", GH_ParamAccess.list);
            pManager.AddNumberParameter("DownForce", "DownForce", "DownForce", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AreaLoads", "AreaLoads", "AreaLoads", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<BHE.FEMesh> meshes = GHE.DataUtils.GetGenericDataList<BHE.FEMesh>(DA, 0);
            List<BHL.Loadcase> vertCases = GHE.DataUtils.GetGenericDataList<BHL.Loadcase>(DA, 1);
            List<BHL.Loadcase> horCases = GHE.DataUtils.GetGenericDataList<BHL.Loadcase>(DA, 2);
            List<Curve> globalCurves = GHE.DataUtils.GetDataList<Curve>(DA, 3);
            List<string[]> globalTags = GHE.DataUtils.GetDataList<string>(DA, 4).Select(x => x.Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries)).ToList();
            List<Curve> localCurves = GHE.DataUtils.GetDataList<Curve>(DA, 5);
            List<string[]> localTags = GHE.DataUtils.GetDataList<string>(DA, 6).Select(x => x.Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries)).ToList();
            List<double> upForce = GHE.DataUtils.GetDataList<double>(DA, 7);
            List<double> downForce = GHE.DataUtils.GetDataList<double>(DA, 8);


            List<Point3d> meshMidPts = new List<Point3d>();

            foreach (BHE.FEMesh mesh in meshes)
            {
                List<Point3d> pts = mesh.Nodes.Select(x => GHE.GeometryUtils.Convert(x.Point)).ToList();

                Point3d mid = Point3d.Origin;

                foreach (Point3d p in pts)
                {
                    mid += p;
                }

                mid.Z = 0;
                mid /= pts.Count;

                meshMidPts.Add(mid);
            }


            Dictionary<string, List<BHL.AreaUniformalyDistributedLoad>> loads = new Dictionary<string, List<BHL.AreaUniformalyDistributedLoad>>();

            for (int i = 0; i < vertCases.Count; i++)
            {
                string vertName = vertCases[i].Name;

                if (loads.ContainsKey(vertName))
                    continue;

                string horName = horCases[i].Name;

                List<string> caseGlobalTags = new List<string>();
                List<Curve> caseGlobalCurves = new List<Curve>();
                List<string> caseLocalTags = new List<string>();
                List<Curve> caseLocalCurves = new List<Curve>();

                for (int j = 0; j < globalTags.Count; j++)
                {
                    if (vertName.Contains(globalTags[j][0]) && vertName.Contains(globalTags[j][1]))
                    {
                        caseGlobalTags.Add(globalTags[j][1]);
                        caseGlobalCurves.Add(globalCurves[j]);
                    }
                }

                for (int j = 0; j < localTags.Count; j++)
                {
                    if (horName.Contains(localTags[j][0]))
                    {
                        caseLocalTags.Add(localTags[j][1]);
                        caseLocalCurves.Add(localCurves[j]);
                    }
                }


                Dictionary<string, BHL.AreaUniformalyDistributedLoad> caseLoads = new Dictionary<string, BHL.AreaUniformalyDistributedLoad>();

                for (int j = 0; j < meshMidPts.Count; j++)
                {
                    string upDown = null;

                    for (int k = 0; k < caseGlobalCurves.Count; k++)
                    {
                        if (caseGlobalCurves[k].Contains(meshMidPts[j]) == PointContainment.Inside)
                        {
                            upDown = caseGlobalTags[k];
                            break;
                        }
                    }

                    if (upDown == null)
                        continue;

                    int regionIndex = -1;

                    for (int k = 0; k < caseLocalCurves.Count; k++)
                    {
                        if (caseLocalCurves[k].Contains(meshMidPts[j]) == PointContainment.Inside)
                        {
                            regionIndex = int.Parse(caseLocalTags[k]);
                            break;
                        }
                    }

                    if (regionIndex < 0)
                        continue;

                    string loadName = upDown + regionIndex;

                    BHL.AreaUniformalyDistributedLoad load;

                    if (!caseLoads.TryGetValue(loadName, out load))
                    {
                        load = new BHL.AreaUniformalyDistributedLoad();
                        load.Name = loadName;
                        load.Axis = BHL.LoadAxis.Local;
                        load.Loadcase = vertCases[i];
                        double pressure;

                        if (upDown == "Down")
                            pressure = downForce[regionIndex];
                        else
                            pressure = upForce[regionIndex];

                        load.Pressure = new BHoM.Geometry.Vector(0, 0, pressure);

                        caseLoads.Add(loadName, load);
                    }

                    load.Objects.Data.Add(meshes[j]);


                }

                loads.Add(vertName, caseLoads.Values.ToList());


            }


            DA.SetDataList(0, loads.Values);



        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("DB9C96DF-423F-4F6C-A3D3-312D1E4A6200");
            }
        }
    


    }
}
