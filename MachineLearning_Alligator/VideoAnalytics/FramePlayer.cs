using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Rhino.Geometry;
using BH.UI.Alligator;
using BH.Engine.MachineLearning;

namespace StadiaCrowdAnalytics_Alligator
{
    public class FramePlayer : GH_Component
    {
        private Guid _refID;

        public FramePlayer()
            : base("StadiaFramePlayer", "StadiaFramePlayer",
                  "Play the frames of the video file",
                  "Alligator", "MachineLearning")
        {
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("20EDB541-CE97-425B-9AF5-063F9FC85026"); }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Folder", "Folder", "Folder that contains the frames", GH_ParamAccess.item);
            pManager.AddBooleanParameter("DisplayAnalysedFrames", "DisplayAnalysedFrames", "Display the analysed frames or unanalysed frames? Default - display analysed frames (true)", GH_ParamAccess.item, true);
            pManager.AddNumberParameter("Frame number", "Frame number", "Which frame to display?", GH_ParamAccess.item);
            pManager.AddSurfaceParameter("Surface", "Surface", "Surface to display frame on", GH_ParamAccess.item);
            pManager.AddGenericParameter("AnalysedData", "AnalysedData", "Data that was analysed to build a mesh with", GH_ParamAccess.item);
            pManager.AddBooleanParameter("DisplayMesh", "DisplayMesh", "Display analysed mesh", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "Mesh", "Mesh", GH_ParamAccess.list);
            //pManager.AddSurfaceParameter("Srf", "Srf", "Srf", GH_ParamAccess.item);
            pManager.AddBrepParameter("Brp", "Brp", "Brp", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string folder = "";
            bool analysedFrames = false;
            int frameNumber = 0;
            Grasshopper.Kernel.Types.GH_Surface srf = new Grasshopper.Kernel.Types.GH_Surface();
            VideoAnalyser va = new VideoAnalyser();
            bool showMesh = false;

            DA.GetData(0, ref folder);
            DA.GetData(1, ref analysedFrames);
            Convert.ToInt32(DA.GetData(2, ref frameNumber));
            DA.GetData(3, ref srf);
            DA.GetData(4, ref va);
            DA.GetData(5, ref showMesh);

            if (folder == null || frameNumber < 0) return; //Try again

            if (srf != null)
                _refID = srf.ReferenceID;

            if (_refID == null) return; //Can't display on an empty surface!

            if (!folder.EndsWith("\\"))
                folder += "\\";

            string fileName = "";

            if (analysedFrames)
                fileName = folder + "AN_" + frameNumber.ToString() + ".png";
            else
                fileName = folder + "NA_" + frameNumber.ToString() + ".png";

            if (!System.IO.File.Exists(fileName)) return; //No image for that frame number!

            //Apply texture to surface
            Rhino.DocObjects.BrepObject srf2 = (Rhino.DocObjects.BrepObject)Rhino.RhinoDoc.ActiveDoc.Objects.Where(x => x.Id == _refID).First();
            int index = Rhino.RhinoDoc.ActiveDoc.Materials.Add();
            Rhino.DocObjects.Material mat = Rhino.RhinoDoc.ActiveDoc.Materials[index];
            mat.SetBitmapTexture(fileName);
            mat.CommitChanges();

            Rhino.DocObjects.ObjectAttributes attr = new Rhino.DocObjects.ObjectAttributes();
            attr.MaterialIndex = index;
            attr.MaterialSource = Rhino.DocObjects.ObjectMaterialSource.MaterialFromObject;

            //srf2.Attributes.MaterialIndex = index;
            //srf2.CommitChanges();

            _refID = Rhino.RhinoDoc.ActiveDoc.Objects.AddBrep(srf2.BrepGeometry, attr);

            /*obj.Attributes.MaterialIndex = materialIndex;
    obj.CommitChanges(); 
*/

            //DA.SetData(1, Rhino.RhinoDoc.ActiveDoc.Objects.Last().Geometry);

            //Rhino.DocObjects.BrepObject srf7 = (Rhino.DocObjects.BrepObject)Rhino.RhinoDoc.ActiveDoc.Objects.Where(x => x.Id == _refID).First();
            //DA.SetData(1, srf7.BrepGeometry);

            //Mesh it up
            if (showMesh)
            {
                //Mesh[] m = Mesh.CreateFromBrep((Brep)srf2.Geometry);
                double width = 0;
                double height = 0;
                Rhino.Geometry.Brep srf3 = (Rhino.Geometry.Brep)srf2.Geometry;
                Rhino.Geometry.Collections.BrepSurfaceList srfl = srf3.Surfaces;
                Rhino.Geometry.Surface srf4 = srfl[0];
                srf4.GetSurfaceSize(out width, out height);
                double wScale = va.FrameWidth / width;
                double hScale = va.FrameHeight / height;
                List<Mesh> meshes = new List<Mesh>();

                List<ROI> rois = va.ROIs;
                Gradient g = new Gradient(this);

                double min = 1e10;
                double max = -1e10;
                foreach (ROI r in rois)
                {
                    foreach (KeyValuePair<int, double> kvp in r.Values)
                    {
                        min = Math.Min(min, kvp.Value);
                        max = Math.Max(max, kvp.Value);
                    }
                }

                g.MinGradient = min;
                g.MaxGradient = max;

                g.MaxGradient = 0.1;

                foreach (ROI r in rois)
                {
                    Polyline pl = new Polyline();   // TODO Clean up CrowdAnalysis mess
                    //pl.Add(new Point3d(r.StartPoint.X / wScale, r.StartPoint.Y / hScale, r.StartPoint.Z + 1));
                    //pl.Add(new Point3d(r.EndPoint.X / wScale, r.StartPoint.Y / hScale, r.StartPoint.Z + 1));
                    //pl.Add(new Point3d(r.EndPoint.X / wScale, r.EndPoint.Y / hScale, r.EndPoint.Z + 1));
                    //pl.Add(new Point3d(r.StartPoint.X / wScale, r.EndPoint.Y / hScale, r.StartPoint.Z + 1));
                    //pl.Add(new Point3d(r.StartPoint.X / wScale, r.StartPoint.Y / hScale, r.StartPoint.Z + 1));

                    Mesh m = Mesh.CreateFromClosedPolyline(pl);

                    if (m != null)
                    {
                        try
                        {
                            double colour = r.Values[frameNumber];
                            System.Drawing.Color col = g.SWFGradient.ColourAt((colour - g.MinGradient) / (g.MaxGradient + 0.0001 - g.MinGradient));
                            for (int x = 0; x < m.Vertices.Count; x++)
                                m.VertexColors.Add(col);
                            meshes.Add(m);
                        }
                        catch { }
                    }

                }
                DA.SetDataList(0, meshes);
            }



            Rhino.RhinoDoc.ActiveDoc.Objects.Delete(srf2.Id, true); //Delete original surface - this causes some errors/issues because then the surface doesn't exist in the GH window
        }
    }
}
