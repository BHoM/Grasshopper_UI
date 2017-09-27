using System;
using System.Collections.Generic;
using BHG = BHoM.Geometry;
using BHA = BHoM.Acoustic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Acoustic_Alligator
{
    public class CreatePanel : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreatePanel class.
        /// </summary>
        public CreatePanel()
          : base("CreatePanel", "Panel",
              "Creates BHoM Acoustic Panel",
              "Alligator", "Acoustics")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Mesh", "M", "Triangular or Quadrangular Mesh. Do not input joined mesh, but single faces.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Absorption", "a", "Absorbtion coefficient between 0 and 1", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Panel", "Panel", "BHoM Acoustic Panel", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<BHA.Panel> panels = new List<BHA.Panel>();

            List<BHG.Mesh> mesh = new List<BHG.Mesh>();
            List<List<double>> r = new List<List<double>>();

            if (!DA.GetDataList(0, mesh)) { return; }

            for (int i = 0; i < mesh.Count; i++)
            {
                if (r.Count < mesh.Count)
                {
                    r.Add( new List<double> { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0} );
                }
                BHA.Panel panel = new BHA.Panel(mesh[i],r[i]);
                panels.Add(panel);

                DA.SetDataList(0, panels);
            }
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{86ea8c0c-5070-4a31-a653-7fbfaa7ed667}"); }
        }
    }
}