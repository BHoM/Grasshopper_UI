using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BHG = BH.oM.Geometry;
using BH.UI.Alligator.Base;
using BH.UI.Alligator.Query;

namespace BH.UI.Alligator.Geometry
{
    public class Group : GH_Component
    {
        public Group() : base("BHGroup", "BHGroup", "Create a BHoM Group", "Alligator", "geometry") { }
        public override Guid ComponentGuid { get { return new Guid("8E80E222-5459-402A-BDB7-45FD1E3374B0"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return Alligator.Geometry.Properties.Resources.BHoM_Geometry_Group; } }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new BHoMGeometryParameter(), "curves", "curves", "GH curves", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMGeometryParameter(), "Group", "Group", "BHoM Group", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<BHG.ICurve> curves = new List<BHG.ICurve>();
            curves = DA.BH_GetDataList(0, curves);

            BHG.CompositeGeometry geom = new BHG.CompositeGeometry(curves);
            DA.BH_SetData(0, geom);
        }
    }
}
