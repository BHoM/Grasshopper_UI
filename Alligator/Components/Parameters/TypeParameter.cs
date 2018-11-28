using Grasshopper.Kernel;
using System;
using System.Collections.Generic;

namespace BH.UI.Alligator.Base
{
    public class TypeParameter : GH_PersistentParam<GH_Type>
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Icon { get; } = null;

        public override GH_Exposure Exposure { get; } = GH_Exposure.tertiary;

        public override Guid ComponentGuid { get; } = new Guid("D59B3EE2-41A0-4231-A74D-0B79D51C6B37"); 

        public override string TypeName { get; } = "Type";

        public bool Hidden { get; set; } = false;

        public bool IsPreviewCapable { get; } = false;

        public override bool Obsolete { get; } = true;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public TypeParameter()
            : base(new GH_InstanceDescription("Object Type", "Type", "Represents the type of an object", "Params", "Primitive"))
        {
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override GH_GetterResult Prompt_Singular(ref GH_Type value)
        {
            return GH_GetterResult.cancel;
        }

        /*******************************************/

        protected override GH_GetterResult Prompt_Plural(ref List<GH_Type> values)
        {
            return GH_GetterResult.cancel;
        }

        /*******************************************/
    }
}
