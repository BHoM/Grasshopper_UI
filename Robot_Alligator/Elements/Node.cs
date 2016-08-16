﻿using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using GHE = Grasshopper_Engine;
using BHI = BHoM.Structural.Interface;
using BHE = BHoM.Structural.Elements;


namespace Alligator.Robot.Elements
{
    public class SetNode : GH_Component
    {
        public SetNode() : base("Set Node", "ExNode", "Create a node", "Robot", "Elements") { }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "Application", "Application to import nodes from", GH_ParamAccess.item);
            pManager.AddGenericParameter("Node", "Nodes", "BHoM Node", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Activate", "Activate", "Set Geometry", GH_ParamAccess.item);
            pManager[2].AddVolatileData(new Grasshopper.Kernel.Data.GH_Path(0), 0, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Id", "Id", "Node Id", GH_ParamAccess.list); ;
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (GHE.DataUtils.Run(DA, 2))
            {
                BHI.IElementAdapter app = GHE.DataUtils.GetGenericData<BHI.IElementAdapter>(DA, 0);
                if (app != null)
                {
                    List<BHE.Node> nodes = GHE.DataUtils.GetGenericDataList<BHE.Node>(DA, 1);
                    List<string> ids = null;
                    app.SetNodes(nodes, out ids);

                    DA.SetDataList(0, ids);
                }
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("322adc1f-86b4-491a-93fa-1495515ca5aa"); }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Robot_Alligator.Properties.Resources.node; }
        }
    }
}
