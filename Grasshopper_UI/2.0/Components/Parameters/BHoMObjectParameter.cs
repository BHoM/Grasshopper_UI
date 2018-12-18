using Grasshopper.Kernel;
using System;
using System.Collections.Generic;

namespace BH.UI.Grasshopper.Base
{
    public class BHoMObjectParameter : GH_PersistentParam<GH_BHoMObject>, IGH_PreviewObject
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Icon { get; } =  null;

        public override GH_Exposure Exposure { get; } = GH_Exposure.tertiary;

        public override Guid ComponentGuid { get; } = new Guid("d3a2b455-74d5-4b26-bdf2-bf672d1dd927"); 

        public override string TypeName { get; } = "BHoM Object";

        public bool Hidden { get; set; } = false;

        public bool IsPreviewCapable { get; } = true;

        public Rhino.Geometry.BoundingBox ClippingBox { get { return Preview_ComputeClippingBox(); } }

        public override bool Obsolete { get; } = true;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public BHoMObjectParameter()
            : base(new GH_InstanceDescription("BHoM object", "BHoM", "Represents a collection of generic BHoM objects", "Params", "Primitive"))
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

        protected override GH_GetterResult Prompt_Singular(ref GH_BHoMObject value)
        {
            return GH_GetterResult.cancel;
        }

        /*******************************************/

        protected override GH_GetterResult Prompt_Plural(ref List<GH_BHoMObject> values)
        {
            return GH_GetterResult.cancel;
        }

        /*******************************************/
    }
}
