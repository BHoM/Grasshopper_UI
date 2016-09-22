using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BHE = BHoM.Structural.Elements;
using Grasshopper_Engine;

namespace Alligator.Structural.Elements
{
    public class CreateRigidLink : GH_Component
    {
        public CreateRigidLink() : base("Create Rigid Link", "RigLin", "Create a rigid link", "Structure", "Elements") { }


        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("72FE4955-941E-4CD8-AC51-B5267B4F9F4D");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Master", "M", "Master node", GH_ParamAccess.item);
            pManager.AddGenericParameter("Slaves", "S", "Slave nodes", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Rigid Link", "RL", "Rigid link", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHE.Node master;
            List<BHE.Node> slaves;

            if (!DataUtils.GetNodeFromPointOrNode(DA, 0, out master)) { return; }
            if (!DataUtils.GetNodeListFromPointOrNodes(DA, 1, out slaves)) { return; }

            BHE.RigidLink link = new BHoM.Structural.Elements.RigidLink(master, slaves);

            DA.SetData(0, link);
        }
    }
}
