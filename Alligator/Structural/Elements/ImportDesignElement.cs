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
using GHKT = Grasshopper.Kernel.Types;
using Grasshopper_Engine.Components;
using Grasshopper.Kernel.Data;

namespace Alligator.Structural.Elements
{
    public class ImportDesignElement : ImportComponent<BHE.Bar>
    {
        public ImportDesignElement() : base("Import Design Element", "GetDesElem", "Get the geometry and properties of a design element as a BHoM Bar", "Structure", "Element Design")
        {

        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (GHE.DataUtils.Run(DA, 2))
            {
                BHI.IDesignMemberAdapter app = GHE.DataUtils.GetGenericData<BHI.IDesignMemberAdapter>(DA, 0);
                if (app != null)
                {
                    List<string> ids = null;
                    List<BHE.Bar> bars = null;
                    DataTree<R.Line> curves = new DataTree<R.Line>();

                    ids = app.GetBarDesignElement(out bars, ids);

                    for (int i = 0; i < bars.Count; i++)
                    {
                        curves.Add(GHE.GeometryUtils.Convert(bars[i].Line));
                    }

                    DA.SetDataList(0, ids);
                    DA.SetDataList(1, bars);
                    DA.SetDataTree(2, curves);
                }
            }
        }

        public override List<BHE.Bar> GetObjects(BHI.IElementAdapter app, List<string> objectIds, out IGH_DataTree geom, out List<string> outIds)
        {
            throw new Exception();
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("5639413B-C6A7-47D6-865B-DF76C409E5A6"); }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_Bar_Import; }
        }
    }

}