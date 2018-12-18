using BH.UI.Grasshopper.Properties;
using Grasshopper.Kernel;
using System;
using System.Collections.Generic;

namespace BH.UI.Grasshopper.Objects
{
    public class Param_Dictionary : GH_PersistentParam<Engine.Grasshopper.Objects.GH_Dictionary>
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Icon { get; } = Resources.Dictionary_Param;

        public override GH_Exposure Exposure { get; } = GH_Exposure.tertiary;

        public override Guid ComponentGuid { get; } = new Guid("82AA94FD-F2D9-4DBD-9425-F4C9EA8A1C37");

        public override string TypeName { get; } = "Dictionary";

        public bool Hidden { get; set; } = false;

        public bool IsPreviewCapable { get; } = false;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public Param_Dictionary()
            : base(new GH_InstanceDescription("Dictionary", "Dictionary", "Represents an Dictionary", "Params", "Primitive"))
        {
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override GH_GetterResult Prompt_Singular(ref Engine.Grasshopper.Objects.GH_Dictionary value)
        {
            return GH_GetterResult.cancel;
        }

        /*******************************************/

        protected override GH_GetterResult Prompt_Plural(ref List<Engine.Grasshopper.Objects.GH_Dictionary> values)
        {
            return GH_GetterResult.cancel;
        }

        /*******************************************/
    }
}
