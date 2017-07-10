using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using BHoM;

namespace SVG_Alligator
{
    public class SVGObjectComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PlotObjectComponent class.
        /// </summary>
        public SVGObjectComponent()
          : base("SVG Object", "SVG Object",
              "Description2",
              "Alligator", "SVG")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Objects", "O", "The BHoM geometry to plot", GH_ParamAccess.list);
            pManager.AddGenericParameter("Style", "S", "Style", GH_ParamAccess.item);

            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("SVG String", "S", "SVG String", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            string svgText = null;
            Dictionary<string, object> StyleData = new Dictionary<string, object>();
            List<BHoM.Geometry.GeometryBase> Objects = new List<BHoM.Geometry.GeometryBase>();


            ///////////////////////DEFINE STYLE/////////////////////////////

            if (!DA.GetDataList<BHoM.Geometry.GeometryBase>(0, Objects)) { return; }
            DA.GetData(1, ref StyleData);
            

            if (StyleData.Count > 0)
            {

                svgText += "<g ";   

                if (StyleData["Stroke"] != null)
                {
                    string sString = "stroke=\"rgb(_rSVal, _gSVal, _bSVal)\" ";

                    System.Drawing.Color Stroke = (System.Drawing.Color)StyleData["Stroke"];
                    sString = sString.Replace("_rSVal", Stroke.R.ToString());
                    sString = sString.Replace("_gSVal", Stroke.G.ToString());
                    sString = sString.Replace("_bSVal", Stroke.B.ToString());

                    svgText += sString;

                }
                else
                {
                    string sString = "stroke=\"none\" ";
                    svgText += sString;
                }

                if (StyleData["Fill"] != null)
                {
                    string fString = "fill=\"rgb(_rFVal, _gFVal, _bFVal)\" ";

                    System.Drawing.Color Fill = (System.Drawing.Color)StyleData["Fill"];
                    fString = fString.Replace("_rFVal", Fill.R.ToString());
                    fString = fString.Replace("_gFVal", Fill.G.ToString());
                    fString = fString.Replace("_bFVal", Fill.B.ToString());

                    svgText += fString;
                }
                else
                {
                    string fString = "fill=\"none\" ";
                    svgText += fString;
                }

                if (StyleData["Thickness"] != null)
                {

                    string tString = "stroke-width=\"_stroke\" ";

                    double sThickenss = (double)StyleData["Thickness"];
                    tString = tString.Replace("_stroke", sThickenss.ToString());

                    svgText += tString;
                }
                else
                {
                    string tString = "stroke-width=\"none\" ";
                    svgText += tString;
                }

                svgText += ">" + System.Environment.NewLine;
            }


            ///////////////////////DRAW OBJECTS/////////////////////////////


            for (int i = 0; i < Objects.Count; i++)
            {
                string curveString = drawObject(Objects[i]);
                svgText += curveString;
            }


            if (StyleData.Count > 0)
            {
                svgText += "</g>" + System.Environment.NewLine;
            }

            DA.SetData(0, svgText);


        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("addfab13-7cd6-442f-9d63-0ad9163466ef"); }
        }

        private string drawObject(object geometry)
        {
            string crvSvgText = null;

            if (geometry != null)
            {
                if (geometry is BHoM.Geometry.Point)
                {
                    BHoM.Geometry.Point Pt = geometry as BHoM.Geometry.Point;

                    //TODO
                }

                else if (geometry is BHoM.Geometry.Line)
                {
                    BHoM.Geometry.Line L1 = geometry as BHoM.Geometry.Line;

                    crvSvgText = DrawSVGLine(L1.StartPoint, L1.EndPoint);

                }

                else if (geometry is BHoM.Geometry.Circle)
                {
                    BHoM.Geometry.Circle C1 = geometry as BHoM.Geometry.Circle;

                    //TODO

                }
                else if (geometry is BHoM.Geometry.Curve)
                {


                    BHoM.Geometry.Curve C1 = geometry as BHoM.Geometry.Curve;

                    List<BHoM.Geometry.Point> crvPts = C1.ControlPoints;

                    bool closed = false;

                    if (C1.IsClosed())
                    {
                        closed = true;
                    }

                    crvSvgText = DrawSVGPath(crvPts, closed);

                    //TODO
                }


                else if (geometry is BHoM.Geometry.NurbCurve)
                {
                    BHoM.Geometry.Curve C1 = geometry as BHoM.Geometry.NurbCurve;

                    List<BHoM.Geometry.Point> crvPts = C1.ControlPoints;

                    bool closed = false;

                    if (C1.IsClosed())
                    {
                        closed = true;
                    }

                    crvSvgText = DrawSVGPath(crvPts, closed);


                    //TODO
                }

                else if (geometry is BHoM.Geometry.Surface)
                {
                    BHoM.Geometry.Surface S1 = geometry as BHoM.Geometry.Surface;

                    //TODO

                }
                else if (geometry is BHoM.Geometry.Brep)
                {
                    BHoM.Geometry.Brep B1 = geometry as BHoM.Geometry.Brep;

                    //TODO

                }
                else if (geometry is BHoM.Geometry.Mesh)
                {
                    BHoM.Geometry.Mesh M1 = geometry as BHoM.Geometry.Mesh;

                    //TODO

                }


            }


            return crvSvgText;
        }

        private string DrawSVGLine(BHoM.Geometry.Point startPt, BHoM.Geometry.Point endPt)
        {

            string lineString = "<line x1=\"_x1\" y1=\"_y1\" z1=\"_z1\" x2=\"_x2\" y2=\"_y2\" z2=\"_z2\" vector-effect=\"non-scaling-stroke\" />" + System.Environment.NewLine;

            lineString = lineString.Replace("_x1", startPt.X.ToString());
            lineString = lineString.Replace("_y1", startPt.Y.ToString());
            lineString = lineString.Replace("_z1", startPt.Z.ToString());
            lineString = lineString.Replace("_x2", endPt.X.ToString());
            lineString = lineString.Replace("_y2", endPt.Y.ToString());
            lineString = lineString.Replace("_z2", endPt.Z.ToString());

            return lineString;

        }

        public string DrawSVGPath(List<BHoM.Geometry.Point> ptList, bool closed)
        {

            string pathString = "<path d=\"";


            for (int i = 0; i < (ptList.Count); i++)
            {

                if (i == 0)
                {

                    pathString += "M " + ptList[i].X.ToString() + " " + ptList[i].Y.ToString() + " ";
                }
                else
                {


                    pathString += "L " + ptList[i].X.ToString() + " " + ptList[i].Y.ToString() + " ";

                }

            }

            if (closed)
            {

                pathString += "Z";
            }

            pathString += "\" vector-effect=\"non-scaling-stroke\" />" + System.Environment.NewLine;

            return pathString;
        }

    }
}