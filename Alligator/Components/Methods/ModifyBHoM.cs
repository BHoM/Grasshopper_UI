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
    public class ModifyBHoM : MethodCallTemplate
    {
        public ModifyBHoM() : base("Modify BHoM Object", "ModifyBHoM", "Modify a BHoMObject", "Alligator", "Base")
        {
        }
        public override Guid ComponentGuid { get { return new Guid("C275B1A2-BB2D-4F3B-8D5C-18C78456A831"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }

        public override GH_Exposure Exposure { get { return GH_Exposure.secondary; } }


        /*************************************/

        protected override Tree<MethodBase> GetRelevantMethods()
        {
            Type enumerableType = typeof(IEnumerable);
            Tree<MethodBase> root = new Tree<MethodBase> { Name = "Select modifier" };

            foreach (MethodBase method in BH.Engine.Reflection.Query.GetBHoMMethodList().Where(x => x.DeclaringType.Name == "Transform")) //TODO: Should be "Modify" here
            {
                AddMethodToTree(root, method);
            }
                

            return root;
        }
    }
}