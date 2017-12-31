using Grasshopper.Kernel;
using System;
using System.Collections.Generic;

namespace BH.UI.Alligator.Base
{
    public class DictionaryParameter : GH_PersistentParam<GH_Dictionary>
    {
        public DictionaryParameter()
            : base(new GH_InstanceDescription("Dictionary", "Dictionary", "Represents an Dictionary", "Params", "Primitive"))
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
            get { return new Guid("0F6CA969-BB8A-4B5C-9225-104AFE549DE2"); }
        }

        public override string TypeName
        {
            get
            {
                return "Dictionary";
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

        protected override GH_GetterResult Prompt_Singular(ref GH_Dictionary value)
        {
            return GH_GetterResult.cancel;
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GH_Dictionary> values)
        {
            return GH_GetterResult.cancel;
        }
    }
}
