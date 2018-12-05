using Grasshopper.Kernel;
using System;
using System.Collections.Generic;

namespace BH.UI.Alligator.Base
{
    public class EnumParameter : GH_PersistentParam<GH_Enum>
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Icon { get; } = null;

        public override GH_Exposure Exposure { get; } = GH_Exposure.tertiary;

        public override Guid ComponentGuid { get; } = new Guid("53B91458-B41A-4EC8-A097-0833C88A1D7C"); 

        public override string TypeName { get; } = "Enum";

        public bool Hidden { get; set; } = false;

        public bool IsPreviewCapable { get; } = false;

        public override bool Obsolete { get; } = true;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public EnumParameter() : base(new GH_InstanceDescription("Enum", "Enum", "Represents an enum", "Params", "Primitive"))
        {
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override GH_GetterResult Prompt_Singular(ref GH_Enum value)
        {
            return GH_GetterResult.cancel;
        }

        /*******************************************/

        protected override GH_GetterResult Prompt_Plural(ref List<GH_Enum> values)
        {
            return GH_GetterResult.cancel;
        }

        /*******************************************/
    }
}
