using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHB = BHoM.Base;
using GHE = Grasshopper_Engine;

namespace Alligator.Base
{
    public class ToJSON : GH_Component
    {
        public ToJSON() : base("ToJSON", "ToJSON", "Convert a BHoM object to a JSON string", "Alligator", "Base") { }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_ToJSON; }
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("C36278A4-DD93-4645-8F69-2DB73167DD32");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("BHoM object", "object", "BHoM object to convert", GH_ParamAccess.list);
            pManager.AddTextParameter("password", "password", "password to encrypt data", GH_ParamAccess.item, "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("json", "json", "json representation of the BHoM object", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<BHB.BHoMObject> objects = GHE.DataUtils.GetGenericDataList<BHB.BHoMObject>(DA, 0);
            string password = GHE.DataUtils.GetData<string>(DA, 1);

            DA.SetData(0, BHB.BHoMJSON.WritePackage(objects.Cast<BHB.BHoMObject>().ToList(), password));
        }
    }
}
