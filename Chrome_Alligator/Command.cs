using System;
using System.Collections.Generic;
using System.Linq;
using BH.UI.Alligator.Templates;
using Grasshopper.Kernel;
using BH.oM.Chrome.Commands;

namespace BH.UI.Alligator.Chrome
{
    public class Command : CreateObjectTemplate
    {
        public Command() : base("ChromeCommand", "Command", "Creates a command to execute in Chrome", "Alligator", "Chrome") { }
        public override Guid ComponentGuid { get { return new Guid("595933A0-A1B2-483C-974C-3A80818350EB"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }

        /*************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Command", "Command", "Command", GH_ParamAccess.item);
        }

        /*************************************/

        protected override IEnumerable<Type> GetRelevantTypes()
        {
            Type viewType = typeof(ICommand);
            return BH.Engine.Reflection.Create.TypeList().Where(x => viewType.IsAssignableFrom(x)).OrderBy(x => x.Name);
        }
    }
}