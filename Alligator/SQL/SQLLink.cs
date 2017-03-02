using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BHB = BHoM.Base;
using GHE = Grasshopper_Engine;

namespace Alligator.SQL
{
    //public class SQLLink : GH_Component
    //{
    //    public SQLLink() : base("SQLLink", "SQLLink", "Create a link to a SQL database", "Alligator", "SQL") { }

    //    public override Guid ComponentGuid
    //    {
    //        get
    //        {
    //            return new Guid("2E3BAAB4-D22F-4B36-9C62-B6A68B319FA8");
    //        }
    //    }

    //    /// <summary> Icon (24x24 pixels)</summary>
    //    protected override System.Drawing.Bitmap Internal_Icon_24x24
    //    {
    //        get { return Alligator.Properties.Resources.BHoM_SQL_Link; }
    //    }

    //    protected override void RegisterInputParams(GH_InputParamManager pManager)
    //    {
    //        pManager.AddTextParameter("server", "server", "address of the server", GH_ParamAccess.item, "(localdb)\\ProjectsV13");
    //        pManager.AddTextParameter("database", "database", "name of the database", GH_ParamAccess.item);
    //        pManager.AddTextParameter("table", "table", "name of the table", GH_ParamAccess.item, "");
    //    }

    //    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    //    {
    //        pManager.AddGenericParameter("link", "link", "link to the database", GH_ParamAccess.item);
    //    }

    //    protected override void SolveInstance(IGH_DataAccess DA)
    //    {
    //        string server = GHE.DataUtils.GetData<string>(DA, 0);
    //        string database = GHE.DataUtils.GetData<string>(DA, 1);
    //        string table = GHE.DataUtils.GetData<string>(DA, 2);

    //        DA.SetData(0, new BHB.SQLAccessor(server, database, table));
    //    }
    //}
}
