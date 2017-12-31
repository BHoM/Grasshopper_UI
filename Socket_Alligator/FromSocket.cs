using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Alligator.Socket
{
    public class FromSocket : GH_Component
    {
        public FromSocket() : base("From Socket", "FromSocket", "Send string to a socket", "Alligator", "Socket")
        {
            m_Socket = new BH.Adapter.Socket.SocketServer();
            m_Socket.DataObservers += MessageReceived;
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("9C6E7D1E-48E4-4A67-BEAF-4AC2A49A0016");
            }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Socket_Alligator.Properties.Resources.BHoM_FromSocket; }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("port", "port", "port used by the socket. Value between 3000 and 9000", GH_ParamAccess.item);
            pManager.AddBooleanParameter("active", "active", "check if the component currently allows data transfer", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("data", "data", "data received", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int port = 8888; DA.GetData<int>(0, ref port);
            bool active = false; DA.GetData<bool>(1, ref active);

            if (!active)
            {
                if (m_Socket.IsActive())
                    m_Socket.Stop();
                return;
            }

            if (!m_Socket.IsActive())
                m_Socket.Start(port);

            DA.SetDataList(0, m_Message);
        }

        private BH.Adapter.Socket.SocketServer m_Socket;
        private List<object> m_Message = new List<object>();

        private void MessageReceived(List<object> message)
        {
            m_Message = message;
            ExpireSolution(true);
        }
    }
}
