using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Base;
using BH.UI.Alligator.Templates;
using System.IO;
using System.Reflection;

namespace BH.UI.Alligator.Base
{
    public class CreateBHoMObject : CreateObjectTemplate
    {
        public CreateBHoMObject() : base("CreateBHoMObject", "BHoMObj", "Creates a specific class of BHoMObject", "Alligator", "Base")
        {
            string folder = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Grasshopper\Libraries\Alligator\";
            foreach (string file in Directory.GetFiles(folder))
            {
                if (file.EndsWith("oM.dll"))
                    Assembly.LoadFrom(file);
            }
        }
        public override Guid ComponentGuid { get { return new Guid("0E1C95EB-1546-47D4-89BB-776F7920622D"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }

        /*************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "BHoMObject", "object", "BHoMObject", GH_ParamAccess.item);
        }

        /*************************************/

        protected override void SetData(IGH_DataAccess DA, object result)
        {
            DA.SetData(0, result as BHoMObject);
        }

        /*************************************/

        protected override IEnumerable<Type> GetRelevantTypes()
        {
            Type bhomType = typeof(BHoMObject);
            return BH.Engine.Reflection.Create.TypeList().Where(x => x.IsSubclassOf(bhomType) && !x.ContainsGenericParameters).OrderBy(x => x.Name);
        }
  
    }
}