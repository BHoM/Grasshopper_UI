using BHoM.Structural;
using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using GHE = Grasshopper_Engine;
using Alligator.Components;
using BHE = BHoM.Structural.Elements;
using BHI = BHoM.Structural.Interface;
using BHP = BHoM.Structural.Properties;
using BHG = BHoM.Geometry;
using System.Windows.Forms;
using R = Rhino.Geometry;
using Grasshopper;
using GHKT = Grasshopper.Kernel.Types;
using Grasshopper_Engine.Components;
using Grasshopper.Kernel.Data;

namespace Alligator.Structural.Elements
{
    public class ImportFEMesh : ImportComponent<BHE.FEMesh>
    {
        public ImportFEMesh() : base("Import FEMesh", "GetFEMesh", "Get the geometry and properties of a FEMesh", "Structure", "Elements")
        {

        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.tertiary;
            }
        }

      

        public override List<BHE.FEMesh> GetObjects(BHI.IElementAdapter app, List<string> objectIds, out IGH_DataTree geom, out List<string> outIds)
        {
            List<BHE.FEMesh> result = null;
            outIds = app.GetFEMeshes(out result, objectIds);

            DataTree<R.Mesh> meshes = new DataTree<R.Mesh>();

            foreach (BHE.FEMesh feMesh in result)
            {
                meshes.Add(GHE.GeometryUtils.FeMeshToRhinoMesh(feMesh));
            }
            geom = meshes;
            return result;
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("8CDF4B7D-D51E-48A8-B387-069C79527A0B"); }
        }


    }

}