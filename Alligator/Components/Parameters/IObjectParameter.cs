using Grasshopper.Kernel;
using System;
using System.Collections.Generic;

namespace BH.UI.Alligator.Base
{
    public class IObjectParameter : GH_PersistentParam<GH_IObject>, IGH_PreviewObject
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Icon { get; } = Properties.Resources.IObject_Param;

        public override GH_Exposure Exposure { get; } = GH_Exposure.tertiary;

        public override Guid ComponentGuid { get; } = new Guid("B2D0BA7B-2802-491A-92C6-C55C96178598");

        public override string TypeName { get; } = "IObject";

        public bool Hidden { get; set; } = false;

        public bool IsPreviewCapable { get; } = true;

        public Rhino.Geometry.BoundingBox ClippingBox { get { return Preview_ComputeClippingBox(); } }

        public override bool Obsolete { get; } = true;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public IObjectParameter()
            : base(new GH_InstanceDescription("BH IObject", "IObject", "Represents a collection of generic BH IObjects", "Params", "Primitive"))
        {
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public void DrawViewportMeshes(IGH_PreviewArgs args)
        {
            Preview_DrawMeshes(args);
        }

        /*******************************************/

        public void DrawViewportWires(IGH_PreviewArgs args)
        {
            Preview_DrawWires(args);
        }

        /*******************************************/

        protected override GH_GetterResult Prompt_Singular(ref GH_IObject value)
        {
            return GH_GetterResult.cancel;
        }

        /*******************************************/

        protected override GH_GetterResult Prompt_Plural(ref List<GH_IObject> values)
        {
            return GH_GetterResult.cancel;
        }

        /*******************************************/
    }
}
