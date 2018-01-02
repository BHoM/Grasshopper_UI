using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using System.Linq;
using BH.UI.Alligator.Templates;

namespace BH.UI.Alligator.Adapter
{
    public class CreateQuery : CreateObjectTemplate
    {
        public CreateQuery() : base("Create Query", "Query", "Creates a specific class of query", "Alligator", " Adapter")
        {
            m_MenuMaxDepth = 0;
        }
        public override Guid ComponentGuid { get { return new Guid("E1BC4C14-9F5B-4879-B8EB-CCAC49178CFE"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return Properties.Resources.QueryAdapter; } }
        public override GH_Exposure Exposure { get { return GH_Exposure.secondary; } }


        /*************************************/

        protected override IEnumerable<Type> GetRelevantTypes()
        {
            Type queryType = typeof(BH.oM.Queries.IQuery);
            return BH.Engine.Reflection.Query.BHoMTypeList().Where(x => queryType.IsAssignableFrom(x)).OrderBy(x => x.Name);
        }

    }
}