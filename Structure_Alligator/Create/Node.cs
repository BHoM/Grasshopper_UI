using BH.oM.Structural;
using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using Rhino.Geometry;
using Grasshopper.Kernel.Data;
using BH.UI.Alligator.Base;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Properties;
using BHG = BH.oM.Geometry;
using BH.UI.Alligator;

namespace BH.UI.Alligator.Structural.Elements
{
    public class CreateNode : GH_Component
    {
        public CreateNode() : base("Create Node", "Node", "Creates BHoM Structural Node", "Structure", "Elements") { }
        public override GH_Exposure Exposure { get { return GH_Exposure.secondary; } }
        public override Guid ComponentGuid { get { return new Guid("c811c998-a60f-4015-8bed-a79d22467a20"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return Properties.Resources.BHoM_Node; } }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new BHoMGeometryParameter(), "Point", "Point", "BHoM Point", GH_ParamAccess.item);
            pManager.AddParameter(new BHoMObjectParameter(), "Constraint", "Constraint", "BHoM structural contraint", GH_ParamAccess.item);
            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "Nodes", "Nodes", "BHoM nodes to export", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHG.Point point = new BHG.Point();
            NodeConstraint constraint = new NodeConstraint();

            DA.GetData(0, ref point);
            DA.GetData(1, ref constraint);

            Node node = new Node();
            node.Point = point;
            node.Constraint = constraint;
            
            DA.SetData(0, node);
        }
    }
}
