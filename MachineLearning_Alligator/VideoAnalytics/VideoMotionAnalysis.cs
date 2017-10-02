using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using BH.Engine.MachineLearning;
using BH.UI.Alligator.Query;

namespace BH.UI.Alligator.MachineLearning
{
    public class VideoMotionAnalysis : GH_Component
    {
        public VideoMotionAnalysis() : base("VideoMotionAnalysis", "VideoMotion", "Analyse the motion level in each frame of a video", "Alligator", "MachineLearning")
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
            videoFile = DA.BH_GetData(0, videoFile);
            active = DA.BH_GetData(7, active);

            if (!active) return;

            MotionLevelAnalyser analyser = new MotionLevelAnalyser();
            MotionLevelAnalyser.Config config = new MotionLevelAnalyser.Config();
            config.EndFrame = DA.BH_GetData(2, config.EndFrame);
            config.FrameStep = DA.BH_GetData(3, config.FrameStep);
            config.NbRows = DA.BH_GetData(4, config.NbRows);
            config.NbColumns = DA.BH_GetData(5, config.NbColumns);
            config.OutFolder = DA.BH_GetData(6, config.OutFolder);

            // TODO VideoMotionAnalyser.Result does not exist. Ask Fraser
            // Dictionary<int, List<double>> result = analyser.Run(videoFile, config).Result;
            // List<List<double>> motionLevel = result.Values.ToList();
            // DA.BH_SetDataList(0, motionLevel);
        }
    }
}
