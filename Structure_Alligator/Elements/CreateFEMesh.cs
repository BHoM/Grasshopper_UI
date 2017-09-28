//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Grasshopper.Kernel;
//using BH.oM.Structural.Elements;
//using BH.oM.Structural.Properties;
//using Rhino.Geometry;
//using GHE = BH.Engine.Grasshopper;

//namespace BH.UI.Alligator.Structural.Elements
//{
//    public class CreateFEMesh : GH_Component
//    {
//        public CreateFEMesh() : base("Create FEMesh", "FEMesh", "Create a BH FEmesh object", "Structure", "Elements") { }

//        /// <summary> Icon (24x24 pixels)</summary>
//        protected override System.Drawing.Bitmap Internal_Icon_24x24
//        {
//            get { return Alligator.Structural.Properties.Resources.BHoM_Mesh; }
//        }

//        public override Guid ComponentGuid
//        {
//            get
//            {
//                return new Guid("6F367478-D7A1-4AC7-825E-DAFEB56B77BA");
//            }
//        }

//        protected override void RegisterInputParams(GH_InputParamManager pManager)
//        {
//            pManager.AddMeshParameter("Mesh", "M", "Rhinomesh to convert", GH_ParamAccess.item);
//            pManager.AddGenericParameter("Panel Property", "P", "Panelproperty to apply to the mesh", GH_ParamAccess.item);
//            pManager.AddTextParameter("Name", "N", "Name of the mesh", GH_ParamAccess.item);
//            pManager.AddGenericParameter("Custom Data", "CD", "Customd ata for the mesh", GH_ParamAccess.item);

//            pManager[2].Optional = true;
//            pManager[3].Optional = true;
//        }

//        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
//        {
//            pManager.AddGenericParameter("FE-Mesh", "FEM", "The constructed FE-Mesh", GH_ParamAccess.item);
//        }

//        protected override void SolveInstance(IGH_DataAccess DA)
//        {
//            Mesh mesh = GHE.DataUtils.GetData<Mesh>(DA, 0);
//            PanelProperty prop = GHE.DataUtils.GetGenericData<PanelProperty>(DA, 1);

//            if (mesh == null || prop == null)
//                return;

//            FEMesh feMesh = new FEMesh();

//            feMesh.PanelProperty = prop;

//            for (int i = 0; i < mesh.Vertices.Count; i++)
//            {
//                feMesh.Nodes.Add(new Node(GHE.GeometryUtils.Convert(mesh.Vertices[i])));
//            }

//            foreach (MeshFace face in mesh.Faces)
//            {
//                FEFace feFace = new FEFace();

//                feFace.NodeIndices.Add(face.A);
//                feFace.NodeIndices.Add(face.B);
//                feFace.NodeIndices.Add(face.C);

//                if (face.IsQuad)
//                    feFace.NodeIndices.Add(face.D);

//                feMesh.Faces.Add(feFace);
//            }

//            string name = null;

//            if (DA.GetData(2, ref name))
//            {
//                feMesh.Name = name;
//            }

//            Dictionary<string, object> customData = GHE.DataUtils.GetGenericData<Dictionary<string, object>>(DA, 3);

//            if (customData != null)
//            {
//                foreach (KeyValuePair<string,object> kvp in customData)
//                {
//                    feMesh.CustomData[kvp.Key] = kvp.Value;
//                }
//            }

//            DA.SetData(0, feMesh);
//        }
//    }
//}
