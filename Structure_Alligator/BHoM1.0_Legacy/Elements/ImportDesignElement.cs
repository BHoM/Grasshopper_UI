//using BH.oM.Structural;
//using System;
//using Grasshopper.Kernel;
//using System.Collections.Generic;
//using GHE = BH.Engine.Grasshopper;
//using BH.UI.Alligator.Components;
//using BHE = BH.oM.Structural.Elements;
//using BHI = BH.oM.Structural.Interface;
//using BHP = BH.oM.Structural.Properties;
//using BHG = BH.oM.Geometry;
//using System.Windows.Forms;
//using R = Rhino.Geometry;
//using Grasshopper;
//using GHKT = Grasshopper.Kernel.Types;
//using BH.Engine.Grasshopper.Components;
//using Grasshopper.Kernel.Data;
//using BH.Engine.Structure;

//namespace BH.UI.Alligator.Structural.Elements
//{
//    public class ImportDesignElement : ImportComponent<BHE.Bar>
//    {
//        public ImportDesignElement() : base("Import Design Element", "GetDesElem", "Get the geometry and properties of a design element as a BHoM Bar", "Structure", "Element Design")
//        {

//        }

//        protected override void SolveInstance(IGH_DataAccess DA)
//        {
//            if (GHE.DataUtils.Run(DA, 2))
//            {
//                BHI.IDesignMemberAdapter app = GHE.DataUtils.GetGenericData<BHI.IDesignMemberAdapter>(DA, 0);
//                if (app != null)
//                {
//                    List<string> ids = null;
//                    List<BHE.Bar> bars = null;
//                    DataTree<R.Line> curves = new DataTree<R.Line>();

//                    ids = app.GetBarDesignElement(out bars, ids);

//                    for (int i = 0; i < bars.Count; i++)
//                    {
//                        curves.Add(GHE.GeometryUtils.Convert(bars[i].GetCentreline()));
//                    }

//                    DA.SetDataList(0, ids);
//                    DA.SetDataList(1, bars);
//                    DA.SetDataTree(2, curves);
//                }
//            }
//        }

//        public override Guid ComponentGuid
//        {
//            get { return new Guid("5639413B-C6A7-47D6-865B-DF76C409E5A6"); }
//        }

//        /// <summary> Icon (24x24 pixels)</summary>
//        protected override System.Drawing.Bitmap Internal_Icon_24x24
//        {
//            get { return Alligator.Structural.Properties.Resources.BHoM_DesignBar_Import; }
//        }
//    }

//}