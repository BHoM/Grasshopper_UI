using BHoM.Structural;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

namespace Alligator.Components
{
    public class CreateBar : BHoMBaseComponent<Bar>
    {
        public CreateBar() : base("Create Bar", "CreateBar", "Create a BH bar object", "Alligator", "Structural") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("5FE0E2C4-5E50-410F-BBC7-C255FD0BD2B3");
            }
        }
    }

    public class CreateNode : BHoMBaseComponent<Node>
    {
        public CreateNode() : base("Create Node", "CreateNode", "Create a BH Node object", "Alligator", "Structural") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("5FE0E2C4-5E50-410F-BBC7-C255FD1BD2B3");
            }
        }
    }

    public class CreateDOF : BHoMBaseComponent<DOF>
    {
        public CreateDOF() : base("Create DOF", "CreateDOF", "Create a BH DOF object", "Alligator", "Structural") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("5FE0E2C4-5E10-410F-BBC7-C255FD1BD2B3");
            }
        }
    }

    public class CreateConstraint : BHoMBaseComponent<Constraint>
    {
        public CreateConstraint() : base("Create Constraint", "CreateConstraint", "Create a BH Constraint object", "Alligator", "Structural") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("5FE0E2C4-5E10-410F-BBC7-C255FE1BD2B3");
            }
        }
    }

    public class TestCurve : GH_Component
    {
        public TestCurve() : base("G", "G", "Test", "Alligator", "Structural") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("5FF0E2C4-5E50-410F-BBC7-C255FD1BD2B3");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("G", "G", "G", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("G", "G", "G", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHoM.Geometry.Arc arc = new BHoM.Geometry.Arc(new BHoM.Geometry.Point(0, 0, 0), new BHoM.Geometry.Point(1, 0, 0), new BHoM.Geometry.Point(0.5, 0.1, 0));
            Rhino.Geometry.Curve c = GeometryUtils.Convert(arc);
            DA.SetData(0, c);

        }
    }
}
