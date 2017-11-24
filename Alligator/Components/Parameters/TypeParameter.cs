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
    public class TypeParameter : GH_PersistentParam<GH_Type>
    {
        public TypeParameter()
            : base(new GH_InstanceDescription("Object Type", "Type", "Represents the type of an object", "Params", "Primitive"))
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
            get { return new Guid("D59B3EE2-41A0-4231-A74D-0B79D51C6B37"); }
        }

        public override string TypeName
        {
            get
            {
                return "Type";
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

        protected override GH_GetterResult Prompt_Singular(ref GH_Type value)
        {
            return GH_GetterResult.cancel;
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GH_Type> values)
        {
            return GH_GetterResult.cancel;
        }
    }
}
