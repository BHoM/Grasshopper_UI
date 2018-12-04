using Grasshopper.Kernel;
using System;
using System.Collections.Generic;

namespace BH.UI.Alligator.Base
{
    public class DictionaryParameter : GH_PersistentParam<GH_Dictionary>
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Icon { get; } = Properties.Resources.Dictionary_Param;

        public override GH_Exposure Exposure { get; } = GH_Exposure.tertiary;

        public override Guid ComponentGuid { get; } = new Guid("0F6CA969-BB8A-4B5C-9225-104AFE549DE2"); 

        public override string TypeName { get; } = "Dictionary";

        public bool Hidden { get; set; } = false;

        public bool IsPreviewCapable { get; } = false;

        public override bool Obsolete { get; } = true;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public DictionaryParameter()
            : base(new GH_InstanceDescription("Dictionary", "Dictionary", "Represents an Dictionary", "Params", "Primitive"))
        {
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override GH_GetterResult Prompt_Singular(ref GH_Dictionary value)
        {
            return GH_GetterResult.cancel;
        }

        /*******************************************/

        protected override GH_GetterResult Prompt_Plural(ref List<GH_Dictionary> values)
        {
            return GH_GetterResult.cancel;
        }

        /*******************************************/
    }
}
