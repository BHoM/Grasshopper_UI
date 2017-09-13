//using BH.oM.Structural.Elements;
//using BH = BH.oM.Geometry;
//using R = Rhino.Geometry;
//using Grasshopper.Kernel;
//using BH.Engine.Grasshopper;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Collections;

//namespace BH.UI.Alligator.Geometry
//{
//    public class GetGeometry : GH_Component   /TODO: Requires corresonding method in engine 2.0
//    {
//        public GetGeometry() : base("Get Geometry", "GetGeometry", "Gets the Geometry of a BHoM Objects", "Structure", "Geometry")
//        {

//        }
//        protected override System.Drawing.Bitmap Internal_Icon_24x24
//        {
//            get { return Alligator.Properties.Resources.BHoM_Element_Geometry; }
//        }

//        public override Guid ComponentGuid
//        {
//            get
//            {
//                return new Guid("{99745777-C5C7-44AB-B75E-A61D7E0D0B05}");
//            }
//        }

//        protected override void RegisterInputParams(GH_InputParamManager pManager)
//        {
//            pManager.AddGenericParameter("Object", "Obj", "Object to get geometry from", GH_ParamAccess.item);
//        }

//        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
//        {
//            pManager.AddGeometryParameter("Geometry", "G", "Geometry", GH_ParamAccess.list);
//        }

//        protected override void SolveInstance(IGH_DataAccess DA)
//        {
//            BHoMObject obj = DataUtils.GetGenericData<BHoMObject>(DA, 0);
//            if (obj != null)
//            {
//                BH.GeometryBase geom = obj.GetGeometry();

//                if (typeof(IEnumerable).IsAssignableFrom(geom.GetType()))
//                {
//                    Type listType = geom.GetType().GetGenericArguments()[0];

//                    if (typeof(BH.oM.Geometry.GeometryBase).IsAssignableFrom(listType))
//                    {
//                        var utils = typeof(GeometryUtils);
//                        var methodInfo = utils.GetMethod("ConvertGroup", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
//                        var genericMethodInfo = methodInfo.MakeGenericMethod(new Type[] { listType });
//                        var list = genericMethodInfo.Invoke(null, new object[] { geom });
//                        DA.SetDataList(0, (IEnumerable)list);
//                    }
//                }
//                else
//                {
//                    DA.SetDataList(0, new List<R.GeometryBase>() { GeometryUtils.Convert(geom) } );
//                }
//            }
//        }
//    }
//}
