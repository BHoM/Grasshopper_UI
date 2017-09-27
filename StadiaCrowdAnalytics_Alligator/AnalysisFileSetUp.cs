using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grasshopper.Kernel;
using GHE = Grasshopper_Engine;

using StadiaCrowdAnalysis_Engine;

using BHoM.Geometry;

using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StadiaCrowdAnalytics_Alligator
{
    public class AnalysisFileSetUp : GH_Component
    {
        public AnalysisFileSetUp()
            : base("StadiaAnalysisFrameDisplay", "StadiaAnalysisFrameDisplay", "Set up a video frame in Rhino to begin drawing a grid on", "Alligator", "StadiaCrowdAnalytics")
        {
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("B20E9BD5-9E9F-4217-9F99-4884B7463122"); }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("VideoFile", "VideoFile", "Full path to the video file", GH_ParamAccess.item);
            pManager.AddSurfaceParameter("SurfaceMap", "SurfaceMap", "Surface to map the frame on to build your grid", GH_ParamAccess.item);
            //pManager.AddLineParameter("Grid Rows", "Grid Rows", "The lines that make up the rows of the grid", GH_ParamAccess.list);
            //pManager.AddLineParameter("Grid Columns", "Grid Columns", "The lines that make up the columns of the grid", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Display frame", "Display frame", "Choose to display the frame on Rhino or not", GH_ParamAccess.item, true);

            pManager[1].Optional = true;
            pManager[2].Optional = true;
            //pManager[3].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            //pManager.AddGenericParameter("Grid", "Grid", "Video file with associated grid", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            String videoFile = GHE.DataUtils.GetData<String>(DA, 0);
            Grasshopper.Kernel.Types.GH_Surface srf = GHE.DataUtils.GetData<Grasshopper.Kernel.Types.GH_Surface>(DA, 1);
            //List<Grasshopper.Kernel.Types.GH_Line> rows = GHE.DataUtils.GetDataList<Grasshopper.Kernel.Types.GH_Line>(DA, 1);
            //List<Grasshopper.Kernel.Types.GH_Line> cols = GHE.DataUtils.GetDataList<Grasshopper.Kernel.Types.GH_Line>(DA, 2);
            
            if (videoFile == null || srf == null) return;

            VideoAnalyser analyser = new VideoAnalyser();
            analyser.FileName = videoFile;
            //bool result = analyser.LoadFile().Result;
            analyser.LoadFile();
            //System.Drawing.Bitmap frame = analyser.LoadFile().Result;

            //if (!result) return;

            bool display = GHE.DataUtils.GetData<bool>(DA, 2);
            if (!display) return;

            //Display the frame on the Rhino window as a wallpaper
            System.Drawing.Bitmap frame = analyser.CurrentFrame;

            String fileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + @"\myImage.jpg";
            frame.Save(fileName);

            Rhino.DocObjects.BrepObject srf2 = (Rhino.DocObjects.BrepObject)Rhino.RhinoDoc.ActiveDoc.Objects.Where(x => x.Id == srf.ReferenceID).First();
            Console.WriteLine(srf2.ToString());

            //Apply texture to surface
            int index = Rhino.RhinoDoc.ActiveDoc.Materials.Add();
            Rhino.DocObjects.Material mat = Rhino.RhinoDoc.ActiveDoc.Materials[index];
            mat.SetBitmapTexture(fileName);
            mat.CommitChanges();

            Rhino.DocObjects.ObjectAttributes attr = new Rhino.DocObjects.ObjectAttributes();
            attr.MaterialIndex = index;
            attr.MaterialSource = Rhino.DocObjects.ObjectMaterialSource.MaterialFromObject;
            
            Rhino.RhinoDoc.ActiveDoc.Objects.AddBrep(srf2.BrepGeometry, attr);

            Rhino.RhinoDoc.ActiveDoc.Objects.Delete(srf.ReferenceID, true); //Delete original surface
            //Rhino.RhinoDoc.ActiveDoc.Objects.AddSurface(srf2, attr);
            
            
            //Rhino.DocObjects.Texture txt = new Rhino.DocObjects.Texture();
            //txt.FileName = fileName;
            

            //Get viewport
            //Rhino.Display.RhinoViewport rv = Rhino.Display.RhinoView.
            //Rhino.Display.RhinoViewport rv = Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport;
            //rv.SetWallpaper(fileName, false);

            //var srf = new Rhino.Geometry.Surface();
            //var srf = new Rhino.Geometry.Surface();
            //Rhino.Display.DisplayBitmap.Load(fileName);
            //rv.SetWallpaper
        }

        public override void RemovedFromDocument(GH_Document document)
        {
            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + @"\myImage.jpg");
            base.RemovedFromDocument(document);
        }
    }
}
