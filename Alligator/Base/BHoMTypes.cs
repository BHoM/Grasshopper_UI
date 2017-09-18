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
using BH.UI.Alligator.Base;

namespace BH.UI.Alligator.Base
{
    public class BH_Goo : GH_Goo<IObject>
    {
        #region Constructors
        public BH_Goo()
        {
            this.Value = null;
        }
        public BH_Goo(IObject bh)
        {
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
            if (source.GetType() == typeof(GH_Goo<IObject>))
            {
                this.Value = (IObject)source;
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
        #region Constructors
        public BHoMObjectParameter()
            : base(new GH_InstanceDescription("BHoM object", "BHoM", "Represents a collection of generic BHoM objects", "Params", "Primitive"))
        {
        }
        #endregion

        #region Properties
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
        #endregion
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
                this.Value = BH.Engine.Grasshopper.GeometryUtils.Convert((Rhino.Geometry.GeometryBase)source);
                return true;
            }
            if (typeof(GH_Point).IsAssignableFrom(source.GetType()))
            {
                Value = BH.Engine.Grasshopper.GeometryUtils.Convert(((GH_Point)source).Value);
            }
            else if (typeof(GH_Vector).IsAssignableFrom(source.GetType()))
            {
                Value = BH.Engine.Grasshopper.GeometryUtils.Convert(((GH_Vector)source).Value);
            }
            else if (typeof(GH_Line).IsAssignableFrom(source.GetType()))
            {
                Value = BH.Engine.Grasshopper.GeometryUtils.Convert(((GH_Line)source).Value);
            }
            else if (typeof(GH_Circle).IsAssignableFrom(source.GetType()))
            {
                Value = BH.Engine.Grasshopper.GeometryUtils.Convert(((GH_Circle)source).Value);
            }
            else if (typeof(GH_Curve).IsAssignableFrom(source.GetType()))
            {
                Value = BH.Engine.Grasshopper.GeometryUtils.Convert(((GH_Curve)source).Value);
            }
            else if (typeof(GH_Plane).IsAssignableFrom(source.GetType()))
            {
                Value = BH.Engine.Grasshopper.GeometryUtils.Convert(((GH_Plane)source).Value);
            }
            else if (typeof(GH_Surface).IsAssignableFrom(source.GetType()))
            {
                Value = BH.Engine.Grasshopper.GeometryUtils.Convert(((GH_Surface)source).Value);
            }
            else if (typeof(GH_Brep).IsAssignableFrom(source.GetType()))
            {
                Value = BH.Engine.Grasshopper.GeometryUtils.Convert(((GH_Brep)source).Value);
            }
            else if (typeof(GH_Mesh).IsAssignableFrom(source.GetType()))
            {
                Value = BH.Engine.Grasshopper.GeometryUtils.Convert(((GH_Mesh)source).Value);
            }
            return true;
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
        #region Constructors
        public BHoMGeometryParameter()
            : base(new GH_InstanceDescription("BHoM geometry", "BHoMGeo", "Represents a collection of generic BHoM geometries", "Params", "Geometry"))
        {
        }
        #endregion

        #region Properties
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
        #endregion
    }

    public static class RetrieveInput
    {
        public static bool BH_GetData<T>(this IGH_DataAccess DA, int index, T destination)
        {
            if (typeof(IBHoMGeometry).IsAssignableFrom(destination.GetType()))
            {
                BH_GeometricGoo bhg = new BH_GeometricGoo();
                if (!DA.GetData(index, ref bhg)) { return false; }
                destination = (T)bhg.Value;
                return true;
            }
            else if (typeof(IObject).IsAssignableFrom(destination.GetType()))
            {
                BH_Goo bho = new BH_Goo();
                if (!DA.GetData(index, ref bho)) { return false; }
                destination = (T)bho.Value;
                return true;
            }
            else { return DA.GetData(index, ref destination); }
        }

        public static bool BH_GetDataList<T>(this IGH_DataAccess DA, int index, List<T> destination)
        {
            if (typeof(IBHoMGeometry).IsAssignableFrom(typeof(T)))
            {
                List<BH_GeometricGoo> bhg = new List<BH_GeometricGoo>();
                if (!DA.GetDataList(index, bhg)) { return false; }
                destination.Clear();
                for (int i = 0; i < bhg.Count; i++)
                {
                    destination.Add((T)(bhg[i].Value));
                }
                return true;
            }
            else if (typeof(IObject).IsAssignableFrom(typeof(T)))
            {
                List<BH_Goo> bho = new List<BH_Goo>();
                if (!DA.GetDataList(index, bho)) { return false; }
                destination.Clear();
                for (int i = 0; i < bho.Count; i++)
                {
                    destination.Add((T)(bho[i].Value));
                }
                return true;
            }
            else { return DA.GetDataList(index, destination); }
        }

        public static bool BH_SetData<T>(this IGH_DataAccess DA, int index, T source)
        {
            if (typeof(IBHoMGeometry).IsAssignableFrom(source.GetType()))
            {
                BH_GeometricGoo bhg = new BH_GeometricGoo((source as IBHoMGeometry));
                return DA.SetData(index, bhg);
            }
            if (typeof(IObject).IsAssignableFrom(source.GetType()))
            {
                BH_Goo bho = new BH_Goo(source as IObject);
                return DA.SetData(index, bho);
            }
            else { return DA.SetData(index, source); }
        }
        public static bool BH_SetDataList<T>(this IGH_DataAccess DA, int index, List<T> source)
        {
            if (typeof(IBHoMGeometry).IsAssignableFrom(typeof(T)))
            {
                List<BH_GeometricGoo> bhg = new List<BH_GeometricGoo>();
                for (int i = 0; i < source.Count(); i++)
                {
                    bhg.Add(new BH_GeometricGoo(source[i] as IBHoMGeometry));
                }
                return DA.SetDataList(index, bhg);
            }
            if (typeof(IObject).IsAssignableFrom(typeof(T)))
            {
                List<BH_Goo> bho = new List<BH_Goo>();
                for (int i = 0; i < source.Count; i++)
                {
                    bho.Add(new BH_Goo(source[i] as IObject));
                }
                return DA.SetDataList(index, bho);
            }
            else { return DA.SetDataList(index, source); }

            //private static bool BH_GetData<T, T1, T2>(this IGH_DataAccess DA, int index, T destination)
            //    where T1 : IObject
            //    where T2 : IBHoMGeometry
            //{
            //    if (typeof(T1).IsAssignableFrom(typeof(IBHoMGeometry)) || typeof(T2).IsAssignableFrom(typeof(IBHoMGeometry)))
            //    {
            //        BH_GeometricGoo bhg = new BH_GeometricGoo();
            //        if (!DA.GetData(index, ref bhg)) { return false; }
            //        destination = (T)bhg.Value;
            //        return true;
            //    }
            //    else if (typeof(T1).IsAssignableFrom(typeof(IObject)) || typeof(T2).IsAssignableFrom(typeof(IObject)))
            //    {
            //        if (typeof(IBHoMGeometry).IsAssignableFrom(typeof(T))) { throw new ArgumentException("When dealing with BHoM Geometries you should use the DA.GetDataGeo method"); }
            //        BH_Goo bho = new BH_Goo();
            //        if (!DA.GetData(index, ref bho)) { return false; }
            //        destination = (T)bho.Value;
            //        return true;
            //    }
            //    else { return false; }
            //}
            //public static bool GetBHData<T>(this IGH_DataAccess DA, int index, T destination) where T : BHoMObject
            //{
            //    if (typeof(IBHoMGeometry).IsAssignableFrom(typeof(T))) { throw new ArgumentException("When dealing with BHoM Geometries you should use the DA.GetDataGeo method"); }
            //    BH_Goo bho = new BH_Goo();
            //    if (!DA.GetData(index, ref bho)) { return false; }
            //    destination = (T)bho.Value;
            //    return true;
            //}
            //public static bool GetBHGeo<T>(this IGH_DataAccess DA, int index, T destination) where T : IBHoMGeometry
            //{
            //    if (typeof(BHoMObject).IsAssignableFrom(typeof(T))) { throw new ArgumentException("When dealing with BHoM Geometries you should use the DA.GetDataGeo method"); }
            //    BH_GeometricGoo bhg = new BH_GeometricGoo();
            //    if (!DA.GetData(index, ref bhg)) { return false; }
            //    destination = (T)bhg.Value;
            //    return true;
            //}
            //public static bool GetBHGeoList<T>(this IGH_DataAccess DA, int index, List<T> destination) where T : IBHoMGeometry
            //{
            //    List<BH_GeometricGoo> bhg = new List<BH_GeometricGoo>();
            //    if (!DA.GetDataList(index, bhg)) { return false; }
            //    destination.Clear();
            //    for (int i = 0; i < bhg.Count; i++)
            //    {
            //        destination.Add((T)(bhg[i].Value));
            //    }
            //    return true;
            //}
            //public static bool GetBHDataList<T>(this IGH_DataAccess DA, int index, List<T> destination) where T : BHoMObject
            //{
            //    List<BH_Goo> bho = new List<BH_Goo>();
            //    if (!DA.GetDataList(index, bho)) { return false; }
            //    destination.Clear();
            //    for (int i = 0; i < bho.Count; i++)
            //    {
            //        destination.Add((T)(bho[i].Value));
            //    }
            //    return true;
            //}

            //public static bool BH_SetDataGeo<T>(this IGH_DataAccess DA, int index, T source) where T : IBHoMGeometry
            //{
            //    BH_GeometricGoo bhg = new BH_GeometricGoo((T)source);
            //    return DA.SetData(index, bhg);
            //}
            //public static bool BH_SetData<T>(this IGH_DataAccess DA, int index, T source) where T : BHoMObject
            //{
            //    BH_Goo bho = new BH_Goo((T)source);
            //    return DA.SetData(index, bho);
            //}
            //public static bool BH_SetDataGeoList<T>(this IGH_DataAccess DA, int index, List<T> source) where T : IBHoMGeometry
            //{
            //    List<BH_GeometricGoo> bhg = new List<BH_GeometricGoo>();
            //    for (int i = 0; i < source.Count(); i++)
            //    {
            //        bhg.Add(new BH_GeometricGoo((T)source[i]));
            //    }
            //    return DA.SetDataList(index, bhg);
            //}
            //public static bool BH_SetDataList<T>(this IGH_DataAccess DA, int index, List<T> source) where T : BHoMObject
            //{
            //    List<BH_Goo> bho = new List<BH_Goo>();
            //    for (int i = 0; i < source.Count; i++)
            //    {
            //        bho.Add(new BH_Goo((T)source[DA.Iteration]));
            //    }
            //    return DA.SetDataList(index, bho);
            //}
        }
    }
}
