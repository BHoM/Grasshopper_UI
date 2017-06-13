using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHoM.Base.Results;
using BHoM.Databases;
using BHoM.Structural.Interface;
using BHoM.Structural.Results;


namespace Alligator.Structural.Results.TempMemoryResServer
{
    public class MemoryResultServer : IResultAdapter
    {
        private IResultAdapter m_internalAdapter;
        private Dictionary<string, List<BarForce>> m_ForceData;
        public MemoryResultServer()
        {
            m_ForceData = new Dictionary<string, List<BarForce>>();   
        }

        public MemoryResultServer(IResultAdapter adapter):this()
        {
            m_internalAdapter = adapter;
        }

        public void LoadAllForceData()
        {
            Dictionary<string, IResultSet> set;
            m_internalAdapter.GetBarForces(null, null, 5, ResultOrder.Name, out set);

            foreach (var kvp in set)
            {
                m_ForceData[kvp.Key] = kvp.Value.AsList<BarForce>();
            }

        }

        public bool GetBarCoordinates(List<string> bars, out Dictionary<string, IResultSet> results)
        {
            throw new NotImplementedException();
        }

        public bool GetBarForces(List<string> bars, List<string> cases, int divisions, ResultOrder orderBy, out Dictionary<string, IResultSet> results)
        {

            results = new Dictionary<string, IResultSet>();
            foreach (string s in bars)
            {
                IResultSet set;
                List<BarForce> forces;
                if (m_ForceData.TryGetValue(s, out forces))
                {
                    set = new ResultSet<BarForce>();

                    foreach (BarForce f in forces)
                    {
                        if (cases.Contains(f.Loadcase))
                        {
                            set.AddData(f.GetData());
                        }
                    }

                    results[s] = set;
                }

            }
            return true;
        }

        public bool GetBarStresses(List<string> bars, List<string> cases, int divisions, ResultOrder orderBy, out Dictionary<string, IResultSet> results)
        {
            throw new NotImplementedException();
        }

        public bool GetBarUtilisation(List<string> bars, List<string> cases, ResultOrder orderBy, out Dictionary<string, IResultSet> results)
        {
            throw new NotImplementedException();
        }

        public bool GetModalResults()
        {
            throw new NotImplementedException();
        }

        public bool GetNodeAccelerations(List<string> nodes, List<string> cases, ResultOrder orderBy, out Dictionary<string, IResultSet> results)
        {
            throw new NotImplementedException();
        }

        public bool GetNodeCoordinates(List<string> nodes, out Dictionary<string, IResultSet> results)
        {
            throw new NotImplementedException();
        }

        public bool GetNodeDisplacements(List<string> nodes, List<string> cases, ResultOrder orderBy, out Dictionary<string, IResultSet> results)
        {
            throw new NotImplementedException();
        }

        public bool GetNodeReactions(List<string> nodes, List<string> cases, ResultOrder orderBy, out Dictionary<string, IResultSet> results)
        {
            throw new NotImplementedException();
        }

        public bool GetNodeVelocities(List<string> nodes, List<string> cases, ResultOrder orderBy, out Dictionary<string, IResultSet> results)
        {
            throw new NotImplementedException();
        }

        public bool GetPanelForces(List<string> panels, List<string> cases, ResultOrder orderBy, out Dictionary<string, IResultSet> results)
        {
            throw new NotImplementedException();
        }

        public bool GetPanelStress(List<string> panels, List<string> cases, ResultOrder orderBy, out Dictionary<string, IResultSet> results)
        {
            throw new NotImplementedException();
        }

        public bool GetSlabReinforcement(List<string> panels, List<string> cases, ResultOrder orderBy, out Dictionary<string, IResultSet> results)
        {
            throw new NotImplementedException();
        }

        public bool PushToDataBase(IDatabaseAdapter dbAdapter, List<ResultType> resultTypes, List<string> loadcases, string key, bool append = false)
        {
            throw new NotImplementedException();
        }

        public bool StoreResults(string filename, List<ResultType> resultTypes, List<string> loadcases, bool append = false)
        {
            throw new NotImplementedException();
        }

        public bool GetBarDisplacements(List<string> bars, List<string> cases, int divisions, ResultOrder orderBy, out Dictionary<string, IResultSet> results)
        {
            throw new NotImplementedException();
        }
    }
}
