using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using System.Linq;
using BH.UI.Alligator.Templates;
using System.Reflection;
using BH.Engine.Reflection;

namespace BH.UI.Alligator.Adapter
{
    public class CreateQuery : MethodCallTemplate
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Guid ComponentGuid { get; } = new Guid("E1BC4C14-9F5B-4879-B8EB-CCAC49178CFE"); 

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Base.Properties.Resources.QueryAdapter; 

        public override GH_Exposure Exposure { get; } = GH_Exposure.secondary;

        public override bool ShortenBranches { get; set; } = true;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public CreateQuery() : base("Create Query", "Query", "Creates a specific class of query", "Alligator", " Adapter") {}


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override IEnumerable<MethodBase> GetRelevantMethods()
        {
            Type queryType = typeof(BH.oM.DataManipulation.Queries.IQuery);
            return BH.Engine.Reflection.Query.BHoMMethodList().Where(x => queryType.IsAssignableFrom(x.ReturnType) && !x.IsNotImplemented() && !x.IsDeprecated()).OrderBy(x => x.Name);
        }

        /*******************************************/
    }
}