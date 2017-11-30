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
    public class ConvertBHoM : MethodCallTemplate
    {
        public ConvertBHoM() : base("Convert BHoM Object", "ConvertBHoM", "Convert a BHoMObject", "Alligator", "Base")
        {
        }
        public override Guid ComponentGuid { get { return new Guid("D517E0BF-E979-4441-896E-1D2EC833FE2E"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }

        public override GH_Exposure Exposure { get { return GH_Exposure.secondary; } }


        /*************************************/

        protected override Tree<MethodBase> GetRelevantMethods()
        {
            Type enumerableType = typeof(IEnumerable);
            Tree<MethodBase> root = new Tree<MethodBase> { Name = "Select converter" };

            foreach (MethodBase method in BH.Engine.Reflection.Query.GetBHoMMethodList().Where(x => x.DeclaringType.Name == "Convert")) 
            {
                AddMethodToTree(root, method);
            }
                

            return root;
        }
    }
}