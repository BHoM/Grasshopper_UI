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
    public class CustomQuery : GH_Component   
    {
        public CustomQuery() : base("CustomQuery", "CustomQuery", "Create a custom query", "Alligator", "Adapter") { }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }
        public override Guid ComponentGuid { get { return new Guid("8E22B39D-9ABC-4DCD-8168-3109B10689E4"); } }

        public override GH_Exposure Exposure { get { return GH_Exposure.secondary; } }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Query", "Query", "Manual Query string", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Query", "Query", "FilterQuery", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            string queryString = ""; DA.BH_GetData<string>(0, ref queryString);;

            BH.Adapter.Queries.CustomQuery query = new BH.Adapter.Queries.CustomQuery(queryString);
            DA.BH_SetData(0, query);
        }
    }
}
