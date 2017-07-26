using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using GHE = Grasshopper_Engine;

namespace Alligator.Socket
{
    public class ToSocket : GH_Component
    {
        public ToSocket() : base("ToSocket", "ToSocket", "Send string to a socket", "Alligator", "Socket")
        {
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("0EEFD0B8-CD8E-44FF-9144-2DF685A93EE7");
            }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Socket_Alligator.Properties.Resources.BHoM_ToSocket; }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("IP address", "address", "IP address of the socket to send data to. Local 127.0.0.1 as default", GH_ParamAccess.item, "127.0.0.1");
            pManager.AddIntegerParameter("port", "port", "port used by the socket. Value between 3000 and 9000", GH_ParamAccess.item);
            pManager.AddGenericParameter("data", "data", "data to send", GH_ParamAccess.list);
            pManager.AddBooleanParameter("active", "active", "check if the component currently allows data transfer", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("success", "success", "data transfer succesful", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string address = ""; DA.GetData<string>(0, ref address);
            int port = 8888; DA.GetData<int>(1, ref port);
            List<object> data = new List<object>(); DA.GetDataList<object>(2, data);
            bool active = false; DA.GetData<bool>(3, ref active);

            if (!active) return;

            if (m_Link == null || m_Port != port || m_ServerName != address)
            {
                m_Port = port;
                m_ServerName = address;
                m_Link = new BH.Adapter.Socket.SocketLink(address, port);
            }

            bool success = m_Link.SendData(data);
            DA.SetData(0, success);
        }

        private int m_Port;
        private string m_ServerName;
        private BH.Adapter.Socket.SocketLink m_Link = null;
    }
}
