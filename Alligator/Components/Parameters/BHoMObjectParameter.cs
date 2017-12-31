using Grasshopper.Kernel;
using System;
using System.Collections.Generic;

namespace BH.UI.Alligator.Base
{
    public class BHoMObjectParameter : GH_PersistentParam<GH_BHoMObject>, IGH_PreviewObject
    {
        public BHoMObjectParameter()
            : base(new GH_InstanceDescription("BHoM object", "BHoM", "Represents a collection of generic BHoM objects", "Params", "Primitive"))
        {
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return null;
            }
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.tertiary;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("d3a2b455-74d5-4b26-bdf2-bf672d1dd927"); }
        }

        public override string TypeName
        {
            get
            {
                return "BHoM Objects";
            }
        }

        private bool m_hidden = false;
        public bool Hidden
        {
            get { return m_hidden; }
            set { m_hidden = value; }
        }
        public bool IsPreviewCapable
        {
            get { return true; }
        }

        public Rhino.Geometry.BoundingBox ClippingBox
        {
            get
            {
                return Preview_ComputeClippingBox();
            }
        }
        public void DrawViewportMeshes(IGH_PreviewArgs args)
        {
            Preview_DrawMeshes(args);
        }
        public void DrawViewportWires(IGH_PreviewArgs args)
        {
            Preview_DrawWires(args);
        }

        protected override GH_GetterResult Prompt_Singular(ref GH_BHoMObject value)
        {
            return GH_GetterResult.cancel;
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GH_BHoMObject> values)
        {
            return GH_GetterResult.cancel;
        }
    }
}
