using BH.Engine.Rhinoceros;
using BHG = BH.oM.Geometry;
using RHG = Rhino.Geometry;
using Grasshopper.Kernel;
using System;
using System.Drawing;
using System.Linq;
using BH.Engine.Base;
using BH.oM.Geometry;

namespace BH.Engine.Alligator
{
    public static partial class Compute
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static void DrawViewportMeshes(IGeometry geom, GH_PreviewMeshArgs args)
        {
            if (geom == null)
            {
                return;
            }
            if (geom is BH.oM.Geometry.Mesh)
            {
                Compute.Render((BH.oM.Geometry.Mesh)geom, args);
            }
        }

        /***************************************************/

        public static void DrawViewportWires(IGeometry geom, GH_PreviewWireArgs args)
        {
            if (geom == null)
            {
                return;
            }
            Compute.IRender(geom as dynamic, args);
        }

        /***************************************************/
    }
}
