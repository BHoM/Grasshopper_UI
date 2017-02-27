using BHoM.Base.Results;
using BHoM.Structural.Elements;
using BHoM.Structural.Interface;
using BHoM.Structural.Results;
using Grasshopper.Kernel;
using StructuralDesign_Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Design_Alligator.Structural
{
    public abstract class ElementDesignBase<TD, TU> : GH_Component
        where TD : IElementDesign<TU>, new()
        where TU : IResult, new()
    {
        public ElementDesignBase(string name, string nickname, string description, string category, string subCategory) : base(name, nickname, description, category, subCategory) { }
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("{A4380BC6-B24D-4145-A838-82FBAA4734A0}");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Elements", "E", "Design elements or bars", GH_ParamAccess.list);
            pManager.AddTextParameter("Loadcases", "Loadcases", "Loadcases to design to", GH_ParamAccess.list);
            pManager.AddGenericParameter("ResultServer", "ResultServer", "Bar results", GH_ParamAccess.item);
            pManager.AddTextParameter("Identifier", "Key", "Name of custom data key linking the bar to the result server", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Allow End Offset", "AllowOffset", "Allows and end offset check for over utilised members", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Execute", "Execute", "Starts the element design", GH_ParamAccess.item);

            Params.Input[1].Optional = true;
            Params.Input[4].Optional = true;
            Params.Input[1].AddVolatileDataList(new Grasshopper.Kernel.Data.GH_Path(0), null);
            Params.Input[4].AddVolatileData(new Grasshopper.Kernel.Data.GH_Path(0), 0, false);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            TU su = new TU();
            BarForce force = new BarForce();

            for (int i = 1; i < su.ColumnHeaders.Length; i++)
            {
                if (i < 6)
                    pManager.AddTextParameter(su.ColumnHeaders[i], su.ColumnHeaders[i], su.ColumnHeaders[i], GH_ParamAccess.list);
                else if (i == 6)
                {
                    for (int j = 5; j < force.ColumnHeaders.Length; j++)
                    {
                        pManager.AddNumberParameter(force.ColumnHeaders[j], force.ColumnHeaders[j], force.ColumnHeaders[j], GH_ParamAccess.list);
                    }
                }
                if (i >= 6)
                {
                    pManager.AddNumberParameter(su.ColumnHeaders[i], su.ColumnHeaders[i], su.ColumnHeaders[i], GH_ParamAccess.list);
                }
            }

            pManager.AddNumberParameter("CriticalRatio", "CriticalRatio", "Critical Ratio", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (Grasshopper_Engine.DataUtils.Run(DA, 5))
            {
                List<DesignElement> elems = Grasshopper_Engine.DataUtils.GetDesignElements(DA, 0);
                List<string> loadcases = Grasshopper_Engine.DataUtils.GetDataList<string>(DA, 1);
                IResultAdapter server = Grasshopper_Engine.DataUtils.GetGenericData<IResultAdapter>(DA, 2);
                string key = Grasshopper_Engine.DataUtils.GetData<string>(DA, 3);
                bool allowOffset = Grasshopper_Engine.DataUtils.GetData<bool>(DA, 4);

                TD elemDesign = new TD();
                elemDesign.DesignElements = elems;
                elemDesign.Loadcases = loadcases;
                elemDesign.ResultAdapter = server;
                elemDesign.Key = key;

                List<TU> utilisations = elemDesign.GetUtilisations();
                List<BarForce> forces = elemDesign.GetCriticalForces();

                Dictionary<string, List<object>> results = new Dictionary<string, List<object>>();
                TU su = new TU();
                BarForce bf = new BarForce();

                for (int i = 1; i < su.ColumnHeaders.Length; i++)
                {
                    results.Add(su.ColumnHeaders[i], new List<object>());
                }

                for (int i = 5; i < bf.ColumnHeaders.Length; i++)
                {
                    results.Add(bf.ColumnHeaders[i], new List<object>());
                }

                results.Add("CriticalRatio", new List<object>());

                //Dictionary<string, Bar> filter = new BHoM.Base.ObjectFilter<Bar>(bars).ToDictionary<string>(key, BHoM.Base.FilterOption.UserData);

                ResultSet<TU> set = new ResultSet<TU>();
                set.AddData(utilisations);

                for (int i = 0; i < utilisations.Count; i++)
                {
                    double max = 0;
                    object[] data = utilisations[i].GetData();
                    string barNum = data[1].ToString();

                    for (int j = 1; j < data.Length; j++)
                    {
                        if (j > 6 && data[j] is double)
                        {
                            results[su.ColumnHeaders[j]].Add(Math.Round((double)data[j], 2));
                            if ((double)data[j] > max)
                            {
                                max = (double)data[j];
                            }
                        }
                        else
                        {
                            results[su.ColumnHeaders[j]].Add(data[j]);
                        }
                    }

                    results["CriticalRatio"].Add(Math.Round(max, 2));
                }

                for (int i = 0; i < forces.Count; i++)
                {
                    object[] data = forces[i].GetData();
                    for (int j = 5; j < data.Length; j++)
                    {
                        results[bf.ColumnHeaders[j]].Add(data[j]);
                    }
                }

                foreach (KeyValuePair<string, List<object>> keyPair in results)
                {
                    DA.SetDataList(keyPair.Key, keyPair.Value);
                }
            }
        }
    }
}
