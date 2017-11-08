using System;
using System.Collections.Generic;
using System.Linq;
using BH.UI.Alligator.Templates;
using Grasshopper.Kernel;
using BH.oM.Chrome.DataTypes;

namespace BH.UI.Alligator.Chrome
{
    public class DataType : CreateObjectTemplate
    {
        public DataType() : base("ChromeDataType", "DataType", "Define the data type to be sent to Chrome", "Alligator", "Chrome") { }
        public override Guid ComponentGuid { get { return new Guid("446DD9CE-CFC8-4471-B0A9-9F88481DDC3A"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }
        public override GH_Exposure Exposure { get { return GH_Exposure.primary; } }

        /*************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("DataType", "DataType", "DataType", GH_ParamAccess.item);
        }

        /*************************************/

        protected override IEnumerable<Type> GetRelevantTypes()
        {
            Type viewType = typeof(IDataType);
            return BH.Engine.Reflection.Create.TypeList().Where(x => viewType.IsAssignableFrom(x)).OrderBy(x => x.Name);
        }
    }
}