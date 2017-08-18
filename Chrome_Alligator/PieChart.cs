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
            pManager.AddTextParameter("Radius", "rDims", "First pie value argument", GH_ParamAccess.list);
            pManager.AddTextParameter("Colour", "cDim", "Property that conveys colors criteria", GH_ParamAccess.item, "red");
            pManager.AddIntegerParameter("Legend Type", "Legend", "[0] Lateral legend, [1] for radial legend", GH_ParamAccess.item, 0);
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
            string cDim = "";
            List<string> rDims = new List<string>();
            int legend = 0;

            DA.GetDataList<string>(1, rDims);

            DA.GetData<string>(0, ref parent);
            DA.GetData<string>(2, ref cDim);
            DA.GetData<int>(3, ref legend);

            List<string> config = new List<string>();

            config.Add("type: pieChart");
            config.Add("parent: " + parent);

            string s = "rDims: [";
            for (int i = 0; i < rDims.Count; i++)
            {
                /*
                if (rDims[i].Length > 0)
                {
                    config.Add("rDim" + i + ": " + rDims[i]);
                }
                */
                s += rDims[i];
                s += ",";
            }

            config.Add(s.Trim(',') + ']');

            if (cDim.Length > 0 && (cDim.StartsWith("{") || cDim.IndexOf(',') == -1))
                config.Add("cDim: " + cDim);
            config.Add("legendType: " + legend);

            DA.SetDataList(0, config);
        }

    }
}