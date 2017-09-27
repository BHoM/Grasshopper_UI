using System;
using System.Collections.Generic;

using Grasshopper.Kernel;


namespace Chrome_Alligator
{
    public class StreamChart : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the StreamChart class.
        /// </summary>
        public StreamChart()
          : base("StreamChart", "StreamChart",
              "Description",
              "Alligator", "Chrome")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("parent", "parent", "Define the parent to this component", GH_ParamAccess.item, "body");
            pManager.AddTextParameter("xDim", "xDim", "TimeDate, stream dimension definition for the x axis.", GH_ParamAccess.item);
            pManager.AddTextParameter("yDims", "yDims", "Streams, y dimensions definition for the y axis.", GH_ParamAccess.list);
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
            string parent = "", xDim = "";
            List<string> yDims = new List<string>();

            DA.GetData<string>(0, ref parent);
            DA.GetData<string>(1, ref xDim);
            DA.GetDataList<string>(2, yDims);

            List<string> config = new List<string>();

            config.Add("type: streamChart");
            config.Add("parent: " + parent);
            config.Add("xDim: " + xDim);

            string s = "yDims: [";
            for (int i = 0; i < yDims.Count; i++)
            {
                s += yDims[i];
                s += ",";
            }

            config.Add(s.Trim(',') + ']');

            DA.SetDataList(0, config);
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
            get { return new Guid("28ab88f8-bc76-4e63-b663-288430008954"); }
        }
    }
}