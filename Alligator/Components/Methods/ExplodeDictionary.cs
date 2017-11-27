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
using BH.oM.Geometry;
using BH.Adapter.Rhinoceros;
using Grasshopper.Kernel.Data;
using Grasshopper;

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
            pManager.AddGenericParameter("Values", "Values", "list of values from the dictionary", GH_ParamAccess.tree);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IDictionary dic = null;
            DA.GetData<IDictionary>(0, ref dic);

            DataTree<object> tree = new DataTree<object>();
            int index = 0;
            foreach (var value in dic.Values)
            {
                object converted = ConvertGeometry(value);
                if (converted is IEnumerable && !(converted is string) && !(converted is IDictionary))
                    tree.AddRange(((IEnumerable)converted).Cast<object>(), new GH_Path(index++));
                else
                tree.Add(ConvertGeometry(value), new GH_Path(index++));
            }
                

            DA.SetDataList(0, dic.Keys);
            DA.SetDataTree(1, tree);
        }

        private object ConvertGeometry(object container)
        {
            if (container is string || container is IDictionary)
                return container;
            if (container is IBHoMGeometry)
                return ((IBHoMGeometry)container).IToRhino();
            else if (container is IEnumerable)
                return ((IEnumerable)container).Cast<object>().Select(x => (x is IBHoMGeometry) ? ((IBHoMGeometry)x).IToRhino() : x).ToList();
            else
                return container;
        }
    }
}
