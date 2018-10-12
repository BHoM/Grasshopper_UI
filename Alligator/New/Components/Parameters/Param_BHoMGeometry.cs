using Grasshopper.Kernel;
using System;
using System.Collections.Generic;

namespace BH.UI.Alligator.Objects
{
    public class Param_BHoMGeometry : GH_PersistentParam<GH_IBHoMGeometry>, IGH_PreviewObject
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Icon { get; } = null;

        public override GH_Exposure Exposure { get; } = GH_Exposure.tertiary;

        public override Guid ComponentGuid { get; } = new Guid("EFD86C1F-D674-4905-A660-28C81A807080");

        public override string TypeName { get; } = "BHoM Geometry";

        public bool Hidden { get; set; } = false;

        public bool IsPreviewCapable { get; } = true; 

        public Rhino.Geometry.BoundingBox ClippingBox { get { return Preview_ComputeClippingBox(); } }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public Param_BHoMGeometry()
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