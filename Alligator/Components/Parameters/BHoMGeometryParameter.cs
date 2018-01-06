using Grasshopper.Kernel;
using System;
using System.Collections.Generic;

namespace BH.UI.Alligator.Base
{
    public class BHoMGeometryParameter : GH_PersistentParam<GH_IBHoMGeometry>, IGH_PreviewObject
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Icon { get; } = Properties.Resources.BHoM_BHoM_Object;

        public override GH_Exposure Exposure { get; } = GH_Exposure.tertiary;

        public override Guid ComponentGuid { get; } = new Guid("a96cbe64-8352-47b1-9d24-153927d14795"); 

        public override string TypeName { get; } = "BHoM Geometry";

        public bool Hidden { get; set; } = false;

        public bool IsPreviewCapable { get; } = true; 

        public Rhino.Geometry.BoundingBox ClippingBox { get { return Preview_ComputeClippingBox(); } }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public BHoMGeometryParameter()
            : base(new GH_InstanceDescription("BHoM geometry", "BHoMGeo", "Represents a collection of generic BHoM geometries", "Params", "Geometry"))
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

        protected override GH_GetterResult Prompt_Singular(ref GH_IBHoMGeometry value)
        {
            return GH_GetterResult.cancel;
        }

        /*******************************************/

        protected override GH_GetterResult Prompt_Plural(ref List<GH_IBHoMGeometry> values)
        {
            return GH_GetterResult.cancel;
        }

        /*******************************************/
    }
}