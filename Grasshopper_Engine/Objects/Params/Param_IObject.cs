using Grasshopper.Kernel;
using System;
using System.Collections.Generic;

namespace BH.Engine.Alligator.Objects
{
    public class Param_IObject : GH_PersistentParam<GH_IObject>, IGH_PreviewObject
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Icon { get; } =  null;

        public override GH_Exposure Exposure { get; } = GH_Exposure.tertiary;

        public override Guid ComponentGuid { get; } = new Guid("497A7FAD-78B9-47E6-9676-08ED276F5479");

        public override string TypeName { get; } = "IObject";

        public bool Hidden { get; set; } = false;

        public bool IsPreviewCapable { get; } = true;

        public Rhino.Geometry.BoundingBox ClippingBox { get { return Preview_ComputeClippingBox(); } }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public Param_IObject()
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
