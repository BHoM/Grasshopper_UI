using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.Engine.Base;
using BH.UI.Alligator.Query;
using BH.UI.Alligator.Base;

namespace BH.UI.Alligator.Geometry
{
    public class GetGeometry : GH_Component   //TODO: Requires corresonding method in engine 2.0
    {
        public GetGeometry() : base("Get Geometry", "GetGeometry",
            "Gets the Geometry of a BHoM Objects",
            "Structure", "Geometry")
        { }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }
        public override Guid ComponentGuid { get { return new Guid("{99745777-C5C7-44AB-B75E-A61D7E0D0B05}"); } }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "BHoMObject", "BHoM", "BHoMObject to get geometry from", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMGeometryParameter(), "Geometry", "G", "Geometry", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHoMObject obj = new BHoMObject();
            obj = DA.BH_GetData(0, obj);

            IBHoMGeometry geom = obj.GetGeometry();
            DA.BH_SetData(0, geom);
        }
    }
}
