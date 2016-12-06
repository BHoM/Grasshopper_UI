﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BHL = BHoM.Structural.Loads;
using BHE = BHoM.Structural.Elements;
using Grasshopper_Engine.Components;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;
using GHE = Grasshopper_Engine;


namespace Alligator.Structural.Loads
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

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddVectorParameter("Pressure","Pressure","Pressure",GH_ParamAccess.item);
            pManager.AddBooleanParameter("Projected", "Projected", "Projected", GH_ParamAccess.item);
            pManager.AddGenericParameter("Loadcase", "Loadcase", "Loadcase", GH_ParamAccess.item);
            pManager.AddGenericParameter("Object", "Object", "Object", GH_ParamAccess.list);
            pManager.AddTextParameter("Name", "Name", "Name", GH_ParamAccess.item);
            pManager.AddGenericParameter("CustomData", "CustomData", "CustomData", GH_ParamAccess.item);

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

            BHL.AreaUniformalyDistributedLoad areaLoad = new BHL.AreaUniformalyDistributedLoad(pressure.X, pressure.Y, pressure.Z);
            areaLoad.Projected = projected;
            areaLoad.Loadcase = loadcase;
            BHoM.Base.Group<BHE.IAreaElement> objects = new BHoM.Base.Group<BHE.IAreaElement>(areaelements);

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
