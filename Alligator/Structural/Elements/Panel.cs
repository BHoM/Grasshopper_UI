﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHoM.Structural;
using Alligator.Components;
using Grasshopper.Kernel;
using GHE = Grasshopper_Engine;
using BHG = BHoM.Geometry;
using BHE = BHoM.Structural.Elements;
using BHI = BHoM.Structural.Interface;

namespace Alligator.Structural
{
    public class CreatePanel : BHoMBaseComponent<BHE.Panel>
    {
        public CreatePanel() : base("Create Panel", "CreatePanel", "Create a BH Panel object", "Alligator", "Structural") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("9E64C671-01BD-4D39-94D3-554BD2F8BA52");
            }
        }
    }

    public class SplitOpenings : GH_Component
    {
        public SplitOpenings() : base("SplitOpenings", "SplitOpenings", "Remove openings from the panel", "Alligator", "Structural") { }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Panel", "Panel", "BHoM Panel", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Panel", "Panel", "BHoM Panel", GH_ParamAccess.item);
            pManager.AddGenericParameter("Openings", "Openings", "BHoM Openings", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Getting the inputs from GH
            BHE.Panel panel = GHE.DataUtils.GetGenericData<BHE.Panel>(DA, 0);

            // Creating the panel without openings
            BHE.Panel newPanel = panel.ShallowClone() as BHE.Panel;
            BHG.Group<BHG.Curve> contour = new BHG.Group<BHG.Curve>();
            foreach (BHG.Curve curve in panel.External_Contours)
                contour.Add(curve);
            newPanel.External_Contours = contour;

            // Getting the openings
            DA.SetData(0, newPanel);
            DA.SetDataList(1, panel.Internal_Contours);
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("3C7E971B-E08B-4605-A526-E6CB3248FFBC"); }
        }

    }

    public class MultiExportPanel : GH_Component
    {
        public MultiExportPanel() : base("Multi Export Panel", "ExPanel", "Creates or Replaces the geometry of a Panel", "Alligator", "Structural") { }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "App", "Application to export bars to", GH_ParamAccess.item);
            pManager.AddGenericParameter("Panels", "P", "BHoM panels to export", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Execute", "R", "Generate Panels", GH_ParamAccess.item);

            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("Ids", "Ids", "Bar Numbers", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (GHE.DataUtils.Run(DA, 2))
            {
                BHI.IElementAdapter app = GHE.DataUtils.GetGenericData<BHI.IElementAdapter>(DA, 0);
                if (app != null)
                {
                    List<BHE.Panel> panels = GHE.DataUtils.GetGenericDataList<BHE.Panel>(DA, 1);
                    List<string> ids = null;
                    app.SetPanels(panels, out ids);

                    DA.SetDataList(0, ids);
                }
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("72fb2007-5da5-4037-b480-6e134df8c583"); }
        }
    }
}