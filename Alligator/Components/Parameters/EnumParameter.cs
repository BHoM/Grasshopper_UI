using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using BH.Engine.Base;
using Grasshopper.Kernel.Parameters;

namespace BH.UI.Alligator.Base
{
    public class EnumParameter : GH_PersistentParam<GH_Enum>
    {
        public EnumParameter()
            : base(new GH_InstanceDescription("Enum", "Enum", "Represents an enum", "Params", "Primitive"))
        {
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return null;
            }
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.tertiary;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("53B91458-B41A-4EC8-A097-0833C88A1D7C"); }
        }

        public override string TypeName
        {
            get
            {
                return "Enum";
            }
        }

        private bool m_hidden = false;
        public bool Hidden
        {
            get { return m_hidden; }
            set { m_hidden = value; }
        }
        public bool IsPreviewCapable
        {
            get { return false; }
        }

        protected override GH_GetterResult Prompt_Singular(ref GH_Enum value)
        {
            return GH_GetterResult.cancel;
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GH_Enum> values)
        {
            return GH_GetterResult.cancel;
        }
    }
}
