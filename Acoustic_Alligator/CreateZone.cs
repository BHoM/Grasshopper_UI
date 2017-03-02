using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using Grasshopper.Kernel;
using BHG = BHoM.Geometry;
using BHA = BHoM.Acoustic;

namespace Acoustic_Alligator
{
    public class CreateZone : GH_Component
    {
        public CreateZone() : base("Create Zone", "Create Zone", "Create BHoM Zone", "Alligator", "Acoustics") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("6A8D2D81-EFA3-4DF3-8A5A-9C76005248F3");
            }
        }
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Acoustic_Alligator.Properties.Resources.BHoM_Acoustics_Zone; }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Zone", "Zone", "Brep Zone", GH_ParamAccess.item);
            pManager.AddPointParameter("Reciever Points", "RecieverPts", "Point3d Reciever Location", GH_ParamAccess.list);
            //pManager.AddNumberParameter("Sample Height", "Sample Height", "Height of Recievers", GH_ParamAccess.item);
            //pManager.AddNumberParameter("Sample Step", "Sample Step", "Distance Between Point Cloud", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("BHoM Zone", "BHoM Zone", "BHoM Zone", GH_ParamAccess.item);
            pManager.AddPointParameter("Sample Points", "Sample Points", "GH Sample Points", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<BHG.Point> points = new List<BHoM.Geometry.Point>();
            List<Point3d> samplePoints = new List<Point3d>();

            Rhino.Geometry.Brep geom = new Rhino.Geometry.Brep();
            //double recieverHeight = 0;
            //double recieverStep = 0;

            if (!DA.GetData(0, ref geom)) { return; }
            if (!DA.GetDataList(1, samplePoints)) { return; }

            foreach (Point3d pt in samplePoints)
            {
                points.Add(new BHG.Point(pt.X, pt.Y, pt.Z));
            }

            //if (!DA.GetData(1, ref recieverHeight)) { return; }
            //if (!DA.GetData(2, ref recieverStep)) { return; }

            //Rhino.Geometry.BoundingBox bBox = geom.GetBoundingBox(false);

            //for (double x = bBox.Min.X; x <= bBox.Max.X; x += recieverStep)
            //{
                //for (double y = bBox.Min.Y; y <= bBox.Max.Y; y += recieverStep)
                //{
                    //Point3d pt = new Point3d(x, y, recieverHeight);
                    //points.Add(new BHoM.Geometry.Point(pt.X, pt.Y, pt.Z));
                    //samplePoints.Add(pt);
                //}
            //}

            BHA.Zone zone = new BHA.Zone(points, geom.GetArea(), geom.GetVolume());
            DA.SetData(0, zone);
            DA.SetDataList(1, samplePoints);
        }
    }
}
