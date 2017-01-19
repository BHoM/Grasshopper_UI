using BHoM.Structural.Elements;
using BH = BHoM.Geometry;
using BHG = BHoM.Geometry;
using R = Rhino.Geometry;
using Grasshopper.Kernel;
using Grasshopper_Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alligator.Unreal
{
    public class SplineToUnreal : GH_Component
    {
        public SplineToUnreal() : base("SplineToUnreal", "SplineToUnreal", "Curves into Unreal Message", "Alligator", "Unreal") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("f81f70fa-7178-40c4-9d8f-34ce47234a93");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Polylines", "Polylines", "Polylines to convert", GH_ParamAccess.list);
            pManager.AddGenericParameter("Colors", "Colors", "Colors to use for material", GH_ParamAccess.list);
            pManager.AddTextParameter("Unreal Material", "Unreal Material", "Material to use in Unreal", GH_ParamAccess.item);

            pManager[1].Optional = true;
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Spline Message", "Spline Messsage", "Converted Spline Message", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            List<R.Curve> CurveList = DataUtils.GetDataList<R.Curve>(DA, 0);
            List<string> Colors = DataUtils.GetDataList<string>(DA, 1);
            string Material = DataUtils.GetData<string>(DA, 2);
            List<R.Polyline> PolylineList = new List<R.Polyline>();
            foreach (R.Curve Curve in CurveList) 
            {
                R.Polyline PolyToAdd = new R.Polyline(); 
                Curve.TryGetPolyline(out PolyToAdd);
                PolylineList.Add(PolyToAdd);

            }
            {

            }
            //Add Message Type
            string json = "[[[BHoMSplines]]]";


            //Add Material Message
            json += ",[[[" + Material + "]]]";


            //Create Polylinelist
            json += ",[";
            foreach ( R.Polyline Polyline in PolylineList)
            {
                json += "[";
                foreach ( R.Point3d Point in Polyline)
                {
                    json += "[" + Math.Round(Point.X, 0) + "," + Math.Round(Point.Y, 0) + "," + Math.Round(Point.Z, 0) + "],";
                }
                json = json.Trim(',') + "],";
            }
            json = json.Trim(',') + "]";


            //Add Color Message
            List<int> ColorIndex = new List<int>();
            List<string> ColorNames = new List<string>();
            json += ",[";
            int totalLength = 0;
            if (0 >= Colors.Count)
            {
                json += "[[]],";
            }
            else foreach (R.Polyline Polyline in PolylineList)
                {
                    json += "[";
                    for (int i = 0; i < Polyline.Count; i++)
                    {
                        List<string> colorStrings = Colors[totalLength + i].Split(new Char[] { ',' }, StringSplitOptions.None).ToList();
                        double colorScale = 1.00 / 255.00;
                        string ColorName = "[";
                        for (int k = 0; k < colorStrings.Count; k++)
                        {
                            double colorValue = Math.Round(colorScale * double.Parse(colorStrings[k]), 3);
                            ColorName += colorValue + ",";
                        }
                        ColorName = ColorName.Trim(',') + "]";
                        json += ColorName + ",";
                    }
                    json = json.Trim(',') + "],";
                    totalLength = totalLength + Polyline.Count;
                }
            json = json.Trim(',') + "]";
            DA.SetData(0, json);

        }
    }
}
