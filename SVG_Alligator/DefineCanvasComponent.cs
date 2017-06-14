using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using BHoM;

namespace SVG_Alligator
{
    public class DefineCanvasComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DefineCanvasComponent class.
        /// </summary>
        public DefineCanvasComponent()
          : base("SVG Canvas", "SVG Canvas",
              "Description",
              "Alligator", "SVG")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Objects", "Obj", "The BHoM geometry to plot", GH_ParamAccess.list);
            pManager.AddNumberParameter("Height", "H", "The height of the canvas", GH_ParamAccess.item);
            pManager.AddNumberParameter("Width", "W", "The Width of the canvas", GH_ParamAccess.item);
            pManager.AddNumberParameter("Offset", "O", "The offset of the canvas", GH_ParamAccess.item);
            pManager.AddNumberParameter("Scale", "S", "The scale of the canvas", GH_ParamAccess.item);

            pManager[3].Optional = true;
            pManager[4].Optional = true;

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Canvas", "C", "Canvas", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Dictionary<string, object> canvasData = new Dictionary<string, object>();

            double Width = 0;
            double Height = 0;
            double Scale = 0;
            double Offset = 0;
            List<BHoM.Geometry.GeometryBase> Objects = new List<BHoM.Geometry.GeometryBase>();


            ///////////////////////DEFINE STYLE/////////////////////////////

            if (!DA.GetDataList<BHoM.Geometry.GeometryBase>(0, Objects)) { return; }
            if (!DA.GetData(1, ref Height)) { return; }
            if (!DA.GetData(2, ref Width)) { return; }
            DA.GetData(3, ref Offset);
            DA.GetData(4, ref Scale);
            

            BHoM.Geometry.BoundingBox canvasBounds = defineCanvasBounds(Objects);

            double canvasWidth = canvasBounds.Max.X - canvasBounds.Min.X;
            double canvasHeight = canvasBounds.Max.Y - canvasBounds.Min.Y;
            double Scale1 = (Width - (Offset * 2)) / canvasWidth;
            double Scale2 = (Height - (Offset * 2)) / canvasHeight;

            double cScale = 0;

            if (Scale == 0)
            {
                cScale = Math.Min(Scale1, Scale2);
            }

            else
            {
                cScale = (double)Scale;
            }


            canvasData.Add("Origin", canvasBounds.Min);
            canvasData.Add("Scale", cScale);
            canvasData.Add("Offset", Offset);
            canvasData.Add("Height", Height);
            canvasData.Add("Width", Width);
            canvasData.Add("xBound", canvasWidth);
            canvasData.Add("yBound", canvasHeight);

            DA.SetData(0, canvasData);

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

    }
}