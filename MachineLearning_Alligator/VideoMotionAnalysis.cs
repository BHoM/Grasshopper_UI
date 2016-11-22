using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using GHE = Grasshopper_Engine;
using MachineLearning_Engine;

namespace MachineLearning_Alligator
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
            string videoFile = GHE.DataUtils.GetData<string>(DA, 0);
            bool active = GHE.DataUtils.GetData<bool>(DA, 7);

            if (!active) return;

            MotionLevelAnalyser analyser = new MotionLevelAnalyser();
            MachineLearning_Engine.MotionLevelAnalyser.Config config = new MachineLearning_Engine.MotionLevelAnalyser.Config();
            config.StartFrame = GHE.DataUtils.GetData<int>(DA, 1);
            config.EndFrame = GHE.DataUtils.GetData<int>(DA, 2);
            config.FrameStep = GHE.DataUtils.GetData<int>(DA, 3);
            config.NbRows = GHE.DataUtils.GetData<int>(DA, 4);
            config.NbColumns = GHE.DataUtils.GetData<int>(DA, 5);
            config.OutFolder = GHE.DataUtils.GetData<string>(DA, 6);

            Dictionary<int, List<double>> result = analyser.Run(videoFile, config).Result;
            List<List<double>> motionLevel = result.Values.ToList();
            DA.SetDataList(0, motionLevel);
        }
    }
}
