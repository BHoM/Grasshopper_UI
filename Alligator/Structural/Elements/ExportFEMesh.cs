using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using System.Linq;
using GHE = Grasshopper_Engine;
using BHE = BHoM.Structural.Elements;
using BHI = BHoM.Structural.Interface;
using GH_IO.Serialization;

namespace Alligator.Structural.Elements
{


    public class ExportFEMesh : GHE.Components.ExportComponent<BHE.FEMesh>
    {
        public ExportFEMesh() : base("Export FEMesh", "SetMesh", "Creates or Replaces the geometry of a Mesh", "Structure", "Elements")
        { }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_Mesh_Export; }
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("BC7719CD-C8D4-4179-A9FC-7ACFDAAB9F68");
            }
        }

        protected override List<BHE.FEMesh> SetObjects(BHI.IElementAdapter app, List<BHE.FEMesh> objects, out List<string> ids)
        {
            app.SetFEMeshes(objects, out ids);

            return objects;
        }
    }
}
