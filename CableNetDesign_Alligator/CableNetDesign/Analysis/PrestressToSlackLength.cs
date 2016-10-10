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
    public class PrestressToSlackLength : GH_Component
    {
        /// <summary>
        /// Calculate slack length corresponding to a prestress value
        /// </summary>
        public PrestressToSlackLength()
          : base("PrestressToSlackLength", "PrestressToSlackLength",
              "Converts prestress to slack length for an element",
              "Alligator", "FormFinding")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Start Length", "StartLength", "Length of element", GH_ParamAccess.item);
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
            double lStart = DataUtils.GetData<double>(DA, 0);
            double PS = DataUtils.GetData<double>(DA, 1);
            double E = DataUtils.GetData<double>(DA, 2);
            double A = DataUtils.GetData<double>(DA, 3);

            DA.SetData(0, CableNetDesignToolkit.Utils.Conversions.PSToSlackLength(lStart, PS, E, A));
        }


        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{4913cd14-2e76-47d8-9eec-3dc9e5bbf978}"); }
        }
    }
}
