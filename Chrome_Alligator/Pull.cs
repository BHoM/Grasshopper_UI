using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using CA = BH.Adapter.Chrome;
using Grasshopper.Kernel.Types;
using BH.oM.Base;

namespace Alligator.Mongo
{
    public class Pull : GH_Component
    {
        public Pull() : base("Pull", "Pull", "Pull data from the adapter", "Alligator", "Chrome")
        {
            m_Port = 0;
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("367B7B09-7445-4B9C-88E0-7B1DF96E3888");
            }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.primary;
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("adapter", "adapter", "adapter to get data from", GH_ParamAccess.item);
            pManager.AddTextParameter("query", "query", "query", GH_ParamAccess.item);
            pManager.AddTextParameter("params", "params", "parameters", GH_ParamAccess.list);
            pManager.AddTextParameter("config", "config", "config", GH_ParamAccess.list);
            pManager.AddBooleanParameter("active", "active", "check if the component currently allows data transfer", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Data", "Data", "return the data", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            CA.ChromeAdapter link = null; // GHE.DataUtils.GetGenericData<CA.ChromeAdapter>(DA, 0);
            string query = ""; // GHE.DataUtils.GetData<string>(DA, 1);
            List<string> configList = new List<string>(); // GHE.DataUtils.GetGenericDataList<string>(DA, 3);
            bool active = false; DA.GetData<bool>(4, ref active);

            DA.GetData<CA.ChromeAdapter>(0, ref link);
            DA.GetData<string>(1, ref query);
            DA.GetDataList<string>(3, configList);

            if (!active)
            {
                DA.SetData(0, null);
                return;
            }


            if (m_Port != link.Port)
            {
                link.AddSelectionCallback(MessageReceived);
                m_Port = link.Port;
            }

            DA.SetData(0, m_Message);
        }


        int m_Port = 0;
        private List<object> m_Message = null;

        private void MessageReceived(List<object> message)
        {
            m_Message = message; /*JsonReader.ReadObject(message);*/
            ExpireSolution(true);
        }

    }
}
