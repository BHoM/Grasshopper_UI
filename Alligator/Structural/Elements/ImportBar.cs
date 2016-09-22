using BHoM.Structural;
using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using GHE = Grasshopper_Engine;
using Alligator.Components;
using BHE = BHoM.Structural.Elements;
using BHI = BHoM.Structural.Interface;
using BHP = BHoM.Structural.Properties;
using BHG = BHoM.Geometry;
using System.Windows.Forms;
using R = Rhino.Geometry;
using Grasshopper;
using GHKT= Grasshopper.Kernel.Types;
using Grasshopper_Engine.Components;

namespace Alligator.Structural.Elements
{
    public class ImportBar : ImportComponent
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

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (GHE.DataUtils.Run(DA, 2))
            {
                BHI.IElementAdapter app = GHE.DataUtils.GetGenericData<BHI.IElementAdapter>(DA, 0);
                if (app != null)
                {
                    List<string> ids = null;
                    List<BHE.Bar> bars = null;
                    DataTree<R.Curve> curves = new DataTree<R.Curve>();
                    if (m_Selection == BHI.ObjectSelection.FromInput)
                        ids = GHE.DataUtils.GetDataList<string>(DA, 1);

                    app.Selection = m_Selection;
                    ids = app.GetBars(out bars, ids);

                    for (int i = 0; i < bars.Count;i++)
                    {
                        curves.Add(GHE.GeometryUtils.Convert(bars[i].Line));
                    }

                    DA.SetDataList(0, ids);
                    DA.SetDataList(1, bars);
                    DA.SetDataTree(2, curves);
                }
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("1320dc1b-87b3-491a-93fa-1495315aa5a2"); }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_Bar_Import; }
        }
    }

}
