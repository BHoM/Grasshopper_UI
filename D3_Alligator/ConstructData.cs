using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using GHE = Grasshopper_Engine;
using BHB = BH.oM.Base;
using D3T = D3_Toolkit;

namespace Alligator.D3
{
    public class ConstructData : GH_Component
    {
        public ConstructData() : base("ConstructData", "ConstructData", "Create D3 data from a list of keys and a list of values", "Alligator", "D3")
        {
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("96088249-8070-4934-A3ED-59BC1592E4D9");
            }
        }
        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return D3_Alligator.Properties.Resources.BH.oM_D3_Construct_Data; }
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("key", "key", "list of property names", GH_ParamAccess.list);
            pManager.AddNumberParameter("values", "values", "list of values for the properties", GH_ParamAccess.list);

            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("object", "object", "constructed object", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<string> keys = GHE.DataUtils.GetDataList<string>(DA, 0);
            List<double> values = GHE.DataUtils.GetDataList<double>(DA, 1);

            DA.SetData(0, D3T.Json.ToJsonObject(keys, values.Select(x => x.ToString()).ToList()));
        }
    }
}
