using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHoM.Structural;
using Grasshopper.Kernel;

namespace Alligator.Structural.Elements
{
    public class CreateDOF: BHoMBaseComponent<DOF>
    {
        public CreateDOF() : base("Create DOF", "CreateDOF", "Create a BH Degree of Freedom Object", "Alligator", "Structural") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("6f104da4-e442-48a4-b9e7-deaebceab865");
            }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.constraint; }
        }
    }
}
