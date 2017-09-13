using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using GHE = BH.Engine.Grasshopper;

namespace BH.UI.Alligator.SQL
{
    //public class FromSQL : GH_Component
    //{
    //    //public FromSQL() : base("FromSQL", "FromSQL", "Get BHoM objects from a SQL database", "Alligator", "SQL") { }

        //public override Guid ComponentGuid
        //{
        //    get
        //    {
        //        return new Guid("932205D3-8E33-44F0-B95E-FF0B741CC04C");
        //    }
        //}

        ///// <summary> Icon (24x24 pixels)</summary>
        //protected override System.Drawing.Bitmap Internal_Icon_24x24
        //{
        //    get { return Alligator.Properties.Resources.BHoM_SQL_GetObject; }
        //}

        //protected override void RegisterInputParams(GH_InputParamManager pManager)
        //{
        //    pManager.AddGenericParameter("SQL link", "link", "collection to get the data from", GH_ParamAccess.item);
        //    pManager.AddTextParameter("query", "query", "query string", GH_ParamAccess.item);
        //    pManager.AddBooleanParameter("active", "active", "check if the component currently allows data transfer", GH_ParamAccess.item, false);
        //}

        //protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        //{
        //    pManager.AddTextParameter("names", "propertyNames", "property names from the query", GH_ParamAccess.list);
        //    pManager.AddTextParameter("values", "propertyValues", "property values from the query", GH_ParamAccess.tree);
        //}

        //protected override void SolveInstance(IGH_DataAccess DA)
        //{
        //    BHB.Data.SQLAccessor link = GHE.DataUtils.GetGenericData<BHB.SQLAccessor>(DA, 0);
        //    string query = GHE.DataUtils.GetData<string>(DA, 1);
        //    bool active = false; DA.GetData<bool>(2, ref active);

        //    if (!active) return;

        //    List<List<string>> result = link.Query(query);
        //    Grasshopper.DataTree<string> tree = new Grasshopper.DataTree<string>();
        //    for (int i = 1; i < result.Count; i++)
        //    {
        //        GH.Kernel.Data.GH_Path path = new GH.Kernel.Data.GH_Path(i);
        //        foreach (string val in result[i])
        //            tree.Add(val, path);  
        //    }

        //    DA.SetDataList(0, result[0]);
        //    DA.SetDataTree(1, tree);
        //}
    //}
}
