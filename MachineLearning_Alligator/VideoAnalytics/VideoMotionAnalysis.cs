using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using BH.Engine.MachineLearning;
using BH.UI.Alligator;

namespace BH.UI.Alligator.MachineLearning
{
    public class VideoMotionAnalysis : GH_Component
    {
        public VideoMotionAnalysis() : base("VideoMotionAnalysis", "VideoMotion",
            "Analyse the motion level in each frame of a video",
            "Alligator", "MachineLearning")
        {
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("064A9B3C-E4E5-4BD3-AF12-FF5036B82B31");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("videoFile", "videoFile", "Full path of the video file", GH_ParamAccess.item);
            pManager.AddIntegerParameter("startFrame", "startFrame", "First frame to be processed", GH_ParamAccess.item, 0);
            pManager.AddIntegerParameter("endFrame", "endFrame", "Last frame to be processed", GH_ParamAccess.item, int.MaxValue);
            pManager.AddIntegerParameter("frameStep", "frameStep", "step between frames created in the output folder", GH_ParamAccess.item, 1);
            pManager.AddIntegerParameter("nbRows", "nbRows", "nb of rows of separate analysis", GH_ParamAccess.item, 1);
            pManager.AddIntegerParameter("nbColumns", "nbColumns", "nb of columns of separate analysis", GH_ParamAccess.item, 1);
            pManager.AddTextParameter("outFolder", "outFolder", "Full path of the output folder", GH_ParamAccess.item, "");
            pManager.AddBooleanParameter("active", "active", "activate the calculations", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("motion", "motionLevel", "Level of motion (from 0 to 1.0) for each frame", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string videoFile = "";
            bool active = false;
            DA.GetData(0, ref videoFile);
            DA.GetData(7, ref active);

            if (!active) return;

            MotionLevelAnalyser analyser = new MotionLevelAnalyser();
            MotionLevelAnalyser.Config config = new MotionLevelAnalyser.Config();
            DA.GetData(2, ref config.EndFrame);
            DA.GetData(3, ref config.FrameStep);
            DA.GetData(4, ref config.NbRows);
            DA.GetData(5, ref config.NbColumns);
            DA.GetData(6, ref config.OutFolder);

            Dictionary<int, List<double>> result = analyser.Run(videoFile, config);
            List<List<double>> motionLevel = result.Values.ToList();
            DA.SetDataList(0, motionLevel);
        }
    }
}
