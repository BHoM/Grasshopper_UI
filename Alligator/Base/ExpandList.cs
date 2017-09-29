using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;

namespace BH.UI.Alligator.Base
{
    public class ExpandList : GH_Component
    {
        public ExpandList() : base("ExpandList", "ExList", "Gives a full list of objects from a list item", "Alligator", "Utilities") { }
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("6BAC2DB1-BCB0-4876-BDFD-33A40A826B16");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("List item", "List", "The list item to expand", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("List", "List", "The expanded list", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IList list = default(IList);
            DA.GetData(0, ref list);
            DA.SetDataList(0, list);
        }
    }
}
