//using BH.oM.Structural.Design;
//using BH.oM.Structural.Elements;
//using Grasshopper.Kernel;
//using BH.Engine.Grasshopper;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BH.UI.Alligator.Structural.Elements.Design
//{
//    public class CreateFromConnectedBars : GH_Component  //TODO: Needs the coresponding method in the engine of BHoM 2.0
//    {
//        public CreateFromConnectedBars() : base("Create Design Elements", "CreateDE", "Create design elements from a list of connected bars", "Structure", "Design") { }

//        public override Guid ComponentGuid
//        {
//            get
//            {
//                return new Guid("AEA62A6A-51D6-4D24-AB4F-C8BB661D5C68");
//            }
//        }

//        protected override void RegisterInputParams(GH_InputParamManager pManager)
//        {
//            pManager.AddGenericParameter("Bars", "Bars", "List of bars to connect", GH_ParamAccess.list);
//            pManager.AddNumberParameter("Tolerance", "Tolerance", "Connect bars within the defined tolerance", GH_ParamAccess.item);
//            pManager[1].AddVolatileData(new GH.Kernel.Data.GH_Path(0), 0, 0.01);
//        }

//        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
//        {
//            pManager.AddGenericParameter("DesignElement", "DesElem", "The designelement with generated spans", GH_ParamAccess.list);
//        }

//        protected override void SolveInstance(IGH_DataAccess DA)
//        {
//            List<Bar> bars = DataUtils.GetGenericDataList<Bar>(DA, 0);
//            double tolerance = DataUtils.GetData<double>(DA, 1);
//            List<StructuralLayout> result = StructuralLayout.CreateFromConnectedBars(bars, tolerance);

//            DA.SetDataList(0, result);
//        }
//    }
//}
