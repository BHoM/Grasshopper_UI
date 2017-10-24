using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BH.UI.Alligator;
using BH.Engine.Reflection;

namespace BH.UI.Alligator.Base
{
    public class ViewCustomData : GH_Component
    {
        public ViewCustomData() : base("ViewCustomData", "ViewData", "view a custom data dictionary on a BHoMobject", "Alligator", "Base")
        {

        }
        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Properties.Resources.BHoM_Read__CustomData; }
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("{BB22A083-3B0C-4E89-9D4E-FECCEDA95099}");
            }
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
            pManager.AddGenericParameter("BHoMObject", "Object", "View Custom data", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Data Name", "K", "Custom data name/key", GH_ParamAccess.list);
            pManager.AddGenericParameter("Data value", "V", "Custom data value", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHoMObject obj = new BHoMObject();
            DA.GetData(0, ref obj);

            Dictionary<string, object> dict = obj.GetPropertyDictionary();

            DA.SetDataList(0, dict.Keys);
            DA.SetDataList(1, dict.Values);
        }
    }
}
