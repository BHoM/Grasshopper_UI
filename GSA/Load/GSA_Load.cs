using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BHoM.Structural.Loads;
using BHoM.Structural;
using GSAToolkit;

namespace Alligator.GSA.Load
{
    public class CreateGravityLoad : GH_Component
    {
        public CreateGravityLoad() : base("CreateGravityLoad", "CreateGravityLoad", "CreateGravityLoad", "GSA", "Elements") { }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Loadcase", "Loadcase", "Loadcase", GH_ParamAccess.item);
            pManager.AddGenericParameter("Objects", "Objects", "BHoM objects to apply load to", GH_ParamAccess.list);

            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Load", "Load", "Load", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            Loadcase loadcase = Utils.GetGenericData<Loadcase>(DA, 0);
            List<Bar> bars = Utils.GetGenericDataList<Bar>(DA, 1);

            Loadcase loadcase2 = new Loadcase(5, "", LoadNature.Dead);

            BarGravityLoad gLoad = new BarGravityLoad();
            gLoad.Loadcase = loadcase;
            gLoad.Objects = bars;
            //PropertyIO.SetSectionProperty(app, secProp);

            DA.SetData(0, gLoad);

        }

        public override Guid ComponentGuid
        {
            get { return new Guid("f85ca66a-32c0-4373-97b0-cd136c65e2c3"); }
        }

        /// <summary> Icon(24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return GSA.Properties.Resources.bar; }
        }

    }
}
