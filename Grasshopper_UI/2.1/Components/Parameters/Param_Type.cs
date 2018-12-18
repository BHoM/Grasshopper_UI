using BH.UI.Grasshopper.Properties;
using Grasshopper.Kernel;
using System;
using System.Collections.Generic;

namespace BH.UI.Grasshopper.Objects
{
    public class Param_Type : GH_PersistentParam<Engine.Grasshopper.Objects.GH_Type>
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Icon { get; } = Resources.Type_Param;

        public override GH_Exposure Exposure { get; } = GH_Exposure.tertiary;

        public override Guid ComponentGuid { get; } = new Guid("AA7DDCDC-2789-4A23-88AD-E1E4CD84FB37");

        public override string TypeName { get; } = "Type";

        public bool Hidden { get; set; } = false;

        public bool IsPreviewCapable { get; } = false;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public Param_Type()
            : base(new GH_InstanceDescription("Object Type", "Type", "Represents the type of an object", "Params", "Primitive"))
        {
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override GH_GetterResult Prompt_Singular(ref Engine.Grasshopper.Objects.GH_Type value)
        {
            return GH_GetterResult.cancel;
        }

        /*******************************************/

        protected override GH_GetterResult Prompt_Plural(ref List<Engine.Grasshopper.Objects.GH_Type> values)
        {
            return GH_GetterResult.cancel;
        }

        /*******************************************/
    }
}
