using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Alligator.Socket
{
    public class ToSocket : GH_Component
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Guid ComponentGuid { get; } = new Guid("0EEFD0B8-CD8E-44FF-9144-2DF685A93EE7"); 

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Socket_Alligator.Properties.Resources.BHoM_ToSocket; 


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public ToSocket() : base("To Socket", "ToSocket", "Send string to a socket", "Alligator", "Socket")
        {
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("IP address", "address", "IP address of the socket to send data to. Local 127.0.0.1 as default", GH_ParamAccess.item, "127.0.0.1");
            pManager.AddIntegerParameter("port", "port", "port used by the socket. Value between 3000 and 9000", GH_ParamAccess.item, 8888);
            pManager.AddTextParameter("tag", "tag", "tag attached to the data", GH_ParamAccess.item, "");
            pManager.AddGenericParameter("data", "data", "data to send", GH_ParamAccess.list);
            pManager.AddBooleanParameter("active", "active", "check if the component currently allows data transfer", GH_ParamAccess.item, false);
        }

        /*******************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("success", "success", "data transfer succesful", GH_ParamAccess.item);
        }

        /*******************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string address = ""; DA.GetData<string>(0, ref address);
            int port = 8888; DA.GetData<int>(1, ref port);
            string tag = ""; DA.GetData(2, ref tag);
            List<object> data = new List<object>(); DA.GetDataList<object>(3, data);
            bool active = false; DA.GetData<bool>(4, ref active);

            if (!active) return;

            if (m_Link == null || m_Port != port || m_Address != address)
            {
                m_Port = port;
                m_Address = address;
                m_Link = new BH.Adapter.Socket.SocketLink_Tcp(port, address);
            }

            bool success = m_Link.SendData(data, tag);
            DA.SetData(0, success);
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        private int m_Port;
        private string m_Address;
        private BH.Adapter.Socket.SocketLink_Tcp m_Link = null;


        /*******************************************/
    }
}
