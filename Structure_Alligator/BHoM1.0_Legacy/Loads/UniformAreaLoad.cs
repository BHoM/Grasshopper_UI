using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BHL = BH.oM.Structural.Loads;
using BHE = BH.oM.Structural.Elements;
using BH.Engine.Grasshopper.Components;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;
using GHE = BH.Engine.Grasshopper;


namespace BH.UI.Alligator.Structural.Loads
{
    public class UniformAreaLoad : BHoMBaseComponent<BHL.Load<BHE.IAreaElement>>
    {
        public UniformAreaLoad(): base("Create Uniform Area Load", "Create Area Load", "Create a area load", "Structure", "Loads") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("7C6535C7-B8FF-44A0-BB61-52DBBDBAD1B1");
            }
        }
        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Structural.Properties.Resources.BHoM_AreaLoad; }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddVectorParameter("Pressure","Pressure","Pressure",GH_ParamAccess.item);
            pManager.AddBooleanParameter("Projected", "Projected", "Projected", GH_ParamAccess.item);
            pManager.AddGenericParameter("Loadcase", "Loadcase", "Loadcase", GH_ParamAccess.item);
            pManager.AddGenericParameter("Object", "Object", "Object", GH_ParamAccess.list);
            pManager.AddTextParameter("Name", "Name", "Name", GH_ParamAccess.item);
            pManager.AddGenericParameter("CustomData", "CustomData", "CustomData", GH_ParamAccess.item);

            pManager[1].Optional = true;
            pManager[3].Optional = true;
            pManager[5].Optional = true;

            AppendEnumOptions("Axis", typeof(BHL.LoadAxis));
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AreaUniformalyDistributedLoad", "AreaUniformalyDistributedLoad", "AreaUniformalyDistributedLoad", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Vector3d pressure = GHE.DataUtils.GetData<Vector3d>(DA, 0);
            bool projected = GHE.DataUtils.GetData<bool>(DA, 1);
            BHL.Loadcase loadcase = GHE.DataUtils.GetData<BHL.Loadcase>(DA, 2);
            List<BHE.IAreaElement> areaelements = GHE.DataUtils.GetDataList<BHE.IAreaElement>(DA, 3);
            string name = GHE.DataUtils.GetData<string>(DA, 4);

            BHL.AreaUniformalyDistributedLoad areaLoad = new BHL.AreaUniformalyDistributedLoad(loadcase, pressure.X, pressure.Y, pressure.Z);
            areaLoad.Projected = projected;
            areaLoad.Loadcase = loadcase;
            List<BHE.IAreaElement> objects = new List<BHE.IAreaElement>(areaelements);

            switch ((BHL.LoadAxis)m_SelectedOption[0])
            {
                case BHL.LoadAxis.Local:
                    areaLoad.Axis = BHL.LoadAxis.Local;
                    break;
                case BHL.LoadAxis.Global:
                    areaLoad.Axis = BHL.LoadAxis.Global;
                    break;
                default: areaLoad.Axis = BHL.LoadAxis.Global;

                    break;
            }

            areaLoad.Objects = objects;
            areaLoad.Name = name;            

            DA.SetData(0, areaLoad);
        }
    }
}
