using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino;
using Grasshopper;
using Grasshopper.Kernel.Data;
using System.Linq;


namespace SVG_Alligator
{
    public class Make2DComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public Make2DComponent()
          : base("Make2D", "Make2D",
              "Description",
              "Alligator", "SVG")
        {            
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Geometry", "G", "The geometry to select", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Run", "R", "Run", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("2D Curves", "C", "2D Curves", GH_ParamAccess.tree);
            pManager.AddBooleanParameter("Success", "S", "Success", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            Dictionary<string, List<Curve>> crvDictionary = new Dictionary<string, List<Curve>>();
            List<List<Curve>> crvList = new List<List<Curve>>();

            List<Grasshopper.Kernel.Types.IGH_GeometricGoo> geoList = new List<Grasshopper.Kernel.Types.IGH_GeometricGoo>();
            bool runBoolean = false;
            bool successBool = false;
            RhinoDoc doc = RhinoDoc.ActiveDoc;

            if (!DA.GetDataList(0, geoList)) { return; }
            if (!DA.GetData(1, ref runBoolean)) { return; }

            if ((geoList.Count > 0) && (runBoolean))
            {

                List<Guid> GuidList = new List<Guid>();
                doc.Objects.UnselectAll();

                for (int i = 0; i < geoList.Count; i++)
                {
                    doc.Objects.Select(geoList[i].ReferenceID);

                }

                successBool = Rhino.RhinoApp.RunScript(" -Make2D DrawingLayout=CurrentView" + "ShowTangentEdges=Yes "
                + "CreateHiddenLines=No "
                + "MaintainSourceLayers=Yes Enter ", false);

                List<Rhino.DocObjects.RhinoObject> objs = doc.Objects.GetSelectedObjects(false, false).ToList();

                for (int i = 0; i < objs.Count; i++)
                {

                    int index = objs[i].Attributes.LayerIndex;
                    string name = doc.Layers[index].Name;

                    Rhino.Geometry.Curve crv = objs[i].Geometry as Curve;

                    if (!crvDictionary.ContainsKey(name))
                    {

                        crvDictionary.Add(name, new List<Curve>());
                    }

                    crvDictionary[name].Add(crv);
                    doc.Objects.Delete(objs[i], true);

                }

            }

            crvList = crvDictionary.Values.ToList();

            DataTree<Curve> outTree = new DataTree<Curve>();

            for (int i = 0; i < crvList.Count; i++)
            {
                outTree.AddRange(crvList[i], new GH_Path(i));
            }

            DA.SetDataTree(0, outTree);
            DA.SetData(1, successBool);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{291ebfc4-fc9d-4ebc-87f5-1103b31aec6a}"); }
        }
    }
}
