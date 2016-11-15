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

namespace Alligator.Structural.Loads
{
    public class CheckLoadRegionContainment : GH_Component
    {
        public CheckLoadRegionContainment() : base("CheckLoadRegionConstainment", "CheckLoadRegionConstainment", "Checks if the midpoint of an FE-Mesh face is inside a curve and tags it if it is", "Structure", "Utilities")
        { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("902F6706-7EF9-4A75-9174-D12FFEDF0CFA");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("FEMesh", "FEMesh", "The finite element mesh to check", GH_ParamAccess.item);
            pManager.AddCurveParameter("Crv", "Crv", "Region curve to check if inside", GH_ParamAccess.list);
            pManager.AddTextParameter("Tag", "Tag", "String to tag the femesh with", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FEMesh", "FEMesh", "The tagged mesh", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHE.FEMesh inMesh = GHE.DataUtils.GetGenericData<BHE.FEMesh>(DA, 0);
            BHE.FEMesh mesh = (BHE.FEMesh)inMesh.ShallowClone();

            mesh.CustomData = new Dictionary<string, object>();

            foreach (KeyValuePair<string,object> kvp in inMesh.CustomData)
            {
                mesh.CustomData[kvp.Key] = kvp.Value;
            }

            List<Curve> crvs = GHE.DataUtils.GetDataList<Curve>(DA, 1);
            List<string> tags = GHE.DataUtils.GetDataList<string>(DA, 2);

            if (crvs.Count != tags.Count)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Needs the same number of tags and curves");
                return;
            }



            List<BHoM.Geometry.Point> points = mesh.Nodes.Select(x => x.Point).ToList();
            Point3d midPt = new Point3d(0, 0, 0);

            foreach (BHoM.Geometry.Point p in points)
            {
                midPt += GHE.GeometryUtils.Convert(p);
            }
            midPt /= points.Count;

            for (int i = 0; i < crvs.Count; i++)
            {

                if (crvs[i].Contains(midPt) == PointContainment.Inside)
                {
                    if (!mesh.CustomData.ContainsKey("LoadRegions"))
                    {
                        mesh.CustomData["LoadRegions"] = new List<string>();
                    }
                  ((List<string>)mesh.CustomData["LoadRegions"]).Add(tags[i]);
                }

            }

            DA.SetData(0, mesh);

        }
    }

    public class GroupLoadedRegions : GH_Component
    {
        public GroupLoadedRegions() : base("GroupLoadedRegions", "GroupLoadedRegions", "Creates a group for a set of element based on load region tag", "Structure", "Utilities")
        { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("3A32B746-41C0-4903-8CE9-3A6BE216869F");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("FEMesh", "FEMesh", "The finite element mesh to group", GH_ParamAccess.list);
            pManager.AddGenericParameter("Load", "Load", "Load", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Groups", "Groups", "Groups", GH_ParamAccess.list);
            pManager.AddGenericParameter("Loads", "Loads", "Loads", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {


            List<BHE.FEMesh> meshes = GHE.DataUtils.GetGenericDataList<BHE.FEMesh>(DA, 0);
            List<BHL.ILoad> loads = GHE.DataUtils.GetGenericDataList<BHL.ILoad>(DA, 1);

            HashSet<string> names = new HashSet<string>();

            foreach (BHL.ILoad load in loads)
            {
                if (!(load is BHL.AreaUniformalyDistributedLoad))
                    continue;

                names.Add(((BHL.AreaUniformalyDistributedLoad)load).Objects.Name);

            }


            List<BHB.Group<BHE.FEMesh>> groups = new List<BHB.Group<BHE.FEMesh>>();

            foreach (string name in names)
            {

                BHB.Group<BHE.FEMesh> group = new BHB.Group<BHE.FEMesh>();
                group.Name = name;

                bool hasItems = false;

                foreach (BHE.FEMesh mesh in meshes)
                {
                    if (mesh.CustomData.ContainsKey("LoadRegions") && ((List<string>)mesh["LoadRegions"]).Contains(name))
                    {
                        hasItems = true;
                        group.Data.Add(mesh);
                    }
                }

                if(hasItems)
                    groups.Add(group);

            }

            DA.SetDataList(0, groups);

        }
    }
}
