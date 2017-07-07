using System;
using System.IO;
using System.Collections.Generic;
using Grasshopper.Kernel;
using XML_Adapter.gbXML;
using XML_Adapter;
using BHoM;

namespace XML_Alligator
{
    public class WriteXML : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public WriteXML()
          : base("WriteXML", "WriteXML",
              "Writes an XML file",
              "Alligator", "XML")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("XML object", "XML", "XML object to write.", GH_ParamAccess.item);
            pManager.AddTextParameter("File Path", "P", "File path to write the file to.", GH_ParamAccess.item);
            pManager.AddTextParameter("Name", "N", "Name of the file.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Run", "Run", "Write the file,", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Sucess", "Sucess", "Return from Writer.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            gbXML gbXML = new gbXML(); DA.GetData<gbXML>(0, ref gbXML);
            string filePath = ""; DA.GetData<string>(1, ref filePath);
            string Name = ""; DA.GetData<string>(2, ref Name);
            Path.Combine(filePath, Name);
            bool active = false; DA.GetData<bool>(3, ref active);

            if (!active) return;

            string success = XMLWriter.Save(filePath, Name, gbXML);
            DA.SetData(0, success);
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
            get { return new Guid("{b14c5a10-2a8f-4ae4-adb2-a7ef8d03d311}"); }
        }
    }
}