using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using BH.oM.Base;
using BH.Engine.Reflection;
using System.Linq;
using BH.UI.Alligator.Base;

namespace BH.UI.Alligator.Base
{
    public class ExplodeObject : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ExplodeObject class.
        /// </summary>
        public ExplodeObject()
          : base("ExplodeObject", "ExplodeBH",
              "Get all properties from a BHoM objects",
              "Alligator", "Base")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "BHoM object", "BHoM", "BHoM object", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "Properties", "Properties", "Get object properties", GH_ParamAccess.list);
        }

        protected override void BeforeSolveInstance()
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHoMObject bh = new BHoMObject();
            DA.BH_GetData(0, bh);
            Dictionary<string, object> dict = bh.GetPropertyDictionary();
            List<object> values = dict.Values.ToList();
            DA.SetDataList(0, values);
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
            get { return new Guid("5e53b348-439a-4421-ba08-b18a5df1ff53"); }
        }
    }
}