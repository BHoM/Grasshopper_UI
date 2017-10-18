using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Kinect_Adapter;
using BH.oM.HumanBody;
using BHG = BH.oM.Geometry;
using BH.UI.Alligator;

namespace BH.UI.Alligator.Kinect
{
    public class GetSkeleton : GH_Component
    {
        public GetSkeleton() : base("Get skeleton", "GetSkeleton", "Gets skeletons from kinect sensor", "Kinect", "BodyTracking") { }
        public override Guid ComponentGuid { get { return new Guid("a7a70ee7-5276-4fca-be8d-b449141f4709"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return Properties.Resources.skeleton; } }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Sensor", "app", "Kinect body tracking", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Skeleton", "S", "BHoM Skeleton", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Sensor app = new Sensor();
            KinectBody kinectBody = new KinectBody();

            app = DA.BH_GetData(0, app);

            List<BHG.Point> points = new List<BHG.Point>();
            List<Skeleton> skeletons = new List<Skeleton>();
            kinectBody.GetSkeleton(out skeletons, out points, app);

            DA.SetData(0, skeletons);
        }
    }
}
