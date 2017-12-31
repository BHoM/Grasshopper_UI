using System;
using System.Collections.Generic;
using ScriptComponents;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Parameters.Hints;
using BH.UI.Alligator.GeometryHints;

namespace BH.UI.Alligator.Base
{
    public class BH_CSNET_Script : Component_CSNET_Script
    {
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return Properties.Resources.BS_Script; } }
        protected override System.Drawing.Bitmap Icon { get { return Properties.Resources.BS_Script; } }
        public override Guid ComponentGuid { get { return new Guid("5703ec61-7e58-4fff-84e0-9e4043a02e74"); } }
        public override GH_Exposure Exposure { get { return GH_Exposure.primary; } }

        public BH_CSNET_Script() : base()
        {
            Name = "B# Script"; NickName = "B#"; Description = "A C#.Net scriptable component with BHoM custom features";
            Category = "Alligator"; SubCategory = "Scripting";
        }

        protected override string CreateSourceForEdit(ScriptSource code)
        {
            return base.CreateSourceForEdit(code);
        }

        protected override string CreateSourceForCompile(ScriptSource script)
        {
            script.References.Clear();
            string username = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1];
            script.References.Add("C:\\Users\\" + username + "\\AppData\\Roaming\\Grasshopper\\Libraries\\Alligator\\BHoM.dll");
            return base.CreateSourceForCompile(script);
        }
        protected override List<IGH_TypeHint> AvailableTypeHints
        {
            get
            {
                List<IGH_TypeHint> hints = base.AvailableTypeHints;
                hints.Insert(11, new BH_PointHint());
                hints.Insert(12, new BH_VectorHint());
                hints.Insert(13, new BH_LineHint());
                hints.Insert(14, new BH_PolylineHint());
                hints.Insert(15, new BH_NurbCurveHint());
                hints.Insert(16, new BH_MeshHint());
                hints.Insert(17, new GH_HintSeparator());
                return hints;
            }
        }
    }
}
