using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using GHE = Grasshopper_Engine;
using Alligator.Components;
using BHE = BHoM.Environmental.Elements;
using BHG = BHoM.Geometry;
using System.Windows.Forms;
using R = Rhino.Geometry;
using Grasshopper;
using GHKT = Grasshopper.Kernel.Types;
using Grasshopper_Engine.Components;
using ASP = Alligator.Structural.Properties;

namespace Alligator.Environmental.Elements
{
    public class CreateSpace : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public CreateSpace() : base("CreateSpace", "CreateSpace", "Create a Space", "Alligator", "Environmental")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Boundary Lines", "BL", "Polyline defining the space", GH_ParamAccess.list);
            pManager.AddGenericParameter("Walls", "Walls", "Walls that define the space", GH_ParamAccess.list);
            pManager.AddTextParameter("Name", "N", "Name of the element", GH_ParamAccess.item);
            pManager.AddGenericParameter("Custom Data", "CD", "Custom data to add to the space", GH_ParamAccess.item);

            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Space", "S", "The Created Space", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            BHE.Space space = new BHE.Space();

            List<BHG.Line> Lines = new List<BHG.Line>();
            DA.GetDataList<BHG.Line>(0, Lines);
            space.Lines = Lines;

            List<BHE.Wall> walls = new List<BHE.Wall>();
            DA.GetDataList<BHE.Wall>(1, walls);
            space.Walls = walls;

            string name = "";

            if (DA.GetData(2, ref name))
            {
                space.Name = name;
            }

            Dictionary<string, object> customData = GHE.DataUtils.GetData<Dictionary<string, object>>(DA, 3);

            if (customData != null)
            {
                foreach (KeyValuePair<string, object> item in customData)
                {
                    space.CustomData.Add(item.Key, item.Value);
                }
            }

            DA.SetData(0, space);
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
            get { return new Guid("{89be84e2-2e50-48b7-bb28-8d11e9e0ff05}"); }
        }
    }
}