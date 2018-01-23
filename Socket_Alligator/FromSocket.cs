using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using BH.Adapter.Socket.Tcp;
using System.Diagnostics;

namespace Alligator.Socket
{
    public class FromSocket : GH_Component
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Guid ComponentGuid { get; } = new Guid("9C6E7D1E-48E4-4A67-BEAF-4AC2A49A0016"); 

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Socket_Alligator.Properties.Resources.BHoM_FromSocket; 


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public FromSocket() : base("From Socket", "FromSocket", "Send string to a socket", "Alligator", "Socket")
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
            pManager.AddBooleanParameter("active", "active", "check if the component currently allows data transfer", GH_ParamAccess.item, false);
        }

        /*******************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("data", "data", "data received", GH_ParamAccess.list);
        }

        /*******************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Debug.WriteLine("FromSocket entering Solve Instance at " + (DateTime.Now.Ticks / (TimeSpan.TicksPerSecond / 10)).ToString());
            string address = ""; DA.GetData<string>(0, ref address);
            int port = 8888; DA.GetData<int>(1, ref port);
            string tag = ""; DA.GetData(2, ref tag);
            bool active = false; DA.GetData<bool>(3, ref active);

            if (!active) return;

            if (m_Link == null || m_Port != port || m_Address != address)
            {
                m_Port = port;
                m_Address = address;
                m_Link = new BH.Adapter.Socket.SocketLink_Tcp(port, address);
                m_Link.DataObservers += MessageReceived;
            }
            m_Tag = tag;

            DA.SetDataList(0, m_Message);
            Debug.WriteLine("FromSocket exiting Solve Instance at " + (DateTime.Now.Ticks / (TimeSpan.TicksPerSecond / 10)).ToString());
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        private void MessageReceived(DataPackage package)
        {
            Debug.WriteLine("FromSocket received package at " + (DateTime.Now.Ticks / (TimeSpan.TicksPerSecond / 10)).ToString());
            if (package.Tag == m_Tag)
            {
                m_Message = package.Data;
                ExpireSolution(true);
            }
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        private int m_Port;
        private string m_Address;
        private string m_Tag;
        private BH.Adapter.Socket.SocketLink_Tcp m_Link = null;
        private List<object> m_Message = new List<object>();


        /*******************************************/
    }
}
