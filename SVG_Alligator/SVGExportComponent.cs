using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using BHoM;

namespace SVG_Alligator
{
    public class SVGExportComponent : GH_Component
    {

        public SVGExportComponent()
          : base("SVG Export", "SVG Export",
              "Description",
              "Alligator", "SVG")
        {
        }


        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Objects", "Obj", "The BHoM geometry to plot", GH_ParamAccess.list);
            pManager.AddNumberParameter("Canvas Height", "H", "The height of the canvas", GH_ParamAccess.item);
            pManager.AddNumberParameter("Canvas Width", "W", "The Width of the canvas", GH_ParamAccess.item);
            pManager.AddNumberParameter("Canvas Offset", "O", "The offset of the canvas", GH_ParamAccess.item);
            pManager.AddTextParameter("File Path", "P", "File Path", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Run", "R", "Run", GH_ParamAccess.item);

            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
            pManager[4].Optional = true;
            pManager[5].Optional = true;
        }
        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SVG", "SVG", "SVG Text", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            double Width = 1000;
            double Height = 1000;
            double Offset = 10;
            List<Dictionary<string, object>> Objects = new List<Dictionary<string, object>>();
            string FilePath = null;
            bool Run = false;
            

            if (!DA.GetDataList<Dictionary<string, object>>(0, Objects)) { return; }
            DA.GetData(1, ref Height);
            DA.GetData(2, ref Width);
            DA.GetData(3, ref Offset);
            DA.GetData(4, ref FilePath);
            DA.GetData(5, ref Run);

            ///////////////////////DEFINE CANVAS BOUNDS/////////////////////////////

            BHoM.Geometry.BoundingBox canvasBounds = null;
            List<string> svgObjectList = new List<string>();

            for (int i = 0; i < Objects.Count; i++)
            {
                if(i == 0)
                {
                    canvasBounds = (BHoM.Geometry.BoundingBox) Objects[i]["bounds"];
                }
                else
                {
                    canvasBounds.Merge((BHoM.Geometry.BoundingBox)Objects[i]["bounds"]);
                }

                svgObjectList.Add((string)Objects[i]["SVG"]);
            }

            double objectWidth = canvasBounds.Max.X - canvasBounds.Min.X;
            double objectHeight = canvasBounds.Max.Y - canvasBounds.Min.Y;
            double Scale1 = (Width - (Offset * 2)) / objectWidth;
            double Scale2 = (Height - (Offset * 2)) / objectHeight;
            double Scale = Math.Min(Scale1, Scale2);
            BHoM.Geometry.Point Origin = canvasBounds.Min;

            ///////////////////////DEFINE CANVAS BOUNDS/////////////////////////////

            BHoM.Geometry.Vector transPt = new BHoM.Geometry.Point(0, 0, 0) - Origin;
            BHoM.Geometry.Vector reversePt = new BHoM.Geometry.Vector(transPt.X, -transPt.Y, transPt.Z);

            double xTrans = transPt.X;
            double yTrans = transPt.Y;

            double xScale = Scale;
            double yScale = -Scale;

            string svgText = null;

            ///////////////////////DEFINE CANVAS STRING/////////////////////////////

            svgText += "<svg height=\"" + Height.ToString() + "\" width=\"" + Width.ToString() + "\" xmlns=\"http://www.w3.org/2000/svg\">" + System.Environment.NewLine;

            svgText += "<g transform= \"translate(_xOffset, _yOffset) translate(_xCorrect, _yCorrect) scale(_xScale, _yScale) translate(_xTrans, _yTrans)\">" + System.Environment.NewLine;

            svgText = svgText.Replace("_xScale", xScale.ToString());
            svgText = svgText.Replace("_yScale", yScale.ToString());

            svgText = svgText.Replace("_xTrans", xTrans.ToString());
            svgText = svgText.Replace("_yTrans", yTrans.ToString());

            svgText = svgText.Replace("_xCorrect", "0");
            svgText = svgText.Replace("_yCorrect", Height.ToString());

            svgText = svgText.Replace("_xOffset", Offset.ToString());
            svgText = svgText.Replace("_yOffset", "-" + Offset.ToString());

            //ADD OBJECTS

            for (int i = 0; i < svgObjectList.Count; i++)
            {
                svgText += svgObjectList[i];

            }
            svgText += "</g>" + System.Environment.NewLine;
            svgText += "</svg>";

            //WRITE TO FILE

            if (Run)
            {
                WrtiteToFile(svgText, FilePath);
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
                return null;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("0ed7f0e7-4252-48b8-bb24-42cb08b2370b"); }
        }

        private BHoM.Geometry.BoundingBox defineCanvasBounds(List<BHoM.Geometry.GeometryBase> objList)
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

        public void WrtiteToFile(string SVGString, string path)
        {

            string fp = "@\"" + path + ".svg\"";


            System.IO.File.WriteAllText(path, SVGString);

        }
    }
}