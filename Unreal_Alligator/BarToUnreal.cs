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
        public BarToUnreal() : base("BarToUnreal", "BarToUnreal", "Convert BHoM Bar into Unreal Message", "Alligator", "Unreal") { }

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
            pManager.AddTextParameter("Bar Message", "Bar Messsage", "Converted Bar Message", GH_ParamAccess.item);
            pManager.AddMeshParameter("Unreal Bar Brep", "Unreal Bar Brep", "Brep to build in Unreal", GH_ParamAccess.list);
            pManager.AddTextParameter("Unreal Bar Names", "Unreal Bar Names", "Name for Unreal Bar", GH_ParamAccess.list);


        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            List<BHoM.Structural.Elements.Bar> barList = DataUtils.GetDataList<Bar>(DA, 0);
            List<string> Colors = DataUtils.GetDataList<string>(DA, 1);
            string Material = DataUtils.GetData<string>(DA, 2);
            int decimals = 0;

            //Create Unique BarBreps
            List<int> SectionPropertyIndex = new List<int>();
            List<R.Mesh> UnrealMeshList = new List<R.Mesh>();
            List<string> SectionProperties = new List<string>();
            for (int i = 0; i < barList.Count; i++)
            {
                if (barList[i] != null && barList[i].SectionProperty != null)
                {
                    if (SectionProperties.Contains(barList[i].SectionProperty.Name.Replace(" ", string.Empty)))
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
                        List<R.Mesh> UnrealMeshParts = new List<Rhino.Geometry.Mesh>();
                        UnrealMeshParts = R.Mesh.CreateFromBrep(UnrealBrep, R.MeshingParameters.Coarse).ToList();
                        R.Mesh UnrealMesh = new Rhino.Geometry.Mesh();
                        foreach (R.Mesh partialMesh in UnrealMeshParts)
                        {
                            UnrealMesh.Append(partialMesh);
                        }
                        UnrealMeshList.Add(UnrealMesh);
                        SectionProperties.Add(barList[i].SectionProperty.Name.Replace(" ", string.Empty));
                    }
                }
            }
            DA.SetDataList(1, UnrealMeshList);
            DA.SetDataList(2, SectionProperties);


            //Add Message Type
            string json = "[[[BHoMBars]]]";


            //Add Material Message
            json += ",[[[" + Material + "]]]";


            //Add SectionProperty Message
            json += ",[[";
            for (int i = 0; i < SectionProperties.Count; i++)
            {
                json += "[" + SectionProperties[i] + "],";
            }
            json = json.Trim(',') + "]]";


            //Add Color Message
            List<int> ColorIndex = new List<int>();
            List<string> ColorNames = new List<string>();
            json += ",[[";
            if (0 >= Colors.Count)
            {
                json += "[],";
            }
            else
            {
                for (int i = 0; i < Colors.Count; i++)
                {

                    List<string> colorStrings = Colors[i].Split(new Char[] { ',' }, StringSplitOptions.None).ToList();
                    double colorScale = 1.00 / 255.00;
                    string ColorName = "[";
                    for (int j = 0; j < colorStrings.Count; j++)
                    {
                        double colorValue = Math.Round(colorScale * double.Parse(colorStrings[j]), 3);
                        ColorName += colorValue + ",";
                    }
                    ColorName = ColorName.Trim(',') + "]";

                    if (ColorNames.Contains(ColorName))
                    {
                        ColorIndex.Add(ColorNames.IndexOf(ColorName));
                    }
                    else
                    {
                        ColorNames.Add(ColorName);
                        ColorIndex.Add(ColorNames.IndexOf(ColorName));
                    }

                }
                for (int i = 0; i < ColorNames.Count; i++)
                {
                    json += ColorNames[i] + ",";
                }
            }
            json = json.Trim(',') + "]]";


            //Add Points Message.
            List<string> Points = new List<string>();
            List<int> StartPointIndex = new List<int>();
            List<int> EndPointIndex = new List<int>();
            for (int i = 0; i < barList.Count; i++)
            {
                string pointstring = "[" + Math.Round(barList[i].StartPoint.X, decimals, MidpointRounding.AwayFromZero) + "," + Math.Round(barList[i].StartPoint.Y, decimals, MidpointRounding.AwayFromZero) + "," + Math.Round(barList[i].StartPoint.Z, decimals, MidpointRounding.AwayFromZero) + "]";

                if (Points.Contains(pointstring))
                { StartPointIndex.Add(Points.IndexOf(pointstring));
                    }
                else
                {
                    Points.Add(pointstring);
                    StartPointIndex.Add(Points.IndexOf(pointstring));
                }

                pointstring = "[" + Math.Round(barList[i].EndPoint.X, decimals, MidpointRounding.AwayFromZero) + "," + Math.Round(barList[i].EndPoint.Y, decimals, MidpointRounding.AwayFromZero) + "," + Math.Round(barList[i].EndPoint.Z, decimals, MidpointRounding.AwayFromZero) + "]";
                if (Points.Contains(pointstring))
                {
                    EndPointIndex.Add(Points.IndexOf(pointstring));
                }
                else
                {
                    Points.Add(pointstring);
                    EndPointIndex.Add(Points.IndexOf(pointstring));
                }
            }

            json += ",[[";
            for (int i = 0; i < Points.Count; i++)
            {
                json += Points[i] + ",";
            }
            json = json.Trim(',') + "]]";


            //Add Bar Message

            json += ",[";

            for (int i = 0; i < barList.Count; i++)
            {
                json += "[[" + SectionProperties.IndexOf(barList[i].SectionProperty.Name.Replace(" ", string.Empty)) + "],[" + ColorIndex[i] + "],[" + StartPointIndex[i] + "," + EndPointIndex[i] + "]],";
            }
            json = json.Trim(',') + "]";
            DA.SetData(0, json);

        }
    }
}
