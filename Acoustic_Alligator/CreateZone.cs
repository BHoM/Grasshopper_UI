using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BHG = BH.oM.Geometry;
using BH.oM.Acoustic;
using BH.UI.Alligator.Base;

namespace BH.UI.Alligator.Acoustic
{
    public class CreateZone : GH_Component
    {
        public CreateZone() : base("Create Zone", "Create Zone", "Create BHoM Zone", "Alligator", "Acoustics") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("6A8D2D81-EFA3-4DF3-8A5A-9C76005248F3");
            }
        }
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Acoustic_Alligator.Properties.Resources.BHoM_Acoustics_Zone; }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Zone", "Zone", "Brep Zone", GH_ParamAccess.item);
            pManager.AddParameter(new BHoMGeometryParameter(), "Reciever Points", "RecieverPts", "Point3d Reciever Location", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "BHoM Zone", "BHoM Zone", "BHoM Zone", GH_ParamAccess.item);
            pManager.AddParameter(new BHoMGeometryParameter(), "Sample Points", "Sample Points", "GH Sample Points", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<BHG.Point> points = new List<BHG.Point>();
            Rhino.Geometry.Brep geom = new Rhino.Geometry.Brep();

            if (!DA.GetData(0, ref geom)) { return; }
            if (!DA.GetDataList(1, points)) { return; }

            Zone zone = new Zone(points, geom.GetArea(), geom.GetVolume());
            DA.BH_SetData(0, zone);
            DA.BH_SetDataList(1, points);
        }
    }
}
