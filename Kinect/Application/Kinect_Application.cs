using System;
using Grasshopper.Kernel;
using KinectToolkit;

namespace Alligator.Kinect
{
    public class KinectApp : GH_Component
    {
        public KinectApp() : base("Kinect Application", "KinectApp", "Creates a kinect Application", "Kinect", "Application") { }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Activate", "A", "Activate kinect sensor", GH_ParamAccess.item);
            pManager[0].Optional = true;

        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "app", "Kinect Application", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (Utils.Run(DA, 0))
            {
                Sensor app = new Sensor();
                DA.SetData(0, app);
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("88073248-194a-4716-aed8-22ffb67dd12a"); }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Kinect.Properties.Resources.kinect_app; }
        }
    }
}
