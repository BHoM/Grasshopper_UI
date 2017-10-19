using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Geometry;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using BH.Engine.MachineLearning;
using BH.UI.Alligator;

namespace StadiaCrowdAnalytics_Alligator
{
    public class StadiaCrowdAnalysis : GH_Component
    {
        public StadiaCrowdAnalysis()
            : base("StadiaCrowdAnalysis", "StadiaAnalysis",
                  "Analyse the video",
                  "Alligator", "MachineLearning")
        {
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("F4F38859-2451-48C5-87FF-F9AE1B657C81"); }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("Grid", "Grid", "The grid dividing up the frame", GH_ParamAccess.list);
            pManager.AddTextParameter("VideoFile", "VideoFile", "Full path to the video file", GH_ParamAccess.item);
            pManager.AddSurfaceParameter("SurfaceMap", "SurfaceMap", "Surface to map the frame on to build your grid", GH_ParamAccess.item);
            pManager.AddTextParameter("OutFolder", "OutFolder", "Full path to the output folder for the JSON file and image frames", GH_ParamAccess.item);
            pManager.AddNumberParameter("FPS", "FPS", "Frames Per Second (FPS) of your video (default, 30)", GH_ParamAccess.item, 30.0);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("N", "N", "N", GH_ParamAccess.item);
            pManager.AddTextParameter("N", "N", "N", GH_ParamAccess.item);
            pManager.AddGenericParameter("M", "M", "M", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<GeometryBase> grid = new List<GeometryBase>();
            string videoFile = "";
            GH_Surface srf = new GH_Surface();
            string outFile = "";
            double fps = 0;

            grid = DA.BH_GetData(0,ref grid);
            videoFile = DA.BH_GetData(1, ref videoFile);
            srf = DA.BH_GetData(2, ref srf);
            outFile = DA.BH_GetData(3,ref  outFile);
            fps = DA.BH_GetData(4, ref fps);

            if (grid == null || grid.Count == 0 || videoFile == null || srf == null || outFile == null) return; //Nope try again
            if (!outFile.EndsWith("\\"))
                outFile += "\\";

            double width = 0;
            double height = 0;

            Rhino.DocObjects.BrepObject srf2 = (Rhino.DocObjects.BrepObject)Rhino.RhinoDoc.ActiveDoc.Objects.Where(x => x.Id == srf.ReferenceID).First();
            Rhino.Geometry.Brep srf3 = (Rhino.Geometry.Brep)srf2.Geometry;
            Rhino.Geometry.Collections.BrepSurfaceList srfl = srf3.Surfaces;
            Rhino.Geometry.Surface srf4 = srfl[0];
            srf4.GetSurfaceSize(out width, out height);

            VideoAnalyser analyser = new VideoAnalyser();
            analyser.FileName = videoFile;
            analyser.LoadFile();

            double wScale = analyser.FrameWidth / width;
            double hScale = analyser.FrameHeight / height;

            //Find the top-left most point of the grid
            double xLeft = 1e10;
            double yTop = -1e10;

            foreach (GeometryBase gb in grid)
            {
                Rhino.Geometry.ControlPoint pt1 = ((Rhino.Geometry.NurbsCurve)gb).Points[0];
                xLeft = Math.Min(xLeft, pt1.Location.X);
                yTop = Math.Max(yTop, pt1.Location.Y);
            }

            foreach (GeometryBase gb in grid)
            {
                try
                {
                    Rhino.Geometry.ControlPoint pt11 = ((Rhino.Geometry.NurbsCurve)gb).Points[0];
                    Rhino.Geometry.ControlPoint pt21 = ((Rhino.Geometry.NurbsCurve)gb).Points[2];
                    Rhino.Geometry.ControlPoint pt1 = new ControlPoint(Math.Abs(pt11.Location.X - xLeft), Math.Abs(pt11.Location.Y - yTop), 0);
                    Rhino.Geometry.ControlPoint pt2 = new ControlPoint(Math.Abs(pt21.Location.X - xLeft), Math.Abs(pt21.Location.Y - yTop), 0);

                    analyser.AddROI(new BH.oM.Geometry.Point((Math.Min(pt1.Location.X, pt2.Location.X) * wScale), (Math.Min(pt1.Location.Y, pt2.Location.Y) * hScale), 0), new BH.oM.Geometry.Point((Math.Max(pt2.Location.X, pt1.Location.X) * wScale), (Math.Max(pt2.Location.Y, pt1.Location.Y) * hScale), 0));
                }
                catch { }
            }

            analyser.IssueHumanIDs();

            analyser.Analyse(outFile);

            analyser.ExportResults(fps);

            DA.SetData(0, width.ToString() + " - " + height.ToString());
            DA.SetData(1, wScale.ToString() + " / " + hScale.ToString());
            DA.SetData(2, analyser);
        }
    }
}
