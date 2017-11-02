using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Chrome.Views;
using BH.UI.Alligator.Templates;
using Grasshopper.Kernel;

namespace BH.UI.Alligator.Base
{
    public class CreateView : CreateObjectTemplate
    {
        public CreateView() : base("CreateView", "View", "Creates a specific view for data sent to Chrome", "Alligator", "Chrome") { }
        public override Guid ComponentGuid { get { return new Guid("1D91BF89-3111-40B0-A1F5-19460941493F"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }

        /*************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("View", "View", "View", GH_ParamAccess.item);
        }

        /*************************************/

        protected override IEnumerable<Type> GetRelevantTypes()
        {
            Type viewType = typeof(IView);
            return BH.Engine.Reflection.Create.TypeList().Where(x => viewType.IsAssignableFrom(x)).OrderBy(x => x.Name);
        }
    }
}