using Grasshopper.Kernel;
using Grasshopper_Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alligator.Base
{
    public class ViewCustomData : GH_Component
    {
        public ViewCustomData() : base("ViewCustomData", "ViewData", "view a custom data dictionary on a BHoMobject", "Alligator", "Base")
        {

        }
        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_Read__CustomData; }
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("{BB22A083-3B0C-4E89-9D4E-FECCEDA95099}");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("BHoMObject", "Object", "View Custom data", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Data Name", "K", "Custom data name/key", GH_ParamAccess.list);
            pManager.AddGenericParameter("Data value", "V", "Custom data value", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHoM.Base.BHoMObject obj = DataUtils.GetGenericData<BHoM.Base.BHoMObject>(DA, 0);
            if (obj != null)
            {
                List<string> keys = new List<string>();
                List<object> data = new List<object>();
                foreach (KeyValuePair<string, object> keyVal in obj.CustomData)
                {
                    keys.Add(keyVal.Key);
                    data.Add(keyVal.Value);
                }
                DA.SetDataList(0, keys);
                DA.SetDataList(1, data);
            }
        }
    }
}
