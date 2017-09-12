//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Grasshopper.Kernel;
//using BH.oM.Structural.Elements;
//using BH.oM.Structural.ModelTracker;
//using BH.oM.Base.Results;
//using GHE = Grasshopper_Engine;

//namespace Alligator.Structural.ModelTracker
//{
//    public class StoreModelTrackingData : GH_Component
//    {
//        public StoreModelTrackingData() : base("Store Model Tracking Results", "StoreTracker", "Stores tracker objects on a result server", "Structure", "ModelTracking")
//        { }
//        public override Guid ComponentGuid
//        {
//            get
//            {
//                return new Guid("14F1C5C6-8C7B-4C76-BE1D-13247ED5144B");
//            }
//        }

//        protected override void RegisterInputParams(GH_InputParamManager pManager)
//        {
//            pManager.AddTextParameter("FileName", "FileName", "FileName", GH_ParamAccess.item);
//            pManager.AddGenericParameter("TrackerObjects", "TrackerObjects", "TrackerObjects", GH_ParamAccess.list);
//            pManager.AddBooleanParameter("Append", "Append", "Append", GH_ParamAccess.item, false);
//            pManager.AddBooleanParameter("Go", "Go", "Go", GH_ParamAccess.item);
//        }

//        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
//        {
//            pManager.AddBooleanParameter("Success", "Success", "Resturns true if storing the data went well", GH_ParamAccess.item);
//        }

//        protected override void SolveInstance(IGH_DataAccess DA)
//        {
//            if (GHE.DataUtils.Run(DA, 3))
//            {
//                string fileName = GHE.DataUtils.GetData<string>(DA, 0);
//                List<BarTracker> barTrackers = GHE.DataUtils.GetGenericDataList<BarTracker>(DA, 1);
//                bool append = GHE.DataUtils.GetData<bool>(DA, 2);
//                ResultServer<BarTracker> resServer = new ResultServer<BarTracker>(fileName, append);
//                resServer.StoreData(barTrackers);
//            }
            
//        }
//    }
//}
