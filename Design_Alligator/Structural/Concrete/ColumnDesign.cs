using BHoM.Base.Results;
using BHoM.Structural.Elements;
using BHoM.Structural.Interface;
using BHoM.Structural.Results;
using BHoM_Design.Concrete;
using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R = Rhino.Geometry;
using BH = BHoM.Geometry;
using Grasshopper_Engine;

namespace Design_Alligator.Structural.Concrete
{
    public class ColumnDesign : GH_Component
    {
        public ColumnDesign() : base("Concrete Column Design", "CColDesign", "Design a single column with the input forces", "Structure", "Design") { }
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("{29440BC6-B24D-4145-A838-82FBAA4734A0}");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Bars", "Bars", "Bars to design", GH_ParamAccess.item);
            pManager.AddNumberParameter("FX", "FX", "FX", GH_ParamAccess.item);
            pManager.AddNumberParameter("FY", "FY", "FY", GH_ParamAccess.item);
            pManager.AddNumberParameter("FZ", "FZ", "FZ", GH_ParamAccess.item);
            pManager.AddNumberParameter("MX", "MX", "MX", GH_ParamAccess.item);
            pManager.AddNumberParameter("MY", "MY", "MY", GH_ParamAccess.item);
            pManager.AddNumberParameter("MZ", "MZ", "MZ", GH_ParamAccess.item);

            Params.Input[1].Optional = true;
            Params.Input[1].AddVolatileDataList(new Grasshopper.Kernel.Data.GH_Path(0), null);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Moment vs Axial Curve", "MA1", "Moment vs Axial Curve about the major axis", GH_ParamAccess.item);
            pManager.AddCurveParameter("Moment vs Axial Curve", "MA2", "Moment vs Axial Curve about the minor axis", GH_ParamAccess.item);
            pManager.AddPointParameter("Moment + Axial", "P1", "Moment + Axial coordinate", GH_ParamAccess.item);
            pManager.AddPointParameter("Moment + Axial", "P2", "Moment + Axial coordinate", GH_ParamAccess.item);
            ConcreteColumnUtilisation cu = new ConcreteColumnUtilisation();
            for (int i = 4; i < cu.ColumnHeaders.Length; i++)
            { 
                pManager.AddNumberParameter(cu.ColumnHeaders[i], cu.ColumnHeaders[i], cu.ColumnHeaders[i], GH_ParamAccess.item);               
            }
            pManager.AddNumberParameter("CriticalRatio", "CriticalRatio", "Critical Ratio", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Bar bar = Grasshopper_Engine.DataUtils.GetGenericData<Bar>(DA, 0);
            BarForce force = new BarForce();
            force.FX = Grasshopper_Engine.DataUtils.GetData<double>(DA, 1);
            force.FY = Grasshopper_Engine.DataUtils.GetData<double>(DA, 2);
            force.FZ = Grasshopper_Engine.DataUtils.GetData<double>(DA, 3);
            force.MX = Grasshopper_Engine.DataUtils.GetData<double>(DA, 4);
            force.MY = Grasshopper_Engine.DataUtils.GetData<double>(DA, 5);
            force.MZ = Grasshopper_Engine.DataUtils.GetData<double>(DA, 6);
           
            BHoM_Design.Concrete.Eurocode1992_2004.ColumnDesign columnDesign = new BHoM_Design.Concrete.Eurocode1992_2004.ColumnDesign();
            string message = "";
            ConcreteColumnUtilisation cu = columnDesign.GetUtilisation(bar, force, 1.5, bar.EffectiveLength, ref message);
            Dictionary<string, object> results = new Dictionary<string, object>();
            R.Curve ma1 = GeometryUtils.Convert(columnDesign.GetMomentAxialPlot(bar, 1.5, 1, ref message));
            R.Curve ma2 = GeometryUtils.Convert(columnDesign.GetMomentAxialPlot(bar, 1.5, 0, ref message));
            R.Point3d p1 = new Rhino.Geometry.Point3d(Math.Abs(force.MY), force.FX, 0);
            R.Point3d p2 = new Rhino.Geometry.Point3d(Math.Abs(force.MZ), force.FX, 0);

            DA.SetData(0, ma1);
            DA.SetData(1, ma2);
            DA.SetData(2, p1);
            DA.SetData(3, p2);
                   
            double max = 0;
            for (int j = 4; j < cu.Data.Length; j++)
            {
                results.Add(cu.ColumnHeaders[j], cu.Data[j]);
                if (j > 5 && (double)cu.Data[j] > max)
                {
                    max = (double)cu.Data[j];
                }
            }

            results.Add("CriticalRatio", max);


            foreach (KeyValuePair<string, object> keyPair in results)
            {
                DA.SetData(keyPair.Key, keyPair.Value);
            }
        }
    }
}

