using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BH.oM.Base;
using BH.UI.Alligator;

namespace BH.UI.Alligator.Base
{
    public class FromJson : GH_Component
    {
        public FromJson() : base("FromJson", "FromJson", "Convert the object to a Json string", "Alligator", "Base") { }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return null; }
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("cc3228e4-6bba-4d83-805c-76bf806daa99");
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
            pManager.AddTextParameter("Json", "Json", "Json string", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "BHoM object", "object", "BHoM object to convert", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string obj = "";
            DA.GetData(0, ref obj);

            DA.SetData(0, BH.Adapter.Convert.FromJson(obj));
        }
    }
}
