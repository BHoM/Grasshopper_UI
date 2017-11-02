using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Base;
using BH.UI.Alligator.Templates;
using BH.Adapter;
using System.IO;
using System.Reflection;

namespace BH.UI.Alligator.Base
{
    public class CreateAdapter : CreateObjectTemplate
    {
        public CreateAdapter() : base("CreateAdapter", "Adapter", "Creates a specific class of Adapter", "Alligator", "Adapter")
        {
            string folder = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Grasshopper\Libraries\Alligator\";
            foreach (string file in Directory.GetFiles(folder))
            {
                if (file.EndsWith("_Adapter.dll"))
                    Assembly.LoadFrom(file);
            }
        }
        public override Guid ComponentGuid { get { return new Guid("A2D956AE-98A8-486D-A2AB-371B45F8B3AE"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }
        public override GH_Exposure Exposure { get { return GH_Exposure.tertiary; } }

        /*************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Adapter", "Adapter", "Adapter", GH_ParamAccess.item);
        }

        /*************************************/

        protected override IEnumerable<Type> GetRelevantTypes()
        {
            Type adapterType = typeof(BHoMAdapter);
            return BH.Engine.Reflection.Create.AdapterTypeList().Where(x => x.IsSubclassOf(adapterType)).OrderBy(x => x.Name);
        }
  
    }
}