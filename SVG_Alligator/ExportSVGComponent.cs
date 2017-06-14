using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace SVG_Alligator
{
    public class ExportSVGComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ExportSVGComponent class.
        /// </summary>
        public ExportSVGComponent()
          : base("Export SVG", "Export SVG",
              "Description",
              "Alligator", "SVG")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Run", "R", "Run", GH_ParamAccess.item);
            pManager.AddGenericParameter("Canvas", "C", "Canvas", GH_ParamAccess.item);
            pManager.AddGenericParameter("SVG Objects", "O", "SVG Objects", GH_ParamAccess.list);
            pManager.AddTextParameter("File Path", "P", "File Path", GH_ParamAccess.item);
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

            Dictionary<string, object> CanvasData = new Dictionary<string, object>();
            List<String> Objects = new List<string>();
            bool Run = false;
            string FilePath = null;

            if (!DA.GetData(0, ref Run)) { return; }
            if (!DA.GetData(1, ref CanvasData)) { return; }
            if (!DA.GetDataList(2, Objects)) { return; }
            if (!DA.GetData(3, ref FilePath)) { return; }


            double Scale = (double)CanvasData["Scale"];
            double CanvasHeight = (double)CanvasData["Height"];
            double Offset = (double)CanvasData["Offset"];
            double yBounds = (double)CanvasData["yBound"];

            BHoM.Geometry.Point Origin = (BHoM.Geometry.Point)CanvasData["Origin"];
            BHoM.Geometry.Vector transPt = new BHoM.Geometry.Point(0, 0, 0) - Origin;
            BHoM.Geometry.Vector reversePt = new BHoM.Geometry.Vector(transPt.X, -transPt.Y, transPt.Z);

            double xTrans = transPt.X;
            double yTrans = transPt.Y;

            double xScale = Scale;
            double yScale = -Scale;

            string svgText = null;

            //DEFINE CANVAS

            svgText += "<svg height=\"" + CanvasData["Height"].ToString() + "\" width=\"" + CanvasData["Width"].ToString() + "\" xmlns=\"http://www.w3.org/2000/svg\">" + System.Environment.NewLine;

            //DEFINE TRANSFORM

            svgText += "<g transform= \"translate(_xOffset, _yOffset) translate(_xCorrect, _yCorrect) scale(_xScale, _yScale) translate(_xTrans, _yTrans)\">" + System.Environment.NewLine;

            svgText = svgText.Replace("_xScale", xScale.ToString());
            svgText = svgText.Replace("_yScale", yScale.ToString());

            svgText = svgText.Replace("_xTrans", xTrans.ToString());
            svgText = svgText.Replace("_yTrans", yTrans.ToString());

            svgText = svgText.Replace("_xCorrect", "0");
            svgText = svgText.Replace("_yCorrect", CanvasHeight.ToString());

            svgText = svgText.Replace("_xOffset", Offset.ToString());
            svgText = svgText.Replace("_yOffset", "-" + Offset.ToString());

            //ADD OBJECTS

            for (int i = 0; i < Objects.Count; i++)
            {
                svgText += Objects[i];

            }
            svgText += "</g>" + System.Environment.NewLine;
            svgText += "</svg>";

            DA.SetData(0, svgText);

            //WRITE TO FILE

            if (Run)
            {
                WrtiteToFile(svgText, FilePath);
            }

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
            get { return new Guid("e15702ca-c9f0-488c-a603-98d6b9b38fd5"); }
        }

        public void WrtiteToFile(string SVGString, string path)
        {

            string fp = "@\"" + path + ".svg\"";


            System.IO.File.WriteAllText(path, SVGString);

        }

        
    }
}