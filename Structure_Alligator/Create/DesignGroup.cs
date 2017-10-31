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

namespace BH.UI.Alligator.Structural.Design
{
    public class CreateDesignGroup : GH_Component
    {
        public CreateDesignGroup() : base("Create Design Group", "DesignGroup", "Creates BHoM Structural DesignGroup", "Structure", "Design") { }
        public override GH_Exposure Exposure { get { return GH_Exposure.secondary; } }
        public override Guid ComponentGuid { get { return new Guid("c811c998-a60f-4015-8bed-a79d22467a21"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return Properties.Resources.BHoM_Node; } }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "N", "Design goup name", GH_ParamAccess.item,"");
            pManager.AddIntegerParameter("Number", "I", "Design group number", GH_ParamAccess.item,0);
            pManager.AddIntegerParameter("ElementNumbers", "E", "Design element ID numbers", GH_ParamAccess.list);
            pManager.AddTextParameter("MaterialName", "M", "Name of the material to be applied to the design group", GH_ParamAccess.item,"");
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("DesignGroup", "G", "Design group", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string designGroupName = "";
            DA.GetData(0, ref designGroupName);
            int designGroupNumber = 0;
            DA.GetData(1, ref designGroupNumber);
            List<int> elementIds = new List<int>();
            DA.GetDataList(2, elementIds);
            string materialName = "";
            DA.GetData(3, ref materialName);
            BH.oM.Structural.Design.DesignGroup designGroup = new oM.Structural.Design.DesignGroup();
            designGroup.Name = designGroupName;
            designGroup.Number = designGroupNumber;
            designGroup.MemberIds = elementIds;
            designGroup.MaterialName = materialName;
            
            DA.SetData(0, designGroup);
        }
    }
}
