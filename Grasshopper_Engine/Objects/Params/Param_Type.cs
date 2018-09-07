using Grasshopper.Kernel;
using System;
using System.Collections.Generic;

namespace BH.Engine.Alligator.Objects
{
    public class Param_Type : GH_PersistentParam<GH_Type>
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Icon { get; } = null;

        public override GH_Exposure Exposure { get; } = GH_Exposure.tertiary;

        public override Guid ComponentGuid { get; } = new Guid("3D9B5B7E-86CD-419E-9126-5909AA45DA5A");

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
