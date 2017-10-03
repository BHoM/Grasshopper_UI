using System;
using System.Collections.Generic;
using BHG = BH.oM.Geometry;
using BH.oM.Acoustic;
using Grasshopper.Kernel;
using BH.UI.Alligator.Base;
using BH.UI.Alligator.Query;

namespace BH.UI.Alligator.Acoustic
{
    public class CreatePanel : GH_Component
    {
        public CreatePanel() : base("CreatePanel", "Panel", "Creates BHoM Acoustic Panel", "Alligator", "Acoustics") { }
        protected override System.Drawing.Bitmap Icon { get { return null; } }
        public override Guid ComponentGuid { get { return new Guid("{86ea8c0c-5070-4a31-a653-7fbfaa7ed667}"); } }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new BHoMGeometryParameter(), "Mesh", "M", "Triangular or Quadrangular Mesh. Do not input joined mesh, but single faces.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Absorption", "a", "Absorbtion coefficient between 0 and 1", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "Panel", "Panel", "BHoM Acoustic Panel", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Panel panels = new Panel();
            BHG.Mesh mesh = new BHG.Mesh();
            List<double> r = new List<double>() { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 };
            mesh = DA.BH_GetData(0, mesh);

            Panel panel = new Panel(mesh, r);
            DA.BH_SetData(0, panels);
        }
    }
}
