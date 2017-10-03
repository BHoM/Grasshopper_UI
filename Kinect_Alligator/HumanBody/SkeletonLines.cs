using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using BH.oM.HumanBody;
using RHG = Rhino.Geometry;
using BHG = BH.oM.Geometry;
using BH.UI.Alligator.Query;
using BH.Adapter.Rhinoceros;

namespace BH.UI.Alligator.Kinect
{
    public class SkeletonLines : GH_Component
    {
        public SkeletonLines() : base("Get skeleton lines", "SkeletonLines", "Gets lines from BHoM Skeleton", "Kinect", "BodyTracking") { }
        public override Guid ComponentGuid { get { return new Guid("b5ae4d84-f8ea-450a-9e22-93ba63d6c4c0"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return Properties.Resources.skeleton_lines; } }

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
            Skeleton skeleton = new Skeleton();
            skeleton = DA.BH_GetData(0, skeleton);
            List<RHG.Line> TrackingLines = new List<RHG.Line>();
            List<string> BodyPartNames = new List<string>();
            List<RHG.Sphere> Spheres = new List<RHG.Sphere>();

            if (skeleton != null)
            {
                Dictionary<string, BHG.Line> trackingLines = skeleton.GetAllTrackingLines();
                foreach (BHG.Line line in trackingLines.Values)
                {
                    BodyPartNames.Add(trackingLines.GetEnumerator().Current.Key);
                    TrackingLines.Add(line.ToRhino());
                    trackingLines.GetEnumerator().MoveNext();
                }

                try
                {
                    BHG.Point headPt = trackingLines["Neck"].Start;
                    RHG.Sphere head = new RHG.Sphere(new RHG.Point3d(headPt.X, headPt.Y, headPt.Z), 0.1);
                    Spheres.Add(head);
                    BodyPartNames.Add("Head");

                    if (skeleton.HandRight.State == HandStateName.Closed)
                    {
                        BHG.Point rightHandPt = trackingLines["HandRight"].End;
                        Spheres.Add(new RHG.Sphere(new RHG.Point3d(rightHandPt.X, rightHandPt.Y, rightHandPt.Z), 0.05));
                        BodyPartNames.Add("rightHand");
                    }
                    if (skeleton.HandLeft.State == HandStateName.Closed)
                    {
                        BHG.Point leftHandPt = trackingLines["HandLeft"].End;
                        Spheres.Add(new RHG.Sphere(new RHG.Point3d(leftHandPt.X, leftHandPt.Y, leftHandPt.Z), 0.05));
                        BodyPartNames.Add("leftHand");
                    }
                }
                catch { }
            }
            DA.SetDataList(0, TrackingLines);
            DA.SetData(1, BodyPartNames);
        }
    }
}