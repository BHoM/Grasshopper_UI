using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using RHG = Rhino.Geometry;
using BHoMG = BHoM.Geometry;
using BHoM.Structural.Elements;
using BHoM.Structural.Interface;
using GHE = Grasshopper_Engine;

namespace GSA_Alligator.Elements
{
    public class ReadBars : GH_Component
    {
        public ReadBars() : base("Read Bars", "ReadBar", "Reads bars from GSA gile", "GSA", "Elements") { }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "App", "Application to import nodes from", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Execute", "R", "Generate Bars", GH_ParamAccess.item);

            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Bars", "Bars", "Bars", GH_ParamAccess.list);
            pManager.AddGeometryParameter("Lines", "Lines", "Lines", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Ids", "Ids", "Bar Numbers", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (GHE.DataUtils.Run(DA, 1))
            {
                IElementAdapter app = GHE.DataUtils.GetGenericData<IElementAdapter>(DA, 0);
                if (app != null)
                {
                    List<Bar> bars = new List<Bar>();
                    List<string> ids = new List<string>();
                    List<RHG.GeometryBase> lines = new List<RHG.GeometryBase>();
                    app.GetBars(out bars);

                    foreach (Bar bar in bars)
                    {
                       lines.Add(GHE.GeometryUtils.Convert(bar.Line as BHoMG.GeometryBase));
                       ids.Add(bar.Name);
                    }

                    DA.SetDataList(0, bars);
                    DA.SetDataList(1, lines);
                    DA.SetDataList(2, ids);
                }
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("682a4bc7-acd9-4812-9b30-54ef914b47da"); }
        }

        /// <summary> Icon(24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return GSA_Alligator.Properties.Resources.bar; }
        }

    }
}
