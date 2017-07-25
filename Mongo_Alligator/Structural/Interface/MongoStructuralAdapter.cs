//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Grasshopper.Kernel;
//using GHE = Grasshopper_Engine;
//using MA = Mongo_Adapter;

//namespace Alligator.Mongo.Structural.Interface
//{
//    public class MongoStructuralAdapter : GH_Component
//    {
//        public MongoStructuralAdapter() : base("Mongo Structural Adapter", "MongoApp", "Create a mongo structural adapter", "Structure", "Application") { }

//        public override Guid ComponentGuid
//        {
//            get
//            {
//                return new Guid("651FD28C-CD07-449A-A5D2-1B1F8BD70962");
//            }
//        }

//        public override GH_Exposure Exposure
//        {
//            get
//            {
//                return GH_Exposure.secondary;
//            }
//        }

//        protected override void RegisterInputParams(GH_InputParamManager pManager)
//        {
//            pManager.AddGenericParameter("Mongo Data Base", "DB", "Mongo database to use as result server", GH_ParamAccess.item);
//        }

//        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
//        {
//            pManager.AddGenericParameter("Result server", "MRS", "Mongo structural result server", GH_ParamAccess.item);
//        }

//        protected override void SolveInstance(IGH_DataAccess DA)
//        {
//            MA.MongoDataBase database = GHE.DataUtils.GetData<MA.MongoDataBase>(DA, 0);

//            DA.SetData(0, new MA.Structural.Interface.MongoStructuralAdapter(database));
//        }
//    }
//}