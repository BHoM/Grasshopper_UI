using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using BHE = BHoM.Structural.Elements;
using BHL = BHoM.Structural.Loads;
using Grasshopper.Kernel;
using Rhino.Geometry;
using GHE = Grasshopper_Engine;
using BHB = BHoM.Base;

namespace Alligator.Structural.Loads.CustomLoadGrouping
{
    public class RafterPointLoads : GH_Component
    {
        public RafterPointLoads() : base("RafterPointLoads", "RafterPointLoads", "RafterPointLoads", "Structure", "Utilities") { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("RafterNodes", "RafterNodes", "RafterNodes", GH_ParamAccess.tree);
            pManager.AddGenericParameter("Loads", "Loads", "Loads", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Groups", "Groups", "Groups", GH_ParamAccess.list);
            pManager.AddGenericParameter("Loads", "Loads", "Loads", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            List<List<BHE.Node>> rafterNodes = GHE.DataUtils.GetGenericDataListOfListFromDataTree<BHE.Node>(DA, 0);
            List<BHL.ILoad> loads = GHE.DataUtils.GetGenericDataList<BHL.ILoad>(DA, 1);



            List<BHL.Load<BHE.Node>> newLoads = new List<BHL.Load<BHE.Node>>();

            foreach (BHL.ILoad load in loads)
            {
                if (!(load is BHL.Load<BHE.Node>))
                    continue;

                string name = ((BHL.Load<BHE.Node>)load).Objects.Name;
                string[] arr = name.Split('1', '2', '3', '4', '5', '6', '7', '8', '9', '0');

                if (!(arr[0] == "R"))
                    continue;

                newLoads.Add((load as BHL.Load<BHE.Node>).ShallowClone() as BHL.Load<BHE.Node>);
            }

            List<BHB.Group<BHE.Node>> groups = new List<BHB.Group<BHE.Node>>();


            Dictionary<string, BHB.Group<BHE.Node>> groupDict = new Dictionary<string, BHB.Group<BHE.Node>>();
            //Regex numRegex = new Regex(@"\d+");

            foreach (BHL.Load<BHE.Node> load in newLoads)
            {
                string groupName = load.Objects.Name;
                BHB.Group<BHE.Node> group;

                if (!groupDict.TryGetValue(groupName, out group))
                {

                    string[] loadNameArr = groupName.Split('R', 'P');
                    group = new BHB.Group<BHE.Node>();
                    group.Name = groupName;

                    int rafterIndex, purlinIndex;

                    string rafterName = loadNameArr[1];
                    bool mirror = false;
                    if (rafterName.Contains("M"))
                    {
                        rafterName = rafterName.Trim('M');
                        mirror = true;
                    }

                    if (!(int.TryParse(rafterName, out rafterIndex) && int.TryParse(loadNameArr[2], out purlinIndex)))
                    {
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Int parsing failed");
                        return;
                    }

                    rafterIndex -= 1;
                    if (mirror)
                    {
                        foreach (int rI in GetAllRafterIndecies(rafterIndex))
                        {
                            group.Data.Add(rafterNodes[rI][purlinIndex]);
                        }
                    }
                    else
                    {
                        group.Data.Add(rafterNodes[rafterIndex][purlinIndex]);
                    }
                    groupDict.Add(groupName, group);
                }

                load.Objects = group;
            }


            DA.SetDataList(0, groupDict.Values.ToList());
            DA.SetDataList(1, newLoads);

        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("D5785148-05C8-423E-A228-50520758C1A5");
            }
        }

        private static List<int> GetAllRafterIndecies(int loadIndex)
        {
            List<int> rafterIndecis = new List<int>();

            rafterIndecis.Add(loadIndex);
            if (loadIndex == 0 || loadIndex == 14)
            {
                rafterIndecis.Add(loadIndex + 28);
            }
            else
            {
                rafterIndecis.Add(28 - loadIndex);
                rafterIndecis.Add(28 + loadIndex);
                rafterIndecis.Add(56 - loadIndex);
            }

            return rafterIndecis;
        }

        private static bool CheckIfCorrectRafter(int barNum, int loadNum)
        {
            if (barNum <= 15)
            {
                return barNum == loadNum;
            }
            else if (barNum <= 29)
            {
                return 30 - barNum == loadNum;
            }
            else if (barNum <= 43)
            {
                return barNum - 28 == loadNum;
            }
            else
            {
                return 58 - barNum == loadNum;
            }

        }

    }
}

