using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Base;
using BH.UI.Alligator.Templates;
using System.IO;
using System.Reflection;
using BH.oM.DataStructure;

namespace BH.UI.Alligator.Base
{
    public class Query : MethodCallTemplate
    {
        public Query() : base("Query BHoM Object", "Query", "Query information about a BHoMObject", "Alligator", "Base")
        {
        }
        public override Guid ComponentGuid { get { return new Guid("63DA0CAC-87BC-48AC-9C49-1D1B2F06BE83"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }

        /*************************************/

        protected override void SetData(IGH_DataAccess DA, object result)
        {
            DA.SetData(0, result as dynamic);
        }

        /*************************************/

        protected override Tree<MethodBase> GetRelevantMethods()
        {
            Tree<MethodBase> root = new Tree<MethodBase> { Name = "Query methods" };

            foreach (MethodBase method in BH.Engine.Reflection.Query.GetBHoMMethodList().Where(x => x.DeclaringType.Name == "Query"))
                AddMethodToTree(root, method.DeclaringType.Namespace.Split('.').Skip(2), method);

            return root;
        }
    }
}