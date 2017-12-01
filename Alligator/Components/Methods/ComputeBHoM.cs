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
    public class ComputeBHoM : MethodCallTemplate
    {
        public ComputeBHoM() : base("Compute / Analyse", "ComputeBHoM", "Run a computationally intensive calculation", "Alligator", "Base")
        {
        }
        public override Guid ComponentGuid { get { return new Guid("9A94F1C4-AF5B-48E6-B0DD-F56145DEEDDA"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }

        public override GH_Exposure Exposure { get { return GH_Exposure.secondary; } }


        /*************************************/

        protected override Tree<MethodBase> GetRelevantMethods()
        {
            Type enumerableType = typeof(IEnumerable);
            Tree<MethodBase> root = new Tree<MethodBase> { Name = "Select Computation" };

            foreach (MethodBase method in BH.Engine.Reflection.Query.GetBHoMMethodList().Where(x => x.DeclaringType.Name == "Compute")) 
            {
                AddMethodToTree(root, method);
            }
                

            return root;
        }

        /*************************************/

        protected override void AddMethodToTree(Tree<MethodBase> tree, MethodBase method) //2. Helper function to build your catalogue of methods
        {
            IEnumerable<string> path = method.DeclaringType.Namespace.Split('.').Skip(2);
            AddMethodToTree(tree, path, method);
        }

        /*************************************/
    }
}