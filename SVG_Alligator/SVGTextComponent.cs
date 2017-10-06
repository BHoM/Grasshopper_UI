using System;
using System.Collections.Generic;
using System.Drawing;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace SVG_Alligator
{
    public class SVGTextComponent : GH_Component
    {

        public SVGTextComponent()
          : base("SVG Text", "SVG Text",
              "Create SVG Text",
              "Alligator", "SVG")
        {
        }

        private readonly List<string> m_text = new List<string>();
        private readonly List<Point3d> m_point = new List<Point3d>();
        private double m_size = 1;
        private string m_font = "Arial";

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Location", "L", "Point location of the text", GH_ParamAccess.item);
            pManager.AddTextParameter("Text", "T", "Text to plot", GH_ParamAccess.item);
            pManager.AddColourParameter("Colour", "C", "Colour of the text. Default set to Black.", GH_ParamAccess.item);
            pManager.AddTextParameter("Font", "F", "Font. Default set to Arial", GH_ParamAccess.item);
            pManager.AddNumberParameter("Size", "S", "Size. Default set to 1", GH_ParamAccess.item);

            pManager[2].Optional = true;
            pManager[3].Optional = true;
            pManager[4].Optional = true;
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("SVG String", "S", "SVG String", GH_ParamAccess.item);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {

            m_text.Clear();
            m_point.Clear();

            double scaleFactor = 1.4;
            Point3d Location = new Point3d();
            string Text = null;
            System.Drawing.Color Colour = System.Drawing.Color.Black;
            string font = "Arial";
            double FontHeight = 1;

            if (!DA.GetData(0, ref Location)) { return; };
            if (!DA.GetData(1, ref Text)) { return; };
            DA.GetData(2, ref Colour);
            DA.GetData(3, ref font);
            DA.GetData(4, ref FontHeight);


            //----------------Set Global Variables ------------------//

            m_text.Add(Text);
            m_point.Add(Location);
            m_size = FontHeight;

            if (font != null)
            {
                m_font = font;
            }

            //----------------Build SVG String ------------------//

            string SVGString = null;

            string color = "rgb(_rSVal, _gSVal, _bSVal)";
            color = color.Replace("_rSVal", Colour.R.ToString());
            color = color.Replace("_gSVal", Colour.G.ToString());
            color = color.Replace("_bSVal", Colour.B.ToString());

            string size = (m_size * scaleFactor).ToString();

            string xLocation = Location.X.ToString();
            string yLocation = Location.Y.ToString();
            string yTrasform = (Location.Y * 2).ToString();

            SVGString += " <text x = \"" + xLocation + "\" y = \"" + yLocation + "\" " + "transform= \"translate(0," + yTrasform + ") scale(1,-1)\"" + " font-family=\"" + m_font + "\" font-size=\"" + size + "px\" fill = \"" + color + "\" > " + Text + "</text>" + System.Environment.NewLine;

            string SVGText = SVGString;

            DA.SetData(0, SVGText);
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
            get { return new Guid("c07541c2-15b2-4614-8b67-88a3cee3f00e"); }
        }


        public override void DrawViewportWires(IGH_PreviewArgs args)
        {
            if (m_text.Count == 0)
                return;

            Plane plane = Plane.WorldXY;
            //args.Viewport.GetFrustumFarPlane(out plane);

            for (int i = 0; i < m_text.Count; i++)
            {
                string text = m_text[i];
                Point3d point = m_point[i];
                plane.Origin = point;

                args.Display.Draw3dText(text, Color.DarkRed, plane, m_size, m_font);

            }
        }

    }
}