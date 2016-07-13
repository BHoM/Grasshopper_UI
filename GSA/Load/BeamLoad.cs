using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BHoM.Structural;
using BHoM.Structural.Loads;

namespace Alligator.GSA.Load
{
    public class SetBeamPrestress: GH_Component
    {
        public SetBeamPrestress() : base("Set Beam Prestress", "BeamPS", "Creates beam prestress loads", "GSA", "Loads") { }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "App", "Application to add prestress loads to", GH_ParamAccess.item);
            pManager.AddGenericParameter("Bars", "Bars", "Bars to add prestress to", GH_ParamAccess.list);
            pManager.AddNumberParameter("Values", "Values", "Prestress values", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Loadcase ID", "Loadcase", "Loadcase ID for prestress loadcase", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Execute", "R", "Add prestress", GH_ParamAccess.item);

            pManager[3].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("Ids", "Ids", "Bar Numbers", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (Utils.Run(DA, 4))
            {
                IStructuralAdapter app = Utils.GetGenericData<IStructuralAdapter>(DA, 0);
                if (app != null)
                {
                    List<Bar> bars = Utils.GetGenericDataList<Bar>(DA, 1);
                    List<double> values = Utils.GetDataList<double>(DA, 2);
                    int ID = Utils.GetData<int>(DA, 3);
                    List<string> ids = null;

                    List<ILoad> psLoads = new List<ILoad>();

                    for (int i = 0; i < bars.Count; i++)
                    {
                        BarPrestressLoad psLoad = new BarPrestressLoad();
                        psLoad.Loadcase = new Loadcase(ID, "", LoadNature.Other);
                        psLoad.Objects = new List<Bar>() { bars[i] };
                        psLoad.PrestressValue = values[i];
                        psLoads.Add(psLoad);
                    }

                    app.SetLoads(psLoads);

                    DA.SetDataList(0, ids);
                }
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("5392740d-9cb2-4f87-b8b1-b5b255d2a327"); }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return GSA.Properties.Resources.barPrestress; }
        }
    }
}
