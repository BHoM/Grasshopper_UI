using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BH.oM.Base;
using BH.Engine.Reflection;
using BH.UI.Alligator;

namespace BH.UI.Alligator.Base
{
    public class GetPropertyNames : GH_Component
    {
        public GetPropertyNames() : base("GetPropertyNames", "GetPropertyNames", "Get names of all properties of a BHoM object", "Alligator", "Base") { }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Properties.Resources.BHoM_GetPropertyNames; }
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("B45841BD-21A8-4129-98B6-E2FF6B3AD145");
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
            pManager.AddParameter(new BHoMObjectParameter(), "BHoM object", "object", "BHoM object to convert", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Property name", "name", "Property names for the BHoM object", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHoMObject obj = new BHoMObject();
            obj = DA.BH_GetData(0, obj);

            DA.SetDataList(0, BH.Engine.Reflection.Query.GetPropertyNames(obj));
        }
    }
}
