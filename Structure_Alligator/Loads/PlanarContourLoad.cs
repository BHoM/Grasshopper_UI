//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Grasshopper.Kernel;
//using Grasshopper.Kernel.Types;
//using BHL = BH.oM.Structural.Loads;
//using BHE = BH.oM.Structural.Elements;
//using Rhino.Geometry;

//namespace Alligator.Structural.Loads
//{
//    public class PlanarContourLoad : GH_Component
//    {
//        // TODO Check PlanarContourLoad
//        public PlanarContourLoad(): base("Create Planar Uniform Contour Load", "Create Planar Contour Load", "Create a contour load", "Structure", "Loads") { }

//        public override Guid ComponentGuid
//        {
//            get
//            {
//                return new Guid("280965BA-0C7E-4802-9196-97A12147F76A");
//            }
//        }
//        protected override void RegisterInputParams(GH_InputParamManager pManager)
//        {
//            pManager.AddVectorParameter("Pressure", "Pressure", "Pressure", GH_ParamAccess.item);
//            pManager.AddBooleanParameter("Projected", "Projected", "Projected", GH_ParamAccess.item);
//            pManager.AddGenericParameter("Loadcase", "Loadcase", "Loadcase", GH_ParamAccess.item);
//            pManager.AddCurveParameter("Contour", "Contour", "Contour", GH_ParamAccess.list);
//            pManager.AddTextParameter("Name", "Name", "Name", GH_ParamAccess.item);
//            pManager.AddGenericParameter("CustomData", "CustomData", "CustomData", GH_ParamAccess.item);

//            pManager[1].Optional = true;
//            pManager[3].Optional = true;
//            pManager[5].Optional = true;

//            AppendEnumOptions("Axis", typeof(BHL.LoadAxis));
//        }
//        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
//        {
//            pManager.AddGenericParameter("PlanarContourLoad", "PlanarContourLoad", "UniformPlanarContourLoad", GH_ParamAccess.item);
//        }
//        protected override void SolveInstance(IGH_DataAccess DA)
//        {
//            Vector3d pressure = GHE.DataUtils.GetData<Vector3d>(DA, 0);
//            bool projected = GHE.DataUtils.GetData<bool>(DA, 1);
//            BHL.Loadcase loadcase = GHE.DataUtils.GetData<BHL.Loadcase>(DA, 2);
//            List<object> contour = GHE.DataUtils.GetDataList<object>(DA, 3);
//            string name = GHE.DataUtils.GetData<string>(DA, 4);

//            BHL.LoadAxis axis = new BHoM.Structural.Loads.LoadAxis();

//            switch ((BHL.LoadAxis)m_SelectedOption[0])
//            {
//                case BHL.LoadAxis.Local:
//                    axis = BHL.LoadAxis.Local;
//                    break;
//                case BHL.LoadAxis.Global:
//                    axis = BHL.LoadAxis.Global;
//                    break;
//                default:
//                    axis = BHL.LoadAxis.Global;

//                    break;
//            }


//            BHoM.Geometry.Vector prVec = new BHoM.Geometry.Vector(pressure.X, pressure.Y, pressure.Z);
//            List<BHL.GeometricalAreaLoad> loadList = new List<BHL.GeometricalAreaLoad>();

//            for (int i = 0; i < contour.Count; i++)
//            {
//                object result = null;

//                if (contour[i] is GH_Curve)
//                {
//                    result = GHE.GeometryUtils.Convert(((GH_Curve)contour[i]).Value);
//                }
                            
//                BHL.GeometricalAreaLoad planarLoad = new BHL.GeometricalAreaLoad((BHoM.Geometry.Curve)result, prVec);

//                planarLoad.Projected = projected;
//                planarLoad.Loadcase = loadcase;
//                planarLoad.Axis = axis;
//                planarLoad.Name = name;
                
//                loadList.Add(planarLoad);

//            }

            
//            //BHL.LoadAxis axis = new BHoM.Structural.Loads.LoadAxis();


//            DA.SetData(0, loadList);
//        }
//    }
//}
