using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.UI.Alligator.Reflection
{
    public class CreateType : GH_Component
    {
        public CreateType() : base("CreateType", "CreateType", "Create a type corresponding to the input name", "Alligator", "Reflection") { }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return null; } }
        public override Guid ComponentGuid { get { return new Guid("02348F61-60DB-4D61-8716-6B234ABD332D"); } }

        public override GH_Exposure Exposure { get { return GH_Exposure.primary; } }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Name of the type", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Type", "Type", "Corresponding type(s)", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string name = ""; DA.GetData(0, ref name);

            Type type = BH.Engine.Reflection.Create.Type(name);
            DA.SetData(0, type);
        }
    }
}
