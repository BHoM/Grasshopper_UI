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
    public class BarToUnreal : GH_Component
    {
        public BarToUnreal() : base("BarToUnreal", "BarToUnreal", "Convert BHoM Bar into Unreal Message", "Alligator", "unreal") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("823bf0ae-717c-49a4-bca4-a8e49e80d02e");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Bars", "Bars", "Bars to convert", GH_ParamAccess.list);
            pManager.AddGenericParameter("Colors", "Colors", "Colors to use for material", GH_ParamAccess.list);
            pManager.AddTextParameter("Unreal Material", "Unreal Material", "Material to use in Unreal", GH_ParamAccess.item);

            pManager[1].Optional = true;
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("BarMessage", "BarMesssage", "Converted Bar Message", GH_ParamAccess.item);
            pManager.AddBrepParameter("UnrealBarBrep", "UnrealBarBrep", "Brep to build in Unreal", GH_ParamAccess.list);
            pManager.AddTextParameter("UnrealBarNames", "UnrealBarNames", "Name for Unreal Bar", GH_ParamAccess.list);


        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<BHoM.Structural.Elements.Bar> barList = DataUtils.GetDataList<Bar>(DA, 0);
            List<string> Colors = DataUtils.GetDataList<string>(DA, 1);
            string Material = DataUtils.GetData<string>(DA, 2);
            //Add Message Type
            string json = "[[[BHoMBars]]]";

            //Add Material Message
            json += ",[[[" + Material + "]]]";

            //Add SectionProperty Message
            json += ",[[";
            for (int i = 0; i < barList.Count; i++)
            {
                json += "[" + barList[i].SectionProperty.Name + "],";
            }
            json = json.Trim(',') + "]]";

            //Add Color Message
            json += ",[[";
            if (0 >= Colors.Count)
            {
                json += "[],";
            }
            else
            {
                for (int i = 0; i < Colors.Count; i++)
                {

                    List<float> colorValues = new List<float>();
                    List<string> colorStrings = Colors[i].Split(new Char[] { ',' }, StringSplitOptions.None).ToList(); ;
                    float colorScale = 1 / 255;
                    json += "[";
                    for (int j = 0; j < colorStrings.Count; j++)
                    {
                        colorValues.Add(float.Parse(colorStrings[i]) * colorScale);
                        json += colorValues[j].ToString() + ",";
                    }
                    json = json.Trim(',') + "],";
                }
            }
            json = json.Trim(',') + "]]";

            //Add Bar Message.
            json += ",[";
            for (int i = 0; i < barList.Count; i++)
            {
                json += "[[" + barList[i].StartPoint.X + "," + barList[i].StartPoint.Y + "," + barList[i].StartPoint.Z + "],[" + barList[i].EndPoint.X + "," + barList[i].EndPoint.Y + "," + barList[i].EndPoint.Z + "]],";
            }
            json = json.Trim(',') + "]";

            DA.SetData(0, json);

            //Create Unique BarBreps
            List<R.Brep> UnrealBrepList = new List<R.Brep>();
            List<string> SectionProperties = new List<string>();
            for (int i = 0; i < barList.Count; i++)
            {
                if (barList[i] != null && barList[i].SectionProperty != null)
                {
                    if (SectionProperties.Contains(barList[i].SectionProperty.Name))
                    { }
                    else
                    {
                        R.Curve genCentreline = GeometryUtils.Convert(new BHG.Line(new BH.Point(0, 0, 0), new BH.Point(0, 0, 1)));
                        List<BH.Curve> curves = BH.Curve.Join(barList[i].SectionProperty.Edges);
                        curves.Sort(delegate (BH.Curve c1, BH.Curve c2)
                        {
                            return c2.Length.CompareTo(c1.Length);
                        });
                        R.Curve perimeter = GeometryUtils.Convert(curves[0]);
                        R.Brep UnrealBrep = GeometryUtils.ExtrudeAlong(perimeter, genCentreline, new R.Plane(R.Point3d.Origin, R.Vector3d.XAxis, R.Vector3d.YAxis))[0].ToBrep().CapPlanarHoles(0.01);
                        UnrealBrep.Translate(new R.Vector3d(0, barList[i].SectionProperty.CentreY * -1, 0));
                        UnrealBrepList.Add(UnrealBrep);
                        SectionProperties.Add(barList[i].SectionProperty.Name);
                    }
                }
            }
            DA.SetDataList(1, UnrealBrepList);
            DA.SetDataList(2, SectionProperties);

        }
    }
}
