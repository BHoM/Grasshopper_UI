using System;
using System.IO;
using System.Collections.Generic;
using Grasshopper.Kernel;
using XML_Adapter.gbXML;
using XML_Adapter;
using BHoM;

namespace XML_Alligator
{
    public class ReadXML : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public ReadXML()
          : base("ReadXML", "ReadXML",
              "Read an XML file",
              "Alligator", "XML")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("File Path", "P", "File path to read the file from.", GH_ParamAccess.item);
            pManager.AddTextParameter("Name", "N", "Name of the file.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Run", "Run", "Read the file,", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("XML object", "XML", "XML object thats read.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string filePath = ""; DA.GetData<string>(0, ref filePath);
            string Name = ""; DA.GetData<string>(1, ref Name);
            Path.Combine(filePath, Name);
            bool active = false; DA.GetData<bool>(2, ref active);

            if (!active) return;

            gbXML gbx = XMLReader.Load(filePath, Name);
            DA.SetData(0, gbx);
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
            get { return new Guid("{84c19d3f-2566-4ddf-b459-b49ba16b4763}"); }
        }
    }
}