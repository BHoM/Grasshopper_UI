using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BH.oM.Base;
using BH.UI.Alligator;
using BH.UI.Alligator.Base;

namespace BH.UI.Alligator.Adapter
{
    public class FromJson : GH_Component
    {
        public FromJson() : base("From Json", "FromJson", "Try to convert a Json string to a BHoM object", "Alligator", "Adapter") { }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Properties.Resources.FromJson; }
        }

        public override Guid ComponentGuid { get { return new Guid("EB108FE0-A807-4CEA-A8EB-2B8D54ADBC04"); } }
        public override GH_Exposure Exposure { get { return GH_Exposure.tertiary; } }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Json", "Json", "Json string", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "BHoM object", "object", "BHoM object to convert", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string json = "";
            DA.GetData(0, ref json);

            DA.SetData(0, BH.Adapter.Convert.FromJson(json));
        }
    }
}
