using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BH.Engine.Grasshopper;

namespace BH.UI.Alligator.Base
{
    public class CollapseList : GH_Component
    {
        public CollapseList() : base("CollapseList", "CoList", "Gives a list item from a list of objects", "Alligator", "Utilities") { }
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("DB945E7E-5E11-40EC-8527-833D338ABC99");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("List item", "List", "The list  to collape", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("List", "List", "The collapsed list", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IList list = default(IList);
            DA.GetData(0, ref list);
            DA.SetData(0, list);
        }
    }
}
