using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using System.Threading;
using GHE = Grasshopper_Engine;

namespace Alligator.Socket
{
    public class FromSocket : GH_Component
    {
        public FromSocket() : base("FromSocket", "FromSocket", "Send string to a socket", "Alligator", "Socket")
        {
            m_Socket = new Socket_Engine.SocketServer();
            m_Socket.MessageReceived += MessageReceived;
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("9C6E7D1E-48E4-4A67-BEAF-4AC2A49A0016");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("port", "port", "port used by the socket", GH_ParamAccess.item);
            pManager.AddBooleanParameter("active", "active", "check if the component currently allows data transfer", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("data", "data", "data received", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int port = 8888; DA.GetData<int>(0, ref port);
            bool active = false; DA.GetData<bool>(1, ref active);

            if (!active) return;

            m_Socket.Listen(port);
            DA.SetData(0, m_Message);
        }

        private Socket_Engine.SocketServer m_Socket;
        private String m_Message = "";

        private void MessageReceived(string message)
        {
            m_Message = message;
            ExpireSolution(true);
        }
    }
}
