using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using BH.UI.Alligator.Base;
using BH.oM.Base;
using BH.UI.Alligator;

namespace BH.UI.Alligator.Adapter
{
    public class FilterQuery : GH_Component   
    {
        public FilterQuery() : base("FilterQuery", "FilterQuery", "Create a filter query", "Alligator", "Adapter") { }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }
        public override Guid ComponentGuid { get { return new Guid("B3B4DD0A-6B6C-4D4C-97C5-BDA39DFFD01A"); } }

        public override GH_Exposure Exposure { get { return GH_Exposure.secondary; } }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Type", "Type", "Type of object to filter (leave empty to get all types)", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Tag", "Tag", "Tag of object to filter (leave empty to get all tags)", GH_ParamAccess.item, "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Query", "Query", "FilterQuery", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            string typeString = "";  DA.GetData(0, ref typeString);
            string tag = ""; DA.GetData(1, ref tag);

            Type type = null;
            if (typeString != "")
                type = BH.Engine.Reflection.Create.Type(typeString);

            BH.Adapter.Queries.FilterQuery query = new BH.Adapter.Queries.FilterQuery(type, tag);
            DA.SetData(0, query);
        }
    }
}
