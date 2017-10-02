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
    public class CableTonnage : GH_Component
    {
        /// <summary>
        /// Component for sizing of cable net cables.
        /// </summary>
        public CableTonnage()
          : base("CableTonnage", "CableTonnage",
              "Cable Tonnage",
              "Alligator", "FormFinding")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Cable Diameter", "Diameter", "Cable Diameter", GH_ParamAccess.item);
            pManager.AddNumberParameter("Cable Length", "Length", "Cable Length", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Number of cables", "NoOfCables", "Number of cables in cable element", GH_ParamAccess.item, 1);
            pManager.AddBooleanParameter("Adjustment", "Adjustment", "Adjustment on cables", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("Couplers", "Couplers", "Couplers on cables", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_DoubleParam("Cable mass", "Cable", "Cable mass", GH_ParamAccess.item);
            pManager.Register_DoubleParam("Fittings mass", "Fittings", "Fittings mass", GH_ParamAccess.item);
            pManager.Register_DoubleParam("Coupler mass", "Couplers", "Couplers mass", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            CableSizingElement cable = new CableSizingElement();
            cable.Diameter = DataUtils.GetData<double>(DA, 0);
            cable.Length = DataUtils.GetData<double>(DA, 1);
            cable.NoOfCables = DataUtils.GetData<int>(DA, 2);
            cable.Adjustment = DataUtils.GetData<bool>(DA, 3);
            cable.Couplers = DataUtils.GetData<bool>(DA, 4);

            DA.SetData(0, cable.CableMass);
            DA.SetData(1, cable.FittingMass);
            DA.SetData(2, cable.CouplerMass);

        }



        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{e23df1d6-b21d-484a-baf5-a4e3d2aa97db}"); }
        }
    }
}
