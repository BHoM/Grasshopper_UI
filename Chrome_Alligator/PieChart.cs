using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using CA = Chrome_Adapter;



namespace Chrome_Alligator
{
    public class PieChart : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PieChart class.
        /// </summary>
        public PieChart() : base("PieChart", "PieChart", "Creates a Chrome-pushable Pie Chart object", "Alligator", "Chrome")
        {
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
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
            get { return new Guid("44191e8d-e81b-4e2b-8562-9557445c9503"); }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("parent", "parent", "Define the parent to this component", GH_ParamAccess.item, "body");
            pManager.AddTextParameter("Radius", "rDim1", "First pie value argument", GH_ParamAccess.item,"");
            pManager.AddTextParameter("Radius", "rDim2", "Second pie value argument for nested pies", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Radius", "rDim3", "Third pie value argument for nested pies", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Colour", "cDim", "Property that conveys colors criteria", GH_ParamAccess.item, "red");
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("config", "config", "config", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string parent = "";
            string rDim1 = "", rDim2 = "", rDim3 = "";
            string cDim = "";
            DA.GetData<string>(0, ref parent);
            DA.GetData<string>(1, ref rDim1);
            DA.GetData<string>(2, ref rDim2);
            DA.GetData<string>(3, ref rDim3);
            DA.GetData<string>(4, ref cDim);

            List<string> config = new List<string>();

            config.Add("type: pieChart");
            config.Add("parent: " + parent);

            if (rDim1.Length > 0) { config.Add("rDim1: " + rDim1); }
            if (rDim2.Length > 0) { config.Add("rDim2: " + rDim2); }
            if (rDim3.Length > 0) { config.Add("rDim3: " + rDim3); }
            if (cDim.Length > 0 && (cDim.StartsWith("{") || cDim.IndexOf(',') == -1))
                config.Add("cDim: " + cDim);

            DA.SetDataList(0, config);
        }

    }
}