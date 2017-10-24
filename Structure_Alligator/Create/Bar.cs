using BH.oM.Structural;
using BH.Engine.Geometry;
using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Properties;
using BHG = BH.oM.Geometry;
using System.Windows.Forms;
using Grasshopper.Kernel.Types;
using BH.Engine.Structure;
using BH.UI.Alligator;
using BH.UI.Alligator.Base;
using BH.oM.Base;
using BH.oM.Materials;
using BH.Engine.Base;

namespace BH.UI.Alligator.Structure
{

    public class CreateBar : GH_Component
    {
        public CreateBar() : base("Create Bar", "CreateBar", "Create a BH Bar object", "Structure", "Elements") { }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Structural.Properties.Resources.BHoM_Bar; }
        }


        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("c5bcd137-c3f3-4e8c-876a-199194c8389c");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("NodeA/Line", "A/Line", "Start node of the bar", GH_ParamAccess.item);
            pManager.AddGenericParameter("NodeB", "B", "End node of the bar", GH_ParamAccess.item);
            pManager.AddGenericParameter("Section Properties", "Properties", "The section property of the bar", GH_ParamAccess.item);
            pManager.AddGenericParameter("Material", "Material", "Material of the bar", GH_ParamAccess.item);
            pManager.AddNumberParameter("Orientation angle", "Angle", "Orientationangle or vector", GH_ParamAccess.item);
            pManager.AddTextParameter("Name", "Name", "Name of the element", GH_ParamAccess.item);
            pManager.AddGenericParameter("Attributes", "Attributes", "Attributes of the bar", GH_ParamAccess.item);
            pManager.AddGenericParameter("Custom Data", "Custom", "Custom data to add to the bar", GH_ParamAccess.item);

            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
            pManager[4].Optional = true;
            pManager[5].Optional = true;
            pManager[6].Optional = true;
            pManager[7].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Bar", "B", "The created bar", GH_ParamAccess.item);
            pManager.AddParameter(new BHoMGeometryParameter(), "Centreline", "CL", "The created bar geometry", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            object lineOrPoint = new BHG.Line();
            //BHoMObject elementA = new BHoMObject(); // Can be a Point or a Node
            object elementB = new object(); // Can be a Point or a Node
            SectionProperty secProp = default(SectionProperty);
            Material material = new Material();
            double angle = 0;
            string name = "";
            object attributes = new object();
            Dictionary<string, object> customData = new Dictionary<string, object>();

            Bar bar = new Bar();

            if (DA.GetData(1, ref elementB))
            {
                DA.GetData(0, ref lineOrPoint);
                //bar = Engine.Structure.Create.IBar(lineOrPoint.UnwrapObject(), elementB.UnwrapObject());
            }
            else if (DA.GetData(0, ref lineOrPoint))
            {
                //bar = Engine.Structure.Create.IBar(lineOrPoint.UnwrapObject() as BHG.ICurve);
            }
            else { return; }
            if (DA.GetData(2, ref secProp))
            {
                DA.GetData(3, ref material);
                secProp.Material = material;
                bar.SectionProperty = secProp;
            }
            if (DA.GetData(4, ref angle)) { bar.OrientationAngle = angle; }
            if (DA.GetData(5, ref name)) { bar.Name = name; }
            if (DA.GetData(6, ref attributes)) { }
            if (DA.GetData(7, ref customData)) { bar.CustomData = customData; }

            DA.SetData(0, bar);
            DA.SetData(1, bar.GetGeometry());
        }
    }
}

