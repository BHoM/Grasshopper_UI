using System;
using Grasshopper.Kernel;
using System.Linq;
using BH.UI.Grasshopper.Templates;
using System.Reflection;
using BH.oM.DataStructure;
using System.Collections;

namespace BH.UI.Grasshopper.Base
{
    public class QueryBHoM : MethodCallTemplate
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Guid ComponentGuid { get; } = new Guid("63DA0CAC-87BC-48AC-9C49-1D1B2F06BE83"); 

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Properties.Resources.Query; 

        public override GH_Exposure Exposure { get; } = GH_Exposure.secondary;

        public override string MethodGroup { get; set; } = "Query";

        public override bool Obsolete { get; } = true;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public QueryBHoM() : base("Query BHoM Object", "QueryBHoM", "Query information about a BHoMObject", "Grasshopper", " Engine") {}


        /*******************************************/
    }
}