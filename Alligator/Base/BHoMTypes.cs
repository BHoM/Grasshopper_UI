using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.UI.Alligator.Base
{
    public class BH_Goo : GH_Goo<BHoMObject>
    {
        #region Constructors
        public BH_Goo()
        {
            this.Value = new BHoMObject();
        }
        public BH_Goo(BHoMObject bh)
        {
            if (bh == null)
                bh = new BHoMObject();
            this.Value = bh;
        }
        #endregion

        #region Properties
        public override bool IsValid
        {
            get
            {
                if (Value == null) { return false; }
                return Value != null;
            }
        }
        public override IGH_Goo Duplicate()
        {
            return new BH_Goo();
        }
        public override string ToString()
        {
            if (Value == null)
                return "Null BHoMObject";
            else
                return Value.ToString();
        }
        public override string TypeName
        {
            get { return ("BHoMObject"); }
        }
        public override string TypeDescription
        {
            get { return ("Defines a generic BHoMObject"); }
        }
        #endregion

        #region Casting Methods
        public override bool CastFrom(object source)
        {
            if (source == null) { return false; }
            if (source.GetType() == typeof(GH_Goo<BHoMObject>))
            {
                this.Value = (BHoMObject)source;
                return true;
            }
            return base.CastFrom(source);
        }
        public override bool CastTo<Q>(ref Q target)
        {
            object ptr = this.Value;
            target = (Q)ptr;
            return true;
        }
        #endregion
    }

    public class BHoMObjectParameter : GH_Param<BH_Goo>
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

        //We do not allow users to pick BHoMObjects, therefore the following 4 methods disable all this ui.
        //protected override GH_GetterResult Prompt_Plural(ref List<BH_Goo> values)
        //{
        //    return GH_GetterResult.accept;
        //}
        //protected override GH_GetterResult Prompt_Singular(ref BH_Goo value)
        //{
        //    return GH_GetterResult.accept;
        //}
        //protected override System.Windows.Forms.ToolStripMenuItem Menu_CustomSingleValueItem()
        //{
        //    System.Windows.Forms.ToolStripMenuItem item = new System.Windows.Forms.ToolStripMenuItem();
        //    item.Text = "Not available";
        //    item.Visible = false;
        //    return item;
        //}
        //protected override System.Windows.Forms.ToolStripMenuItem Menu_CustomMultiValueItem()
        //{
        //    System.Windows.Forms.ToolStripMenuItem item = new System.Windows.Forms.ToolStripMenuItem();
        //    item.Text = "Not available";
        //    item.Visible = false;
        //    return item;
        //}

        private bool m_hidden = false;
        public bool Hidden
        {
            get { return m_hidden; }
            set { m_hidden = value; }
        }
        public bool IsPreviewCapable
        {
            get { return false; }
        }
    }

    public class BH_GeometricGoo : GH_GeometricGoo<IBHoMGeometry>, IGH_PreviewData
    {
        #region Constructors
        public BH_GeometricGoo()
        {
            this.Value = null;
        }
        public BH_GeometricGoo(IBHoMGeometry bh)
        {
            this.Value = bh;
        }
        public override IGH_GeometricGoo DuplicateGeometry()
        {
            return new BH_GeometricGoo { Value = Value.GetClone() };
        }
        #endregion

        #region Properties
        public override bool IsValid
        {
            get
            {
                if (Value == null) { return false; }
                return true;
            }
        }
        public override string ToString()
        {
            if (Value == null)
                return "Null IBHoMGeometry";
            else
                return Value.ToString();
        }
        public override string TypeName
        {
            get { return ("IBHoMGeometry"); }
        }
        public override string TypeDescription
        {
            get { return ("Defines a generic IBHoMGeometry"); }
        }

        public override Rhino.Geometry.BoundingBox Boundingbox
        {
            get
            {
                if (Value == null) { return Rhino.Geometry.BoundingBox.Empty; }
                if (Value == null) { return Rhino.Geometry.BoundingBox.Empty; }
                BoundingBox bb = Value.GetBounds(); // TODO Replace with a proper conversion between Rhino and BH BoundingBox
                return new Rhino.Geometry.BoundingBox(bb.Min.X, bb.Min.Y, bb.Min.Z, bb.Max.X, bb.Max.Y, bb.Max.Z);
            }
        }
        public override Rhino.Geometry.BoundingBox GetBoundingBox(Rhino.Geometry.Transform xform)
        {
            if (Value == null) { return Rhino.Geometry.BoundingBox.Empty; }
            if (Value == null) { return Rhino.Geometry.BoundingBox.Empty; }
            BoundingBox bb = Value.GetBounds(); // TODO Replace with a proper conversion between Rhino and BH BoundingBox
            return new Rhino.Geometry.BoundingBox(bb.Min.X, bb.Min.Y, bb.Min.Z, bb.Max.X, bb.Max.Y, bb.Max.Z);
        }
        #endregion

        #region Casting Methods
        public override bool CastFrom(object source)
        {
            if (source == null) { return false; }
            if (typeof(IBHoMGeometry).IsAssignableFrom(source.GetType()))
            {
                this.Value = (IBHoMGeometry)source;
                return true;
            }
            if (typeof(Rhino.Geometry.GeometryBase).IsAssignableFrom(source.GetType()))
            {
                this.Value = BH.Engine.Grasshopper.GeometryUtils.Convert((Rhino.Geometry.GeometryBase) source);
                return true;
            }
            return base.CastFrom(source);
        }
        public override bool CastTo<Q>(ref Q target)
        {
            object ptr = this.Value;
            target = (Q)ptr;
            return true;
        }
        #endregion

        #region Transformation Methods
        public override IGH_GeometricGoo Transform(Rhino.Geometry.Transform xform)
        {
            // This does not make any sense, so I will just do a validity check
            if (Value == null) { return null; }
            return new BH_GeometricGoo();
        }
        public override IGH_GeometricGoo Morph(Rhino.Geometry.SpaceMorph xmorph)
        {
            // This does not make any sense, so I will just do a validity check
            if (Value == null) { return null; }
            return new BH_GeometricGoo();
        }
        #endregion

        #region Drawing methods
        public Rhino.Geometry.BoundingBox ClippingBox
        {
            get { return Boundingbox; }
        }
        public void DrawViewportMeshes(GH_PreviewMeshArgs args)
        {
            if (Value.GetType() == typeof(BH.oM.Geometry.Mesh))
            {
                // TODO Implement a conversion from BH Mesh to Rhino Mesh
            }
        }
        public void DrawViewportWires(GH_PreviewWireArgs args)
        {
            if (Value == null) { return; }

            // Draw hull shape.
            if (Value.GetType() == typeof(BH.oM.Geometry.Line))
            {
                // TODO Provide conversion of Value from BH to Rhino
                // args.Pipeline.DrawPolyline(Value, args.Color, -1);
            }
            // Etc. for all the basis geometries
        }
        #endregion
    }

    public class BHoMGeometryParameter : GH_Param<BH_GeometricGoo>
    {
        public BHoMGeometryParameter()
            : base(new GH_InstanceDescription("BHoM geometry", "BHoMGeo", "Represents a collection of generic BHoM geometries", "Params", "Geometry"))
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
            get { return new Guid("a96cbe64-8352-47b1-9d24-153927d14795"); }
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
    }
}