using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using RG = Rhino.Geometry;
using BHG = BH.oM.Geometry;

namespace BH.UI.Alligator.Geometry
{
    public class Line : GH_Component
    {
        public Line() : base("BHLine", "BHLine", "Create a BHoM line", "Alligator", "geometry") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("9414007A-FDBA-4A2A-8534-A5D1EAECB350");
            }
        }
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Geometry.Properties.Resources.BHoM_Line; }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("startPt", "startPt", "GH start point", GH_ParamAccess.item);
            pManager.AddPointParameter("endPt", "endPt", "GH end point", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("line", "line", "BHoM line", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            RG.Point3d start = new RG.Point3d();
            RG.Point3d end = new RG.Point3d();

            DA.GetData<RG.Point3d>(0, ref start);
            DA.GetData<RG.Point3d>(1, ref end);

            BHG.Line line = new BHG.Line(new BHG.Point(start.X, start.Y, start.Z), new BHG.Point(end.X, end.Y, end.Z));

            DA.SetData(0, line);
        }
    }
}
