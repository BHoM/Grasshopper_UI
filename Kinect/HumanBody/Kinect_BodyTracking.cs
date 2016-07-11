using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using R = Rhino.Geometry;
using KinectToolkit;
using BHoM.HumanBody;
using BH = BHoM.Geometry;

namespace Alligator.Kinect.BodyTracking
{
    public class GetSkeleton : GH_Component
    {
        public GetSkeleton() : base("Get skeleton", "GetSkeleton", "Gets skeletons from kinect sensor", "Kinect", "BodyTracking") { }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Sensor", "app", "Kinect body tracking", GH_ParamAccess.item);
            pManager[0].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Skeleton", "S", "BHoM Skeleton", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
             {
                List<BHoM.Geometry.Point> points = new List<BHoM.Geometry.Point>();

                Sensor app = Utils.GetGenericData<Sensor>(DA, 0);
                if (!DA.GetData(0, ref app)) return;

                KinectBody kinectBody = new KinectBody();
                List<BHoM.HumanBody.Skeleton> skeletons = new List<BHoM.HumanBody.Skeleton>();
                kinectBody.GetSkeleton(out skeletons, out points, app);
                
                DA.SetData(0, skeletons);
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("a7a70ee7-5276-4fca-be8d-b449141f4709"); }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Kinect.Properties.Resources.skeleton; }
        }
    }

    public class SkeletonLines : GH_Component
    {
        public SkeletonLines() : base("Get skeleton lines", "SkeletonLines", "Gets lines from BHoM Skeleton", "Kinect", "BodyTracking") { }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Skeleton", "S", "BHoM Skeleton", GH_ParamAccess.item);
            pManager[0].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("lines", "lines", "Body part lines", GH_ParamAccess.list);
            pManager.AddTextParameter("names", "names", "Body part names", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Skeleton skeleton = Utils.GetGenericData<Skeleton>(DA, 0);
            List<R.GeometryBase> TrackingLines = new List<R.GeometryBase>();
            List<string> BodyPartNames = new List<string>();
            List<R.Sphere> Spheres = new List<R.Sphere>();

            if (skeleton != null)
            {
                Dictionary<string, BH.Line> trackingLines = skeleton.GetAllTrackingLines();
                foreach (BH.Line line in trackingLines.Values)
                {
                    BodyPartNames.Add(trackingLines.GetEnumerator().Current.Key);
                    TrackingLines.Add(GeometryUtils.Convert(line as BHoM.Geometry.GeometryBase));
                    trackingLines.GetEnumerator().MoveNext();
                }

                try
                {
                    //R.Sphere head = new R.Sphere(GeometryUtils.Convert(trackingLines["Neck"].StartPoint), 0.1);
                    BH.Point headPt = trackingLines["Neck"].StartPoint;
                    R.Sphere head = new R.Sphere(new R.Point3d(headPt.X, headPt.Y, headPt.Z), 0.1);
                    Spheres.Add(head);
                    BodyPartNames.Add("Head");

                    if (skeleton.HandRight.State == HandStateName.Closed)
                    {
                        BH.Point rightHandPt = trackingLines["HandRight"].EndPoint;
                        Spheres.Add(new R.Sphere(new R.Point3d(rightHandPt.X, rightHandPt.Y, rightHandPt.Z), 0.05));
                        BodyPartNames.Add("rightHand");
                    }
                    if (skeleton.HandLeft.State == HandStateName.Closed)
                    {
                        BH.Point leftHandPt = trackingLines["HandLeft"].EndPoint;
                        Spheres.Add(new R.Sphere(new R.Point3d(leftHandPt.X, leftHandPt.Y, leftHandPt.Z), 0.05));
                        BodyPartNames.Add("leftHand");
                    }

                }
                catch { }
            }

            DA.SetDataList(0, TrackingLines);
            //DA.SetData(0, Spheres);
            DA.SetData(1, BodyPartNames);

        }

        public override Guid ComponentGuid
        {
            get { return new Guid("b5ae4d84-f8ea-450a-9e22-93ba63d6c4c0"); }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Kinect.Properties.Resources.skeleton_lines; }
        }
    }

    
}
