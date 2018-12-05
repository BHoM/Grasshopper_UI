using BH.UI.Alligator.Base.Properties;
using Grasshopper.Kernel;
using System;
using System.Collections.Generic;

namespace BH.UI.Alligator.Objects
{
    public class Param_Enum : GH_PersistentParam<Engine.Alligator.Objects.GH_Enum>
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Icon { get; } = Resources.Enum_Param;

        public override GH_Exposure Exposure { get; } = GH_Exposure.tertiary;

        public override Guid ComponentGuid { get; } = new Guid("C62F4BD6-9B7F-4A81-94DD-CA16B2D8D3EC");

        public override string TypeName { get; } = "Enum";

        public bool Hidden { get; set; } = false;

        public bool IsPreviewCapable { get; } = false;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public Param_Enum() : base(new GH_InstanceDescription("Enum", "Enum", "Represents an enum", "Params", "Primitive"))
        {
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override GH_GetterResult Prompt_Singular(ref Engine.Alligator.Objects.GH_Enum value)
        {
            return GH_GetterResult.cancel;
        }

        /*******************************************/

        protected override GH_GetterResult Prompt_Plural(ref List<Engine.Alligator.Objects.GH_Enum> values)
        {
            return GH_GetterResult.cancel;
        }

        /*******************************************/
    }
}
