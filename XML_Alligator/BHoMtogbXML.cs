using System;
using System.IO;
using System.Collections.Generic;
using Grasshopper.Kernel;
using XML_Adapter.gbXML;
using BHoM;

namespace XML_Alligator
{
    public class BHoMtogbXML : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public BHoMtogbXML()
          : base("BHoM to gbXML", "BHoM to gbXML",
              "Converts BHoMBojects to gbXML format",
              "Alligator", "XML")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("BHoM Objects", "BO", "Objects to convert to gbXML.", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("gbXML", "gbXML", "Returns the gbXML object.", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            //List<BHoM.Base.BHoMObject> bhomObjects = Grasshopper_Engine.DataUtils.GetGenericDataList<BHoM.Base.BHoMObject>(DA, 0);
            List<BHoM.Base.BHoMObject> OUT = new List<BHoM.Base.BHoMObject>(); DA.GetDataList<BHoM.Base.BHoMObject>(0, OUT);
            gbXML gbXML = gbXMLSerializer.Serialize(OUT);
            DA.SetData(0, gbXML);
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
            get { return new Guid("{98d5a00b-4453-44e2-9495-73b4056d4ac7}"); }
        }
    }
}