using BH.oM.Structural;
using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using GHE = BH.Engine.Grasshopper;
using BH.UI.Alligator.Components;
using BHE = BH.oM.Structural.Elements;
using BHI = BH.oM.Structural.Interface;
using BHP = BH.oM.Structural.Properties;
using BHG = BH.oM.Geometry;
using System.Windows.Forms;
using R = Rhino.Geometry;
using Grasshopper;
using GHKT = Grasshopper.Kernel.Types;
using BH.Engine.Grasshopper.Components;
using Grasshopper.Kernel.Data;
using BH.Engine.Structure;

namespace BH.UI.Alligator.Structural.Elements
{
    public class ImportBar : ImportComponent<BHE.Bar>
    {       
        public ImportBar() : base("Import Bar", "GetBar", "Get the geometry and properties of a Bar", "Structure", "Elements")
        {
           
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.tertiary;
            }
        }

        //protected override void SolveInstance(IGH_DataAccess DA)
        //{
        //    if (GHE.DataUtils.Run(DA, 2))
        //    {
        //        BHI.IElementAdapter app = GHE.DataUtils.GetGenericData<BHI.IElementAdapter>(DA, 0);
        //        if (app != null)
        //        {
        //            List<string> ids = null;
        //            List<BHE.Bar> bars = null;
        //            DataTree<R.Curve> curves = new DataTree<R.Curve>();
        //            if (m_Selection == BHI.ObjectSelection.FromInput)
        //                ids = GHE.DataUtils.GetDataList<string>(DA, 1);

        //            app.Selection = m_Selection;
        //            ids = app.GetBars(out bars, ids);

        //            for (int i = 0; i < bars.Count;i++)
        //            {
        //                curves.Add(GHE.GeometryUtils.Convert(bars[i].Line));
        //            }

        //            DA.SetDataList(0, ids);
        //            DA.SetDataList(1, bars);
        //            DA.SetDataTree(2, curves);
        //        }
        //    }
        //}

        public override List<BHE.Bar> GetObjects(BHI.IElementAdapter app, List<string> objectIds, out IGH_DataTree geom, out List<string> outIds)
        {
            List<BHE.Bar> result = null;
            outIds = app.GetBars(out result, objectIds);

            DataTree<R.Line> curves = new DataTree<R.Line>();
            for (int i = 0; i < result.Count; i++)
            {
                curves.Add(GHE.GeometryUtils.Convert(result[i].GetCentreline()));
            }
            geom = curves;
            return result;
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("1320dc1b-87b3-491a-93fa-1495315aa5a2"); }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Structural.Properties.Resources.BHoM_Bar_Import; }
        }
    }

}
