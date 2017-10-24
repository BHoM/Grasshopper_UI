using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using BH.Engine.Base;
using Grasshopper.Kernel.Parameters;
using BH.UI.Alligator;

namespace BH.UI.Alligator.Base
{
    public class BHoMGeometryParameter : GH_Param<GH_IBHoMGeometry>, IGH_PreviewObject
    {
        public BHoMGeometryParameter()
            : base(new GH_InstanceDescription("BHoM geometry", "BHoMGeo", "Represents a collection of generic BHoM geometries", "Params", "Geometry"))
        {
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.BHoM_BHoM_Object;
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
            get { return new Guid("a96cbe64-8352-47b1-9d24-153927d14795"); }
        }

        public override string TypeName
        {
            get
            {
                return "BHoM Geometry";
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
    }
}