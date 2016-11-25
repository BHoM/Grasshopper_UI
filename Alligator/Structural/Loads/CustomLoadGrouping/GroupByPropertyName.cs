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
    public class GroupByPropertyName : GH_Component
    {
        public GroupByPropertyName() : base("GroupByPropertyName", "GroupByPropertyName", "GroupByPropertyName", "Structure", "Utilities")
        { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("EA729FE6-5B36-4C5D-BC07-1786C62E8902");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Bars", "Bars", "Bars", GH_ParamAccess.list);
            pManager.AddGenericParameter("Loads", "Loads", "Loads", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Groups", "Groups", "Groups", GH_ParamAccess.list);
            pManager.AddGenericParameter("Loads", "Loads", "Loads", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<BHE.Bar> bars = GHE.DataUtils.GetGenericDataList<BHE.Bar>(DA, 0);
            List<BHL.ILoad> loads = GHE.DataUtils.GetGenericDataList<BHL.ILoad>(DA, 1);

            List<BHL.Load<BHE.Node>> newLoads = new List<BHL.Load<BHE.Node>>();

            foreach (BHL.ILoad load in loads)
            {
                if (!(load is BHL.Load<BHE.Node>))
                    continue;

                if (!((BHL.Load<BHE.Node>)load).Objects.Name.StartsWith("RC"))
                    continue;

                newLoads.Add((load as BHL.Load<BHE.Node>).ShallowClone() as BHL.Load<BHE.Node>);
            }

            List<BHB.Group<BHE.Node>> groups = new List<BHB.Group<BHE.Node>>();

            List<BHE.Bar> compatiableBars = bars.Where(x => x.SectionProperty.Name.StartsWith("RC")).ToList();


            foreach (BHL.Load<BHE.Node> load in newLoads)
            {
                string[] strArr = load.Objects.Name.Split(' ');
                BHB.Group<BHE.Node> group = new BHB.Group<BHE.Node>();
                group.Name = load.Objects.Name;

                foreach (BHE.Bar bar in compatiableBars)
                {
                    if (bar.SectionProperty.Name != strArr[0])
                        continue;

                    if (strArr[1] == "TR")
                        group.Data.Add(bar.StartNode);
                    else
                        group.Data.Add(bar.EndNode);
                }

                load.Objects = group;
                groups.Add(group);
            }


            DA.SetDataList(0, groups);
            DA.SetDataList(1, newLoads);

        }
    }
}
