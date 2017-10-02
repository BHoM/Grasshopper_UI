using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Rhino.Geometry;
using BH.UI.Alligator.Base;

//using BHoM.Geometry;

namespace BH.UI.Alligator.MachineLearning
{
    public class GridSetUp : GH_Component
    {
        public GridSetUp()
            : base("StadiaCrowdAnalysisGridSetUp", "StadiaAnalysisGridSetUp", "Set up a grid on a video frame for analysis", "Alligator", "StadiaCrowdAnalytics")
        {
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("EAF69ED3-CAFD-4C48-9EE9-4AA510D1938F"); }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("Grid Rows", "GridRows", "The lines that make up the rows of the grid", GH_ParamAccess.list);
            pManager.AddGeometryParameter("Grid Columns", "GridCols", "The lines that make up the columns of the grid", GH_ParamAccess.list);
            pManager.AddTextParameter("VideoFile", "VideoFile", "Full path to the video file", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Intersect", "IN", "TEST", GH_ParamAccess.list);
            pManager.AddPointParameter("Intersect", "IN2", "TEST", GH_ParamAccess.list);
            pManager.AddGeometryParameter("LINES", "IN3", "TEST", GH_ParamAccess.list);
            pManager.AddGeometryParameter("LINES", "IN4", "TEST", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<GeometryBase> rows = new List<GeometryBase>();
            List<GeometryBase> cols = new List<GeometryBase>();
            string videoFile = "";

            rows = DA.BH_GetDataList(0, rows);
            cols = DA.BH_GetDataList(1, cols);
            videoFile = DA.BH_GetData(2, videoFile);

            if (videoFile == null) return;

            //Get the intersecting points
            Dictionary<PolylineCurve, List<Point3d>> intersectingPointsByLine = InitialIntersectingPoints(rows, cols);

            //Get the polylines between neighbouring points
            Dictionary<Point3d, List<Polyline>> linesByPoint = LinesByPoint(intersectingPointsByLine);

            //Connect the lines to each other
            List<Polyline> rects = GetRectangles(linesByPoint);

            //Tidy up - remove duplicated rects
            rects = Tidy(rects);

            DA.SetDataList(2, rects);
            //DA.SetDataList(3, rects2);
        }

        private Dictionary<PolylineCurve, List<Point3d>> InitialIntersectingPoints(List<GeometryBase> rows, List<GeometryBase> cols)
        {
            Dictionary<PolylineCurve, List<Point3d>> intersectingPointsByLine = new Dictionary<PolylineCurve, List<Point3d>>();

            foreach (GeometryBase b in rows)
            {
                PolylineCurve row = (PolylineCurve)b;

                //Row line
                double a1 = row.PointAtEnd.Y - row.PointAtStart.Y;
                double b1 = row.PointAtEnd.X - row.PointAtStart.X;
                double c1 = a1 * row.PointAtStart.X + b1 * row.PointAtStart.Y;

                foreach (GeometryBase q in cols)
                {
                    PolylineCurve col = (PolylineCurve)q;

                    //Col line
                    double a2 = col.PointAtEnd.Y - col.PointAtStart.Y;
                    double b2 = col.PointAtEnd.X - col.PointAtStart.X;
                    double c2 = a2 * col.PointAtStart.X + b2 * col.PointAtStart.Y;

                    //Calc
                    double det = a1 * b2 - a2 * b1;
                    double x = (b2 * c1 - b1 * c2) / det;
                    double y = (a1 * c2 - a2 * c1) / det;

                    Point3d pt = new Point3d(x, y, 0);
                    //intersectingPoints.Add(new Point3d(x, y, 0));

                    if (!intersectingPointsByLine.ContainsKey(row))
                        intersectingPointsByLine.Add(row, new List<Point3d>());
                    if (!intersectingPointsByLine.ContainsKey(col))
                        intersectingPointsByLine.Add(col, new List<Point3d>());

                    intersectingPointsByLine[row].Add(pt);
                    intersectingPointsByLine[col].Add(pt);
                }
            }

            //Sort the points in each list
            foreach (KeyValuePair<PolylineCurve, List<Point3d>> kvp in intersectingPointsByLine)
            {
                for (int x = 0; x < kvp.Value.Count; x++)
                {
                    for (int y = x + 1; y < kvp.Value.Count; y++)
                    {
                        if (kvp.Value[y].X < kvp.Value[x].X)
                        {
                            //X value is lower on y index, switch the items around
                            var tmp = kvp.Value[x];
                            kvp.Value[x] = kvp.Value[y];
                            kvp.Value[y] = tmp;
                        }
                    }
                }

                for (int x = 0; x < kvp.Value.Count; x++)
                {
                    for (int y = x + 1; y < kvp.Value.Count; y++)
                    {
                        if (kvp.Value[y].X == kvp.Value[x].X)
                        {
                            //Same x value, check Y value and switch if necessary
                            if (kvp.Value[y].Y < kvp.Value[x].Y)
                            {
                                //Y value is lower on y index, switch the items around
                                var tmp = kvp.Value[x];
                                kvp.Value[x] = kvp.Value[y];
                                kvp.Value[y] = tmp;
                            }
                        }
                    }
                }
            }

            return intersectingPointsByLine;
        }

        private Dictionary<Point3d, List<Polyline>> LinesByPoint(Dictionary<PolylineCurve, List<Point3d>> intersectingPointsByLine)
        {
            Dictionary<Point3d, List<Polyline>> linesByPoint = new Dictionary<Point3d, List<Polyline>>();

            //Connect the lines to their neighbours
            foreach (KeyValuePair<PolylineCurve, List<Point3d>> kvp in intersectingPointsByLine)
            {
                for (int x = 0; x < kvp.Value.Count - 1; x++)
                {
                    List<Point3d> pts = new List<Point3d>();
                    pts.Add(kvp.Value[x]);
                    pts.Add(kvp.Value[x + 1]);

                    if (!linesByPoint.ContainsKey(kvp.Value[x]))
                        linesByPoint.Add(kvp.Value[x], new List<Polyline>());
                    if (!linesByPoint.ContainsKey(kvp.Value[x + 1]))
                        linesByPoint.Add(kvp.Value[x + 1], new List<Polyline>());

                    linesByPoint[kvp.Value[x]].Add(new Polyline(pts));
                    linesByPoint[kvp.Value[x + 1]].Add(new Polyline(pts));
                }
            }

            return linesByPoint;
        }

        private List<Polyline> GetRectangles(Dictionary<Point3d, List<Polyline>> linesByPoint)
        {
            List<Polyline> rects = new List<Polyline>();
            foreach (KeyValuePair<Point3d, List<Polyline>> kvp in linesByPoint)
            {
                foreach (Polyline pl in kvp.Value)
                {
                    List<Point3d> pts = new List<Point3d>();
                    pts.Add(pl[0]);
                    pts.Add(pl[1]);

                    Point3d prevSearch = pl[0];
                    Point3d lastSearch = pl[1];

                    bool lookRight = true;
                    bool lookDown = true;

                    for (int y = 0; y < 3; y++)
                    {
                        List<Polyline> connected = linesByPoint[lastSearch];
                        Polyline pl2 = null;

                        if (connected.Count == 1)
                        {
                            pl2 = connected[0];
                        }
                        else
                        {
                            if (lastSearch.X == prevSearch.X)
                            {
                                //Search for a change in X
                                if (lookRight)
                                {

                                    pl2 = FindRight(connected, lastSearch);
                                    if (pl2 == null)
                                    {
                                        //Something hasn't worked, perhaps we should have been looking to the left first instead...
                                        y--;
                                        lookRight = false;
                                        continue; //Try again
                                    }
                                    lookDown = true; //Look down on the next run through this rectangle
                                }
                                else
                                {
                                    pl2 = FindLeft(connected, lastSearch);
                                    if (pl2 == null)
                                    {
                                        //Something went wrong - try looking right instead
                                        y--;
                                        lookRight = true;
                                        continue; //Try again!
                                    }
                                    lookDown = false; //Look up on the next run through this rectangle
                                }

                            }
                            else
                            {
                                //Search for a change in Y
                                if (lookDown)
                                {
                                    pl2 = FindDown(connected, lastSearch);
                                    if (pl2 == null)
                                    {
                                        //Something went wrong - look up instead
                                        y--;
                                        lookDown = false;
                                        continue;
                                    }
                                    lookRight = false; //Look left on the next run through this rectangle
                                }
                                else
                                {
                                    pl2 = FindUp(connected, lastSearch);
                                    if (pl2 == null)
                                    {
                                        //Something went wrong - look down instead
                                        y--;
                                        lookDown = true;
                                        continue;
                                    }
                                    lookRight = true; //Look right next time through this rectangle
                                }

                            }
                        }

                        if (lastSearch == pl2[0])
                        {
                            pts.Add(pl2[1]);
                            prevSearch = lastSearch;
                            lastSearch = pl2[1];
                        }
                        else
                        {
                            pts.Add(pl2[0]);
                            prevSearch = lastSearch;
                            lastSearch = pl2[0];
                        }
                    }

                    if (pts.Last() == pts.First())
                        rects.Add(new Polyline(pts));
                }
            }

            return rects;
        }

        private List<Polyline> Tidy(List<Polyline> rects)
        {
            List<Polyline> rects2 = new List<Polyline>();
            for (int x = 0; x < rects.Count; x++)
            {
                bool match = false;
                foreach (Polyline pl in rects2)
                {
                    foreach (Point3d p3d in rects[x])
                    {
                        if (pl.Contains(p3d))
                        {
                            match = true;
                        }
                        else
                        {
                            match = false;
                            break;
                        }
                    }

                    if (match) break; //There was a total match found, no point checking anything else
                }

                if (!match)
                    rects2.Add(rects[x]);
            }

            return rects2;
        }

        private Polyline FindRight(List<Polyline> connected, Point3d lastSearch)
        {
            Polyline pl2 = connected.Where(x => x[0].X > lastSearch.X).FirstOrDefault();
            if (pl2 == null)
                pl2 = connected.Where(x => x[1].X > lastSearch.X).FirstOrDefault(); //Move to the right

            return pl2;
        }

        private Polyline FindLeft(List<Polyline> connected, Point3d lastSearch)
        {
            Polyline pl2 = connected.Where(x => x[0].X < lastSearch.X).FirstOrDefault();
            if (pl2 == null)
                pl2 = connected.Where(x => x[1].X < lastSearch.X).FirstOrDefault(); //Move to the left

            return pl2;
        }

        private Polyline FindUp(List<Polyline> connected, Point3d lastSearch)
        {
            Polyline pl2 = connected.Where(x => x[0].Y > lastSearch.Y).FirstOrDefault();
            if (pl2 == null)
                pl2 = connected.Where(x => x[1].Y > lastSearch.Y).FirstOrDefault(); //Move to the right (clockwise - up);

            return pl2;
        }

        private Polyline FindDown(List<Polyline> connected, Point3d lastSearch)
        {
            Polyline pl2 = connected.Where(x => x[0].Y < lastSearch.Y).FirstOrDefault();
            if (pl2 == null)
                pl2 = connected.Where(x => x[1].Y < lastSearch.Y).FirstOrDefault(); //Move to the right (clockwise - down)

            return pl2;
        }
    }
}
