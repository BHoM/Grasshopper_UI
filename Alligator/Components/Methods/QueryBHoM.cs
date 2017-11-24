using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Base;
using BH.UI.Alligator.Templates;
using System.IO;
using System.Reflection;
using BH.oM.DataStructure;
using System.Collections;

namespace BH.UI.Alligator.Base
{
    public class QueryBHoM : MethodCallTemplate
    {
        public QueryBHoM() : base("Query BHoM Object", "QueryBHoM", "Query information about a BHoMObject", "Alligator", "Base")
        {
        }
        public override Guid ComponentGuid { get { return new Guid("63DA0CAC-87BC-48AC-9C49-1D1B2F06BE83"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }


        /*************************************/

        protected override Tree<MethodBase> GetRelevantMethods()
        {
            Type enumerableType = typeof(IEnumerable);
            Tree<MethodBase> root = new Tree<MethodBase> { Name = "Select query" };

            foreach (MethodBase method in BH.Engine.Reflection.Query.GetBHoMMethodList().Where(x => x.DeclaringType.Name == "Query"))
            {
                AddMethodToTree(root, method);
            }
                

            return root;
        }
    }
}