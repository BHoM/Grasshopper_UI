using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using BHoM;
using System.Collections;
using Grasshopper.Kernel.Types;

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
            pManager.AddGenericParameter("Geometry", "G", "The BHoM geometry to plot", GH_ParamAccess.list);
            pManager.AddGenericParameter("Style", "S", "Style", GH_ParamAccess.item);

            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SVG Object", "O", "SVG Object", GH_ParamAccess.item);
            pManager.AddTextParameter("SVG String", "SVG", "SVG String", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            string svgText = null;

            IGH_Goo StyleData = null;
            string StyleName = null;
            Dictionary<string, object> StyleDict = new Dictionary<string, object>();
            List <BHoM.Geometry.GeometryBase> Objects = new List<BHoM.Geometry.GeometryBase>();

            if (!DA.GetDataList<BHoM.Geometry.GeometryBase>(0, Objects)) { return; }
            DA.GetData(1, ref StyleData);

            ///////////////////////CAST STYLE/////////////////////////////
            if (StyleData != null)
            {
                if (typeof(string).IsAssignableFrom(StyleData.GetType()))
                {
                    ((IGH_Goo)StyleData).CastTo<string>(out StyleName);

                    svgText = "<g " + "class=\"" + StyleName + "\" >" + System.Environment.NewLine
                        + "_svgObjectString"
                        + "</g>" + System.Environment.NewLine;

                }

                else if (StyleData is Grasshopper.Kernel.Types.GH_String)
                {
                    ((IGH_Goo)StyleData).CastTo<string>(out StyleName);

                    svgText = "<g " + "class=\"" + StyleName + "\" >" + System.Environment.NewLine
                        + "_svgObjectString"
                        + "</g>" + System.Environment.NewLine;

                }

                else if (typeof(IDictionary).IsAssignableFrom(StyleData.GetType()))
                {
                    ((IGH_Goo)StyleData).CastTo<Dictionary<string, object>>(out StyleDict);

                    string styleSting = unpackStyle(StyleDict);

                    svgText = "<g " + styleSting + ">" + System.Environment.NewLine
                        + "_svgObjectString"
                        + "</g>" + System.Environment.NewLine;
                }


                else if (StyleData is Grasshopper.Kernel.Types.GH_ObjectWrapper)
                {
                    (StyleData as Grasshopper.Kernel.Types.GH_ObjectWrapper).CastTo<Dictionary<string, object>>(out StyleDict);

                    string styleSting = unpackStyle(StyleDict);

                    svgText = "<g " + styleSting + ">" + System.Environment.NewLine
                        + "_svgObjectString"
                        + "</g>" + System.Environment.NewLine;

                }
            }
            else
            {
                svgText = "_svgObjectString";
            }

            ///////////////////////DRAW OBJECTS/////////////////////////////

            Dictionary<string, object> svgObjData = new Dictionary<string, object>();
            
            string objText = null;

            for (int i = 0; i < Objects.Count; i++)
            {
                string curveString = drawObject(Objects[i]);
                objText += curveString;
            }

            svgText = svgText.Replace("_svgObjectString", objText);
            BHoM.Geometry.BoundingBox bounds = defineBounds(Objects);

            svgObjData.Add("SVG", svgText);
            svgObjData.Add("bounds", bounds);

            DA.SetData(0, svgObjData);
            DA.SetData(1, svgText);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return null;
            }
        }

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

        private string DrawSVGPath(List<BHoM.Geometry.Point> ptList, bool closed)
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

        private string unpackStyle(Dictionary<string, object> StyleDict)
        {

            string svgStyle = null;
             
            if (StyleDict["Stroke"] != null)
            {
                string sString = "stroke=\"rgb(_rSVal, _gSVal, _bSVal)\" ";

                System.Drawing.Color Stroke = (System.Drawing.Color)StyleDict["Stroke"];
                sString = sString.Replace("_rSVal", Stroke.R.ToString());
                sString = sString.Replace("_gSVal", Stroke.G.ToString());
                sString = sString.Replace("_bSVal", Stroke.B.ToString());

                svgStyle += sString;

            }
            else
            {
                string sString = "stroke=\"none\" ";
                svgStyle += sString;
            }

            if (StyleDict["Fill"] != null)
            {
                string fString = "fill=\"rgb(_rFVal, _gFVal, _bFVal)\" ";

                System.Drawing.Color Fill = (System.Drawing.Color)StyleDict["Fill"];
                fString = fString.Replace("_rFVal", Fill.R.ToString());
                fString = fString.Replace("_gFVal", Fill.G.ToString());
                fString = fString.Replace("_bFVal", Fill.B.ToString());

                svgStyle += fString;
            }
            else
            {
                string fString = "fill=\"none\" ";
                svgStyle += fString;
            }

            if (StyleDict["Thickness"] != null)
            {

                string tString = "stroke-width=\"_stroke\" ";

                double sThickenss = (double)StyleDict["Thickness"];
                tString = tString.Replace("_stroke", sThickenss.ToString());

                svgStyle += tString;
            }
            else
            {
                string tString = "stroke-width=\"none\" ";
                svgStyle += tString;
            }

            if (StyleDict["Fill Opacity"] != null)
            {
                string FoString = "fill-opacity = \"_fillOpacity\" ";
                double fOpacity = (double)StyleDict["Fill Opacity"];
                FoString = FoString.Replace("_fillOpacity", fOpacity.ToString());
                svgStyle += FoString;
            }

            if (StyleDict["Stroke Dash"] != null && StyleDict["Stroke Gap"] != null)
            {
                string DaString = "stroke-dasharray=\"_dash,_gap\"";
                double dash = (double)StyleDict["Stroke Dash"];
                double gap = (double)StyleDict["Stroke Gap"];
                DaString = DaString.Replace("_dash", dash.ToString());
                DaString = DaString.Replace("_gap", gap.ToString());
                svgStyle += DaString;
            }

            return svgStyle;
        }

        private BHoM.Geometry.BoundingBox defineBounds(List<BHoM.Geometry.GeometryBase> objList)
        {


            List<BHoM.Geometry.Point> ptList = new List<BHoM.Geometry.Point>();

            List<List<double>> outputList = new List<List<double>>();

            List<double> xList = new List<double>();
            List<double> yList = new List<double>();
            List<double> zList = new List<double>();

            for (int i = 0; i < objList.Count; i++)
            {
                var geometry = objList[i];

                if (geometry != null)
                {
                    if (geometry is BHoM.Geometry.Point)
                    {
                        BHoM.Geometry.Point Pt = geometry as BHoM.Geometry.Point;

                        ptList.Add(Pt);
                    }

                    else if (geometry is BHoM.Geometry.Line)
                    {
                        BHoM.Geometry.Line L1 = geometry as BHoM.Geometry.Line;
                        BHoM.Geometry.BoundingBox bBox = L1.Bounds();

                        ptList.Add(bBox.Min);
                        ptList.Add(bBox.Max);
                    }

                    else if (geometry is BHoM.Geometry.Circle)
                    {
                        BHoM.Geometry.Circle C1 = geometry as BHoM.Geometry.Circle;
                        BHoM.Geometry.BoundingBox bBox = C1.Bounds();

                        ptList.Add(bBox.Min);
                        ptList.Add(bBox.Max);
                    }
                    else if (geometry is BHoM.Geometry.Curve)
                    {
                        BHoM.Geometry.Curve C1 = geometry as BHoM.Geometry.Curve;
                        BHoM.Geometry.BoundingBox bBox = C1.Bounds();

                        ptList.Add(bBox.Min);
                        ptList.Add(bBox.Max);
                    }
                    else if (geometry is BHoM.Geometry.Surface)
                    {
                        BHoM.Geometry.Surface S1 = geometry as BHoM.Geometry.Surface;
                        BHoM.Geometry.BoundingBox bBox = S1.Bounds();

                        ptList.Add(bBox.Min);
                        ptList.Add(bBox.Max);
                    }
                    else if (geometry is BHoM.Geometry.Brep)
                    {
                        BHoM.Geometry.Brep B1 = geometry as BHoM.Geometry.Brep;
                        BHoM.Geometry.BoundingBox bBox = B1.Bounds();

                        ptList.Add(bBox.Min);
                        ptList.Add(bBox.Max);
                    }
                    else if (geometry is BHoM.Geometry.Mesh)
                    {
                        BHoM.Geometry.Mesh M1 = geometry as BHoM.Geometry.Mesh;
                        BHoM.Geometry.BoundingBox bBox = M1.Bounds();

                        ptList.Add(bBox.Min);
                        ptList.Add(bBox.Max);
                    }


                }
            }


            BHoM.Geometry.BoundingBox CanvasBox = new BHoM.Geometry.BoundingBox(ptList);

            return CanvasBox;

        }

    }
}