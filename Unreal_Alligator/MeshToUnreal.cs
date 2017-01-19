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
    public class MeshToUnreal : GH_Component
    {
        public MeshToUnreal() : base("MeshToUnreal", "MeshToUnreal", "Convert Mesh into Unreal Message", "Alligator", "Unreal") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("1b1c313e-8e9b-4190-9cee-a0a8aabdd377");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Meshes", "Meshes", "Meshes to convert", GH_ParamAccess.list);
            pManager.AddGenericParameter("Colors", "Colors", "Colors to use for material", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Mesh Message", "Mesh Messsage", "Converted Mesh Message", GH_ParamAccess.item);

        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            List<R.Mesh> Meshes = DataUtils.GetDataList<R.Mesh>(DA, 0);
            List<string> Colors = DataUtils.GetDataList<string>(DA, 1);

            //Add Message Type
            string json = "[[[BHoMMeshes]]]";


            //Add Color Message
            json += ",[";
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

                json += "[" + ColorName + "],";
            }
            json = json.Trim(',') + "]";


            //Add Mesh Message
            json += ",[";
            for (int i = 0; i < Meshes.Count; i++)
            {
                json += "[[";
                json += "{\"vertices\": [";

                foreach (R.Point3d vertex in Meshes[i].Vertices)
                {
                    json += "[" + vertex.X.ToString("0.000") + "," + vertex.Y.ToString("0.000") + "," + vertex.Z.ToString("0.000") + "],";
                }
                json = json.Trim(',') + "], \"faces\": [";


                foreach (R.MeshFace face in Meshes[i].Faces)
                {
                    if (face.IsQuad)
                        json += face.A + "," + face.B + "," + face.C + "," + face.D + "," + face.B + "," + face.C + ",";
                    else
                        json += face.C + "," + face.B + "," + face.A + ",";
                }
                json = json.Trim(',') + "], \"normals\": [";


                foreach (R.Vector3f normal in Meshes[i].Normals)
                {
                    json += "[" + normal.X.ToString("0.000") + "," + normal.Y.ToString("0.000") + "," + normal.Z.ToString("0.000") + "],";
                }

                json = json.Trim(',') + "]}";
                json += "]],";
            }
            json = json.Trim(',') + "]";
            DA.SetData(0, json);


        }
    }
}
