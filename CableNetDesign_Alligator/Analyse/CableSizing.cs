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
    public class CableSizing : GH_Component
    {
        /// <summary>
        /// Component for sizing of cable net cables.
        /// </summary>
        public CableSizing()
          : base("CableSizing", "CableSizing",
              "Cable Sizing based on axial force",
              "Alligator", "FormFinding")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Axial Force", "AxialForce", "Axial force on cable", GH_ParamAccess.item);
            pManager.AddNumberParameter("Safety Factor", "SafetyFactor", "Safety factor for sizing", GH_ParamAccess.item, 1.0);
            pManager.AddNumberParameter("Live Load Factor", "LiveLoadFactor", "Safety factor for sizing", GH_ParamAccess.item, 1.0);
            pManager.AddIntegerParameter("Number of cables", "NoOfCables", "Number of cables in cable element", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("Utilisation limit", "UtilisationLimit", "Utilisation limit for sizing", GH_ParamAccess.item, 1.0);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_DoubleParam("Diameter", "Diameter", "Cable diameter", GH_ParamAccess.item);
            pManager.Register_DoubleParam("Utilisation", "Utilisation", "Cable utilisation", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            CableSizingElement cable = new CableSizingElement();
            cable.AxialForce = DataUtils.GetData<double>(DA, 0);
            cable.SafetyFactor = DataUtils.GetData<double>(DA, 1);
            cable.LiveLoadFactor = DataUtils.GetData<double>(DA, 2);
            cable.NoOfCables = DataUtils.GetData<int>(DA, 3);
            cable.UtilisationLimit = DataUtils.GetData<double>(DA, 4);

            DA.SetData(0, cable.Diameter);
            DA.SetData(1, cable.Utilisation);

        }


        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{0260e569-9089-40fb-ab1a-526f7d2bc47a}"); }
        }
    }
}
