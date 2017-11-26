using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BH.oM.Base;
using BH.UI.Alligator;
using System.Collections;
using Grasshopper.Kernel.Types;
using System.Runtime.CompilerServices;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Parameters.Hints;

namespace BH.UI.Alligator.Base
{
    public class ExplodeDictionary : GH_Component
    {
        public ExplodeDictionary() : base("ExplodeDictionary", "ExplodeDic", "Explode a dictionary into a list of keys and values", "Alligator", "Base") { }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }

        public override Guid ComponentGuid { get { return new Guid("BEA9866B-0326-487D-9F0D-626AE661C721"); } }
        public override GH_Exposure Exposure { get { return GH_Exposure.tertiary; } }


        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new DictionaryParameter(), "Dic", "Dictionary", "Dictionary", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Keys", "Keys", "list of keys from the dictionary", GH_ParamAccess.list);
            pManager.AddGenericParameter("Values", "Values", "list of values from the dictionary", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IDictionary dic = null;
            DA.GetData<IDictionary>(0, ref dic);

            DA.SetDataList(0, dic.Keys);
            DA.SetDataList(1, dic.Values);
        }
    }
}
