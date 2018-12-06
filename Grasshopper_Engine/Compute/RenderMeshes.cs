using BH.Engine.Rhinoceros;
using BHG = BH.oM.Geometry;
using RHG = Rhino.Geometry;
using Rhino.Display;
using Grasshopper.Kernel;
using System;
using System.Drawing;
using System.Linq;
using BH.Engine.Base;
using System.Collections.Generic;

namespace BH.Engine.Alligator
{
    public static partial class Compute
    {
        /***************************************************/
        /**** Public Methods  - Interfaces              ****/
        /***************************************************/

        public static bool IRenderMeshes(this BH.oM.Base.IBHoMObject bhObject, GH_PreviewMeshArgs args)
        {
            if (bhObject == null) { return false; }
            args.Pipeline.ZBiasMode = 0;
            DisplayMaterial bhMaterial = Query.RenderMaterial(args.Material);
            try
            {
                RenderMeshes(bhObject as dynamic, args.Pipeline, bhMaterial);
            }
            catch (Exception) { }
            return false;
        }

        /***************************************************/

        public static void IRenderMeshes(this BHG.IGeometry geometry, GH_PreviewMeshArgs args)
        {
            if (geometry == null) { return; }
            args.Pipeline.ZBiasMode = 0;
            Color bhColour = Query.RenderColour(args.Material.Diffuse);
            DisplayMaterial bhMaterial = Query.RenderMaterial(args.Material);
            try
            {
                RenderMeshes(geometry as dynamic, args.Pipeline, bhMaterial);
            }
            catch (Exception) { }
        }


        /***************************************************/
        /**** Public Methods  - Objects                 ****/
        /***************************************************/

        public static void RenderMeshes(BH.oM.Base.BHoMObject obj, GH_PreviewMeshArgs args)
        {
            IRenderMeshes(obj.IGeometry(), args);
        }


        /***************************************************/
        /**** Public Methods  - Vectors                 ****/
        /***************************************************/

        public static void RenderMeshes(BHG.Point point, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            return;
        }

        /***************************************************/

        public static void RenderMeshes(BHG.Vector vector, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            return;
        }

        /***************************************************/

        public static void RenderMeshes(BHG.Plane plane, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            return;
        }

        /***************************************************/

        public static void RenderMeshes(BHG.CoordinateSystem coordinateSystem, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            return;
        }


        /***************************************************/
        /**** Public Methods  - Curves                  ****/
        /***************************************************/

        public static void RenderMeshes(BHG.Arc arc, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            return;
        }

        /***************************************************/

        public static void RenderMeshes(BHG.Circle circle, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            return;
        }

        /***************************************************/

        public static void RenderMeshes(BHG.Ellipse ellipse, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            return;
        }

        /***************************************************/

        public static void RenderMeshes(BHG.Line line, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            return;
        }
        /***************************************************/

        public static void RenderMeshes(BHG.NurbCurve curve, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            return;
        }

        /***************************************************/

        public static void RenderMeshes(BHG.PolyCurve polycurve, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            return;
        }

        /***************************************************/

        public static void RenderMeshes(BHG.Polyline polyline, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            return;
        }


        /***************************************************/
        /**** Public Methods  - Surfaces                ****/
        /***************************************************/

        public static void RenderMeshes(BHG.Extrusion surface, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            pipeline.DrawBrepShaded(RHG.Brep.CreateFromSurface(surface.ToRhino()), material);
        }

        /***************************************************/

        public static void RenderMeshes(BHG.Loft surface, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            pipeline.DrawBrepShaded(RHG.Brep.CreateFromSurface(surface.ToRhino()), material);
        }

        /***************************************************/

        public static void RenderMeshes(BHG.NurbSurface surface, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            pipeline.DrawBrepShaded(RHG.Brep.CreateFromSurface(surface.ToRhino()), material);
        }

        /***************************************************/

        public static void RenderMeshes(BHG.Pipe surface, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            pipeline.DrawBrepShaded(surface.ToRhino(), material);
        }

        /***************************************************/

        public static void RenderMeshes(BHG.PolySurface polySurface, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            List<BHG.ISurface> surfaces = polySurface.Surfaces;
            for (int i=0; i< surfaces.Count; i++)
            {
                pipeline.DrawBrepShaded(RHG.Brep.CreateFromSurface(surfaces[i].IToRhino()), material);
            }
        }


        /***************************************************/
        /**** Public Methods  - Mesh                    ****/
        /***************************************************/

        public static void RenderMeshes(BHG.Mesh mesh, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            RHG.Mesh rMesh = mesh.ToRhino();
            pipeline.DrawMeshShaded(rMesh, material);
        }


        /***************************************************/
        /**** Public Methods  - Miscellanea             ****/
        /***************************************************/

        public static void RenderMeshes(BHG.BoundingBox bbBox, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            pipeline.DrawBrepShaded(RHG.Brep.CreateFromBox(bbBox.ToRhino()), material);
        }

        /***************************************************/

        public static void RenderMeshes(BHG.CompositeGeometry composite, Rhino.Display.DisplayPipeline pipeline, DisplayMaterial material)
        {
            if (composite.Elements.Count == 0) { return; }
            foreach (BHG.IGeometry geom in composite.Elements)
            {
                try
                {
                    RenderMeshes(geom as dynamic, pipeline, material);
                }
                catch (Exception) { }
            }
        }

        /***************************************************/
    }
}
