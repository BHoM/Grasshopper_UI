using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using GHE = Grasshopper_Engine;

namespace Alligator.Base
{
    public class AddCustomData : GH_Component
    {
        public AddCustomData() : base("AddCustomData", "AddData", "Add a custom data dictionary to a BHoM object", "Alligator", "Base")
        { }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_Create_CustomData; }
        }
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("E17D036C-423B-4E51-8F30-4CAAD552C378");
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
            pManager.AddGenericParameter("BHoM object", "object", "BHoM object to add data to", GH_ParamAccess.item);
            pManager.AddGenericParameter("Custom Data", "CustomData", "Custom data", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("BHoM object", "object", "BHoM object with added data", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHoM.Base.BHoMObject obj = GHE.DataUtils.GetGenericData<BHoM.Base.BHoMObject>(DA, 0);
            Dictionary<string,object> data = GHE.DataUtils.GetGenericData<Dictionary<string,object>>(DA, 1);

            var clone = obj.ShallowClone();
            clone.CustomData = new Dictionary<string, object>(obj.CustomData);

            foreach (var kvp in data)
            {
                clone.CustomData[kvp.Key] = kvp.Value;
            }

            DA.SetData(0, clone);
        }
    }
}