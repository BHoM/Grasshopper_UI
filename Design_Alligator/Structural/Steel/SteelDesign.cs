using BHoM.Base.Results;
using BHoM.Structural.Elements;
using BHoM.Structural.Interface;
using BHoM.Structural.Results;
using BHoM_Design.Steel;
using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Design_Alligator.Structural.Steel
{
    public class SteelDesign : GH_Component
    {
        public SteelDesign() : base("Steel Design", "SteelDesign", "Design a list of bars based on input forces", "Structure", "Design") { }
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("{29380BC6-B24D-4145-A838-82FBAA4734A0}");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Bars", "Bars", "Bars to design", GH_ParamAccess.list);
            pManager.AddTextParameter("Loadcases", "Loadcases", "Loadcases to design to", GH_ParamAccess.list);
            pManager.AddGenericParameter("ResultServer", "ResultServer", "Bar results", GH_ParamAccess.item);
            pManager.AddTextParameter("Identifier", "Key", "Name of custom data key linking the bar to the result server", GH_ParamAccess.item);

            Params.Input[1].Optional = true;
            Params.Input[1].AddVolatileDataList(new Grasshopper.Kernel.Data.GH_Path(0), null);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            SteelUtilisation su = new SteelUtilisation();
            for (int i = 1; i < su.ColumnHeaders.Length; i++)
            {
                if (i < 4)
                    pManager.AddTextParameter(su.ColumnHeaders[i], su.ColumnHeaders[i], su.ColumnHeaders[i], GH_ParamAccess.list);
                else
                {
                    pManager.AddNumberParameter(su.ColumnHeaders[i], su.ColumnHeaders[i], su.ColumnHeaders[i], GH_ParamAccess.list);
                }
            }
            pManager.AddNumberParameter("CriticalRatio", "CriticalRatio", "Critical Ratio", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Bar> bars = Grasshopper_Engine.DataUtils.GetGenericDataList<Bar>(DA, 0);
            List<string> loadcases = Grasshopper_Engine.DataUtils.GetDataList<string>(DA, 1);
            IResultAdapter server = Grasshopper_Engine.DataUtils.GetGenericData<IResultAdapter>(DA, 2);
            string key = Grasshopper_Engine.DataUtils.GetData<string>(DA, 3);

            List<SteelUtilisation> utilisations = new BHoM_Design.Steel.Eurocode1993.MemberUtilisation(bars, loadcases, server, key).GetUtilisations();
            Dictionary<string, List<object>> results = new Dictionary<string, List<object>>();
            SteelUtilisation su = new SteelUtilisation();

            for (int i = 1; i < su.ColumnHeaders.Length; i++)
            {
                results.Add(su.ColumnHeaders[i], new List<object>());
            }

            results.Add("CriticalRatio", new List<object>());

            Dictionary<string, Bar> filter = new BHoM.Base.ObjectFilter<Bar>(bars).ToDictionary<string>(key, BHoM.Base.FilterOption.UserData);

            ResultSet<SteelUtilisation> set = new ResultSet<SteelUtilisation>();
            set.AddData(utilisations);

            for (int i = 0; i < utilisations.Count; i++)
            {
                double max = 0;
                string barNum = utilisations[i].Name;

                for (int j = 1; j < utilisations[i].Data.Length; j++)
                {
                    results[su.ColumnHeaders[j]].Add(utilisations[i].Data[j]);
                    if (j > 5 && (double)utilisations[i].Data[j] > max)
                    {
                        max = (double)utilisations[i].Data[j];
                    }
                }

                results["CriticalRatio"].Add(max);
            }

            foreach (KeyValuePair<string,List<object>> keyPair in results)
            {
                DA.SetDataList(keyPair.Key, keyPair.Value);
            }
        }
    }
}
