using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using GHE = Grasshopper_Engine;
using BHB = BHoM.Base;
using D3T = D3_Toolkit;

namespace Alligator.D3
{
    public class ConstructAxis : GH_Component
    {
        public ConstructAxis() : base("ConstructAxis", "ConstructAxis", "Create D3 axis", "Alligator", "D3")
        {
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("017E9679-6CA4-429F-8116-FA7975F4BF53");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("title", "title", "axis title", GH_ParamAccess.item);
            pManager.AddNumberParameter("minVal", " min value", "minimum value", GH_ParamAccess.item);
            pManager.AddNumberParameter("maxVal", " max value", "maximum value", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("axis", "axis", "axis config", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            D3_Toolkit.D3Axis axis = new D3_Toolkit.D3Axis();

            axis.Title = GHE.DataUtils.GetData<string>(DA, 0);

            double minVal = 0;
            if (DA.GetData<double>(1,ref minVal))
                axis.MinVal = minVal;

            double maxVal = 0;
            if (DA.GetData<double>(2, ref maxVal))
                axis.MaxVal = maxVal;

            DA.SetData(0, axis.ToJson());
        }
    }
}
