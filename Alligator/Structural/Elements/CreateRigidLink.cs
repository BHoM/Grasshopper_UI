using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BHE = BHoM.Structural.Elements;
using BHP = BHoM.Structural.Properties;
using Grasshopper_Engine;

namespace Alligator.Structural.Elements
{
    public class CreateRigidLink : GH_Component
    {
        public CreateRigidLink() : base("Create Rigid Link", "RigLin", "Create a rigid link", "Structure", "Elements") { }
        
        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_RigidLink; }
        }

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
            pManager.AddGenericParameter("Link Constraint", "C", "Constriant to use for the rigid link", GH_ParamAccess.item);

            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Rigid Link", "RL", "Rigid link", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHE.Node master;
            List<BHE.Node> slaves;
            BHP.LinkConstraint constriant;

            if (!DataUtils.GetNodeFromPointOrNode(DA, 0, out master)) { return; }
            if (!DataUtils.GetNodeListFromPointOrNodes(DA, 1, out slaves)) { return; }

            constriant = DataUtils.GetGenericData<BHP.LinkConstraint>(DA, 2);

            if (constriant == null)
                constriant = BHP.LinkConstraint.Fixed;


            BHE.RigidLink link = new BHoM.Structural.Elements.RigidLink(master, slaves, constriant);

            DA.SetData(0, link);
        }
    }
}
