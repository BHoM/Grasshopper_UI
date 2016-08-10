using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using GHE = Grasshopper_Engine;

namespace Alligator.Components
{
    public class CustomData : GH_Component
    {
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("{AD22A083-3B0C-4E89-9D4E-FECCEDA95099}");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Data Name", "K", "Custom data name/key", GH_ParamAccess.list);
            pManager.AddGenericParameter("Data value", "V", "Custom data value", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Custom Data", "CD", "Custom data", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<string> keys = GHE.DataUtils.GetDataList<string>(DA, 0);
            List<object> data = GHE.DataUtils.GetGenericDataList<object>(DA, 1);

            if (keys.Count != data.Count)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                for (int i = 0; i < keys.Count; i++)
                {
                    dictionary.Add(keys[i], data[i]);
                }
                DA.SetData(0, dictionary);
            }
            else
            {
                throw (new Exception("Data Name and data value list must be of the same length"));
            }
        }
    }
}
