using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CableNetDesignToolkit;
using Grasshopper.Kernel;
using Grasshopper_Engine;

namespace Alligator.FormFinding.CableNetDesign
{
    public class DeformedLengthToAxialForce : GH_Component
    {
        /// <summary>
        /// Calculate axial force in an element based on deformed length
        /// </summary>
        public DeformedLengthToAxialForce()
          : base("DeformedLengthToAxialForce", "DeformedLengthToAxialForce",
              "Converts deformed length to axial force",
              "Alligator", "FormFinding")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Deformed Length", "DeformedLength", "Length of deformed element", GH_ParamAccess.item);
            pManager.AddNumberParameter("Start Length", "StartLength", "Length of element before deformation", GH_ParamAccess.item);
            pManager.AddNumberParameter("Prestress", "Prestress", "Prestress value", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("Young's Modulus", "E", "Young's modulus of element", GH_ParamAccess.item);
            pManager.AddNumberParameter("Cross-section area", "A", "Cross-section area of element", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_DoubleParam("Slack length", "SlackLength", "Slack length corresponding to prestress", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double lDeformed = DataUtils.GetData<double>(DA, 0);
            double lStart = DataUtils.GetData<double>(DA, 1);
            double PS = DataUtils.GetData<double>(DA, 2);
            double E = DataUtils.GetData<double>(DA, 3);
            double A = DataUtils.GetData<double>(DA, 4);

            DA.SetData(0, CableNetDesignToolkit.Utils.Conversions.DeformedLengthToAxialForce(lDeformed, lStart, PS, E, A));
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get { return FormFinding_Alligator.Properties.Resources.conversion; }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{56d6d074-2b6a-4d86-93d2-b549d9103044}"); }
        }
    }
}
