using BH.oM.Structural;
using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using Rhino.Geometry;
using Grasshopper.Kernel.Data;
using BH.UI.Alligator.Base;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Properties;
using BHG = BH.oM.Geometry;
using BH.UI.Alligator.Query;

namespace BH.UI.Alligator.Structural.Create
{
    public class CreateConstraint : GH_Component
    {
        public CreateConstraint() : base("Create Constraint", "NodeConstraint", "Creates BHoM Structural NodeConstraint
        Free = 0,
        Fixed = 1,
        FixedNegative = 2,
        FixedPositive = 3,
        Spring = 4,
        SpringNegative = 5,
        SpringPositive = 6,
        SpringRelative = 7,
        SpringRelativeNegative = 8,
        SpringRelativePositive = 9,
        NonLinear = 10,
        Friction = 11,
        Damped = 12,
        Gap = 13", "Structure", "Elements") { }
        public override GH_Exposure Exposure { get { return GH_Exposure.secondary; } }
        public override Guid ComponentGuid { get { return new Guid("7f3ead00-2f58-45c4-9b42-d10cf660f208"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return Properties.Resources.BHoM_Node; } }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("TranslationalX", "RX", "External Translational contraint on X Axis. ", GH_ParamAccess.item);
            pManager.AddBooleanParameter("TranslationalY", "RX", "External Rotational contraint on Y Axis", GH_ParamAccess.item);
            pManager.AddBooleanParameter("TranslationalZ", "RX", "External Rotational contraint on Z Axis", GH_ParamAccess.item);
            pManager.AddBooleanParameter("RotationalX", "RX", "External Rotational contraint around X Axis", GH_ParamAccess.item);
            pManager.AddBooleanParameter("RotationalY", "RX", "External Rotational contraint around Y Axis", GH_ParamAccess.item);
            pManager.AddBooleanParameter("RotationalZ", "RX", "External Rotational contraint around Z Axis", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
            pManager[4].Optional = true;
            pManager[5].Optional = true;
            pManager[6].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "Constraint", "Constraint", "BHoM structural contraint", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            NodeConstraint constr = new NodeConstraint();
            int tx = 0, ty = 0, tz = 0;
            int rx = 0, ry = 0, rz = 0;
            DA.GetData(0, ref tx);
            DA.GetData(1, ref ty);
            DA.GetData(2, ref tz);
            DA.GetData(3, ref rx);
            DA.GetData(4, ref ry);
            DA.GetData(5, ref rz);

            constr.UX = (DOFType)tx;
            constr.UX = (DOFType)tx;
            constr.UX = (DOFType)tx;
            constr.RX = (DOFType)rx;
            constr.RY = (DOFType)ry;
            constr.RZ = (DOFType)rz;

            DA.BH_SetData(0, constr);
        }
    }
}