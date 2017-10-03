using System;
using Grasshopper.Kernel;
using Kinect_Adapter;
using BH.UI.Alligator.Query;

namespace BH.UI.Alligator.Kinect
{
    public class KinectApp : GH_Component
    {
        public KinectApp() : base("Kinect Application", "KinectApp", "Creates a kinect Application", "Kinect", "Application") { }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return Properties.Resources.kinect_app; } }
        public override Guid ComponentGuid { get { return new Guid("88073248-194a-4716-aed8-22ffb67dd12a"); } }
        Sensor Sensor { get; set; } = new Sensor();

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Execute", "E", "Get Kinect sensor", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Sensor", "Sensor", "KinectSensor", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool execute = false;
            execute = DA.BH_GetData(0, execute);
            if (execute)
                DA.SetData(0, Sensor);
            else
                Sensor.Close();
        }
    }
}
